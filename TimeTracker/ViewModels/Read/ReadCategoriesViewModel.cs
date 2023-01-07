using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using TimeTracker.Dialog;
using TimeTracker.Models.Entities;
using TimeTracker.Models.Services;
using TimeTracker.Utils;
using TimeTracker.ViewModels.Command;
using TimeTracker.ViewModels.CreateUpdate;
using TimeTracker.ViewModels.Dialog;

namespace TimeTracker.ViewModels.Read;

public class ReadCategoriesViewModel : NotifyPropertyChangedImpl
{
    // ==============
    // Properties
    // ==============

    public ObservableCollection<Category> Categories => this._categoryService.Categories;

    public Category? SelectedCategory { get; set; }
    
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
    
    private readonly CategoryService _categoryService;
    private readonly DialogService _dialogService;

    private readonly CreateUpdateCategoryViewModel _createUpdateCategoryViewModel;
    
    private bool _showGrid;

    // ==============
    // Initialization
    // ==============

    public ReadCategoriesViewModel(DependencyManager dependencyManager)
    {
        this._categoryService = dependencyManager.CategoryService;
        this._dialogService = dependencyManager.DialogService;

        this._createUpdateCategoryViewModel = dependencyManager.CreateUpdateCategoryViewModel;

        this.CreateCommand = new DelegateCommand(this.Create);
        this.UpdateCommand = new DelegateCommand(this.Update);
        this.DeleteCommand = new DelegateCommand(this.Delete);

        this.Categories.CollectionChanged += this.UpdateShowGrid;
        this.UpdateShowGrid(null, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
    
    // ==============
    // Show / hide grid
    // ==============

    private void UpdateShowGrid(object? sender, NotifyCollectionChangedEventArgs eventArgs)
    {
        this.ShowGrid = this.Categories.Count > 0;
    }
    
    // ==============
    // Command actions
    // ==============
    
    private void Create()
    {
        this._dialogService.ShowCreateUpdateCategoryDialog(this._createUpdateCategoryViewModel);
    }

    private void Update()
    {
        this._dialogService.ShowCreateUpdateCategoryDialog(this._createUpdateCategoryViewModel, this.SelectedCategory);
    }

    private void Delete()
    {
        // Only continue if a category is selected.
        if (this.SelectedCategory == null)
        {
            return;
        }

        // Ask the user for confirmation.
        ConfirmDialogViewModel confirmViewModel = new ConfirmDialogViewModel("Str_ConfirmDeleteCategory");
        bool? result = this._dialogService.ShowConfirmDialog(confirmViewModel);

        // Remove the category if the user confirmed.
        if (result != null && result.Value)
        {
            bool success = this._categoryService.Delete(this.SelectedCategory);
            
            // Show success / error dialog.
            if (success)
            {
                MessageDialogViewModel messageViewModel = new MessageDialogViewModel("Str_Success", "Str_CategoryDeletedSuccess");
                this._dialogService.ShowMessageDialog(messageViewModel);
            }
            else
            {
                MessageDialogViewModel messageViewModel = new MessageDialogViewModel("Str_Error", "Str_CategoryNotDeletedBecauseInUse");
                this._dialogService.ShowMessageDialog(messageViewModel);
            }
        }
    }
}