using System.Collections.ObjectModel;
using System.Windows.Input;
using TimeTracker.Dialog;
using TimeTracker.Models.Entities;
using TimeTracker.Models.Services;
using TimeTracker.Utils;
using TimeTracker.ViewModels.Command;
using TimeTracker.ViewModels.CreateUpdate;
using TimeTracker.ViewModels.Dialog;

namespace TimeTracker.ViewModels.Read;

public class ReadCategoriesViewModel
{
    // ==============
    // Properties
    // ==============
    
    public ObservableCollection<Category> Categories { get; private set; } = null!;
    
    public Category? SelectedCategory { get; set; }
    
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

        this.Categories = this._categoryService.Categories;
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
        ConfirmDialogViewModel confirmViewModel = new ConfirmDialogViewModel("Should the category really be deleted?");
        bool? result = this._dialogService.ShowConfirmDialog(confirmViewModel);

        // Remove the category if the user confirmed.
        if (result != null && result.Value)
        {
            bool success = this._categoryService.Delete(this.SelectedCategory);
            
            // Show success / error dialog.
            if (success)
            {
                MessageDialogViewModel messageViewModel = new MessageDialogViewModel("Success", "The category was successfully deleted.");
                this._dialogService.ShowMessageDialog(messageViewModel);
            }
            else
            {
                MessageDialogViewModel messageViewModel = new MessageDialogViewModel("Error", "The category cannot be deleted because it is used in at least one Entry.");
                this._dialogService.ShowMessageDialog(messageViewModel);
            }
        }
    }
}