using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using TimeTracker.Dialog;
using TimeTracker.Models;
using TimeTracker.Models.Entities;
using TimeTracker.Models.Services;
using TimeTracker.Utils;
using TimeTracker.ViewModels.Command;
using TimeTracker.ViewModels.CreateUpdate;
using TimeTracker.ViewModels.Dialog;

namespace TimeTracker.ViewModels.Read;

public class ReadEntriesViewModel : NotifyPropertyChangedImpl
{
    // ==============
    // Properties
    // ==============

    public ObservableCollection<Entry> Entries => this._entryService.Entries;

    public string TotalTimeOfShownEntries
    {
        get => this._totalTimeOfShownEntries; 
        private set => SetField(ref this._totalTimeOfShownEntries, value);
    }

    public Entry? SelectedEntry { get; set; }
    
    public bool ShowGrid
    {
        get => this._showGrid;
        set => SetField(ref this._showGrid, value);
    }

    // ==============
    // Commands
    // ==============
    
    public ICommand CreateCommand { get; }
    public ICommand UpdateCommand { get; }
    public ICommand DeleteCommand { get; }
    
    // ==============
    // Fields
    // ==============
    
    private readonly EntryService _entryService;
    private readonly DialogService _dialogService;

    private readonly CreateUpdateEntryViewModel _createUpdateEntryViewModel;
    
    private bool _showGrid;

    private string _totalTimeOfShownEntries = "00:00";

    // ==============
    // Initialization
    // ==============

    public ReadEntriesViewModel(DependencyManager dependencyManager)
    {
        this._entryService = dependencyManager.EntryService;
        this._dialogService = dependencyManager.DialogService;

        this._createUpdateEntryViewModel = dependencyManager.CreateUpdateEntryViewModel;

        this.CreateCommand = new DelegateCommand(this.Create);
        this.UpdateCommand = new DelegateCommand(this.Update);
        this.DeleteCommand = new DelegateCommand(this.Delete);

        // Update the if the grid should be shown when changing the Entries collection.
        this.Entries.CollectionChanged += this.UpdateShowGrid;
        this.UpdateShowGrid(null, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        
        // Update the total time of all current Entries when changing the Entries collection.
        this.Entries.CollectionChanged += UpdateTotalTimeOfCurrentEntries;
        this.UpdateTotalTimeOfCurrentEntries(null, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    // ==============
    // Show / hide grid
    // ==============

    private void UpdateShowGrid(object? sender, NotifyCollectionChangedEventArgs eventArgs)
    {
        this.ShowGrid = this.Entries.Count > 0;
    }
    
    // ==============
    // Update the total time of the shown entries
    // ==============
    
    private void UpdateTotalTimeOfCurrentEntries(object? sender, NotifyCollectionChangedEventArgs e)
    {
        // Sum up the total time of all current Entries.
        TimeSpan sum = TimeSpan.Zero;
        foreach (Entry entry in this.Entries)
        {
            sum = sum.Add(entry.TotalTime);
        }
        this.TotalTimeOfShownEntries = TimeSpanStringFormatter.FormatTotalHourAndMinutes(sum);
    }
    
    // ==============
    // Command actions
    // ==============
    
    private void Create()
    {
        this._dialogService.ShowCreateUpdateEntryDialog(this._createUpdateEntryViewModel);
    }

    private void Update()
    {
        this._dialogService.ShowCreateUpdateEntryDialog(this._createUpdateEntryViewModel, this.SelectedEntry);
    }
    
    private void Delete()
    {
        // Only continue if a Entry is selected.
        if (this.SelectedEntry == null)
        {
            return;
        }

        // Ask the user for confirmation.
        ConfirmDialogViewModel confirmViewModel = new ConfirmDialogViewModel("Str_ConfirmDeleteEntry");
        bool? result = this._dialogService.ShowConfirmDialog(confirmViewModel);

        // Remove the Entry if the user confirmed.
        if (result != null && result.Value)
        {
            this._entryService.Delete(this.SelectedEntry);
            
            // Show success dialog.
            MessageDialogViewModel messageViewModel = new MessageDialogViewModel("Str_Success", "Str_EntryDeletedSuccess");
            this._dialogService.ShowMessageDialog(messageViewModel);
        }
    }
}