using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TimeTracker.Dialog;
using TimeTracker.Models.Entities;
using TimeTracker.Models.Services;
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

    public ReadCategoriesViewModel(CategoryService categoryService, 
                                   DialogService dialogService,
                                   CreateUpdateCategoryViewModel createUpdateCategoryViewModel)
    {
        this._categoryService = categoryService;
        this._dialogService = dialogService;

        this._createUpdateCategoryViewModel = createUpdateCategoryViewModel;

        this.UpdateCommand = new DelegateCommand(this.Update);
        
        this.InitializeCategories();
    }
    
    private void InitializeCategories()
    {
        this.Categories = new ObservableCollection<Category>(this._categoryService.ReadAll());
    }
    
    // ==============
    // Command actions
    // ==============

    private void Update()
    {
        Console.WriteLine($"Update: {this.SelectedCategory?.Name}");
        this._dialogService.ShowCreateUpdateCategoryDialog(this._createUpdateCategoryViewModel, this.SelectedCategory);
    }

}