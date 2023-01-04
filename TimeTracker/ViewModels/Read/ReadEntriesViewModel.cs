using System;
using System.Collections.ObjectModel;
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

public class ReadEntriesViewModel
{
    // ==============
    // Properties
    // ==============
    
    public ObservableCollection<Entry> Entries { get; private set; } = null!;
    
    public Entry? SelectedEntry { get; set; }
    
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

        this.Entries = this._entryService.Entries;
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
        ConfirmDialogViewModel confirmViewModel = new ConfirmDialogViewModel("Should the entry really be deleted?");
        bool? result = this._dialogService.ShowConfirmDialog(confirmViewModel);

        // Remove the Entry if the user confirmed.
        if (result != null && result.Value)
        {
            this._entryService.Delete(this.SelectedEntry);
            
            // Show success dialog.
            MessageDialogViewModel messageViewModel = new MessageDialogViewModel("Success", "The entry was successfully deleted.");
            this._dialogService.ShowMessageDialog(messageViewModel);
        }
    }
}