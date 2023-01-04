using System.Windows.Input;
using TimeTracker.Dialog;
using TimeTracker.Models;
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

    public string WindowTitle
    {
        get => this._windowTitle;
        set => SetField(ref this._windowTitle, value);
    }

    public string SubmitButtonText
    {
        get => this._submitButtonText;
        set => SetField(ref this._submitButtonText, value);
    }
    
    /*
     * Input values
     */

    public string InputName
    {
        get => this._inputName;
        set => SetField(ref this._inputName, value);
    }
    
    /*
     * Input validation
     */

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

    private string _windowTitle = null!;            // Not null after Initialize().
    private string _submitButtonText = null!;       // Not null after Initialize().

    private Category? _originalCategory;
    
    // Input values
    private string _inputName = null!;              // Not null after Initialize().
    
    // Input validation
    private bool _isInputValid;

    // ==============
    // Initialization
    // ==============

    public CreateUpdateCategoryViewModel(DependencyManager dependencyManager)
    {
        this._categoryService = dependencyManager.CategoryService;
        this._dialogService = dependencyManager.DialogService;

        this.SubmitCommand = new DelegateCommand(this.Submit);
        this.UpdateStateAfterInputCommand = new DelegateCommand(this.UpdateStateAfterInput);
    }
    
    public void Initialize(Category? category = null)
    {
        // Save the original category (might be null if a new entry should be created).
        // The GUI does not bind to the original category but to separate variables for each input field
        // to avoid changing the original data even if the changes are not saved.
        this._originalCategory = category;
        
        // Adjust the GUI texts depending on whether creating or updating an entry.
        this.WindowTitle = category == null ? LanguageUtil.GiveLocalizedString("Str_CreateCategory") : LanguageUtil.GiveLocalizedString("Str_UpdateCategory");
        this.SubmitButtonText = category == null ? LanguageUtil.GiveLocalizedString("Str_Create") : LanguageUtil.GiveLocalizedString("Str_Save");
        
        // Initialize the data bound to the GUI.
        this.InitializeInput();
        this.UpdateStateAfterInput();
    }
    
    private void InitializeInput()
    {
        if (this._originalCategory == null)
        {
            this.InitializeCreateInput();
        }
        else
        {
            this.InitializeUpdateInput(this._originalCategory);
        }
    }
    
    private void InitializeCreateInput()
    {
        this.InputName = LanguageUtil.GiveLocalizedString("Str_NewCategory");
    }

    private void InitializeUpdateInput(Category category)
    {
        this.InputName = category.Name;
    }
    
    // ==============
    // Command actions
    // ==============

    private void UpdateStateAfterInput()
    {
        this.IsInputValid = !string.IsNullOrWhiteSpace(this.InputName);
    }

    private void Submit()
    {
        // Stop if the input is invalid.
        if (!this.IsInputValid)
        {
            return;
        }
        
        // Create / update the category.
        if (this._originalCategory == null)
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
        Category category = this.GetCategoryFromInput();
        this._categoryService.Create(category);
    }

    private void UpdateCategory()
    {
        // Get the updated data and save them into the original entry.
        Category updatedCategory = this.GetCategoryFromInput();

        this._originalCategory!.Name = updatedCategory.Name;
        
        // Save the changes into the database.
        this._categoryService.Update();
    }

    private Category GetCategoryFromInput()
    {
        return new Category(this.InputName);
    }
}