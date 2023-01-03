using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TimeTracker.Dialog;
using TimeTracker.Models.Entities;
using TimeTracker.Models.Services;
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

    public ReadEntriesViewModel(EntryService entryService, 
                                DialogService dialogService,
                                CreateUpdateEntryViewModel createUpdateEntryViewModel)
    {
        this._entryService = entryService;
        this._dialogService = dialogService;

        this._createUpdateEntryViewModel = createUpdateEntryViewModel;

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

    private void Update()
    {
        this._dialogService.ShowCreateUpdateEntryDialog(this._createUpdateEntryViewModel, this.SelectedEntry);
    }

}