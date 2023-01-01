﻿using System.Windows.Input;
using TimeTracker.Dialog;
using TimeTracker.Models.Database;
using TimeTracker.Models.Services;
using TimeTracker.ViewModels.Command;
using TimeTracker.ViewModels.CreateUpdate;
using TimeTracker.ViewModels.Read;

namespace TimeTracker.ViewModels;

public class MainWindowViewModel
{
    // ==============
    // Properties
    // ==============
    
   public ReadCategoriesViewModel ReadCategoriesViewModel { get; }
    
    // ==============
    // Commands
    // ==============

    public ICommand ShowCreateUpdateEntryDialogCommand { get; }
    public ICommand ShowCreateUpdateCategoryDialogCommand { get; }
    
    // ==============
    // Fields
    // ==============

    private readonly CreateUpdateEntryViewModel _createUpdateEntryViewModel;
    private readonly CreateUpdateCategoryViewModel _createUpdateCategoryViewModel;

    private readonly DialogService _dialogService;

    // ==============
    // Initialization
    // ==============
    
    public MainWindowViewModel()
    {
        // Create the Model instances that are shared by the View Models.
        DatabaseContext dbContext = new DatabaseContext();
        dbContext.DoMigrations();
        
        EntryService entryService = new EntryService(dbContext);
        CategoryService categoryService = new CategoryService(dbContext);

        this._dialogService = new DialogService();
        
        // Create the View Models.
        this._createUpdateEntryViewModel = new CreateUpdateEntryViewModel(entryService, categoryService, this._dialogService);
        this._createUpdateCategoryViewModel = new CreateUpdateCategoryViewModel(categoryService, this._dialogService);
        
        this.ReadCategoriesViewModel = new ReadCategoriesViewModel(categoryService);
        
        // Initialize the Commands of this View Model.
        this.ShowCreateUpdateEntryDialogCommand = new DelegateCommand(ShowCreateUpdateEntryDialog);
        this.ShowCreateUpdateCategoryDialogCommand = new DelegateCommand(ShowCreateUpdateCategoryDialog);
    }
    
    // ==============
    // Command actions
    // ==============

    private void ShowCreateUpdateEntryDialog()
    {
        this._dialogService.ShowCreateUpdateEntryDialog(this._createUpdateEntryViewModel);
    }

    private void ShowCreateUpdateCategoryDialog()
    {
        this._dialogService.ShowCreateUpdateCategoryDialog(this._createUpdateCategoryViewModel);
    }
}