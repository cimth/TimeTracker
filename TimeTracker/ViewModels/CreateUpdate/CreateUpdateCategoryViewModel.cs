using System;
using System.Windows.Input;
using TimeTracker.Dialog;
using TimeTracker.Models.Entities;
using TimeTracker.Models.Services;
using TimeTracker.Utils;
using TimeTracker.ViewModels.Command;

namespace TimeTracker.ViewModels.CreateUpdate;

public class CreateUpdateCategoryViewModel : NotifyPropertyChangedImpl
{
    // ==============
    // Properties
    // ==============

    public Category Category
    {
        get => this._category;
        set
        {
            SetField(ref this._category, value);
            this.UpdateStateAfterInput();
        }
    }

    public bool IsInputValid
    {
        get => this._isInputValid;
        set => SetField(ref this._isInputValid, value);
    }
    
    // ==============
    // Commands
    // ==============
    
    public ICommand SubmitCommand { get; }
    
    public ICommand UpdateStateAfterInputCommand { get; }
    
    // ==============
    // Fields
    // ==============
    
    private readonly CategoryService _categoryService;
    private readonly DialogService _dialogService;

    private Category _category = null!;
    private bool _isInputValid;

    // ==============
    // Initialization
    // ==============

    public CreateUpdateCategoryViewModel(CategoryService categoryService, DialogService dialogService)
    {
        this._categoryService = categoryService;
        this._dialogService = dialogService;

        this.SubmitCommand = new DelegateCommand(this.Submit);
        this.UpdateStateAfterInputCommand = new DelegateCommand(this.UpdateStateAfterInput);

        this.Initialize();
    }
    
    private void Initialize()
    {
        this.Category = this._categoryService.ReadAll()[1]; // new Category("New Category");
        this.IsInputValid = true;
    }
    
    // ==============
    // Command actions
    // ==============

    private void UpdateStateAfterInput()
    {
        this.IsInputValid = !string.IsNullOrWhiteSpace(this.Category.Name);
    }

    private void Submit()
    {
        // Stop if the input is invalid.
        if (!this.IsInputValid)
        {
            return;
        }
        
        // Create / update the category.
        if (this.Category.Id == null)
        {
            this.CreateCategory();
        }
        else
        {
            this.UpdateCategory();
        }
        
        // Close the dialog.
        this._dialogService.CloseCurrentDialog();
    }

    private void CreateCategory()
    {
        this._categoryService.Create(this.Category);
    }

    private void UpdateCategory()
    {
        this._categoryService.Update();
    }
}