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
        
        this.InitializeEntries();
    }
    
    private void InitializeEntries()
    {
        this.Entries = new ObservableCollection<Entry>(this._entryService.ReadAll());
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

}