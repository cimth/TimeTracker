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

}