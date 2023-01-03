using System.Windows.Input;
using TimeTracker.Dialog;
using TimeTracker.Models.Database;
using TimeTracker.Models.Services;
using TimeTracker.Utils;
using TimeTracker.ViewModels.Command;
using TimeTracker.ViewModels.CreateUpdate;
using TimeTracker.ViewModels.Read;

namespace TimeTracker.ViewModels;

public class MainWindowViewModel : NotifyPropertyChangedImpl
{
    // ==============
    // Properties
    // ==============

    public bool IsCategoriesViewShown
    {
        get => this._isCategoriesViewShown;
        set => SetField(ref this._isCategoriesViewShown, value);
    }

    public bool IsEntriesViewShown
    {
        get => this._isEntriesViewShown;
        set => SetField(ref this._isEntriesViewShown, value);
    }

    public ReadCategoriesViewModel ReadCategoriesViewModel { get; }
    public ReadEntriesViewModel ReadEntriesViewModel { get; }
    
    // ==============
    // Commands
    // ==============

    public ICommand ShowCategoriesViewCommand { get; }
    public ICommand ShowEntriesViewCommand { get; }
    
    // ==============
    // Fields
    // ==============

    private readonly CreateUpdateEntryViewModel _createUpdateEntryViewModel;
    private readonly CreateUpdateCategoryViewModel _createUpdateCategoryViewModel;

    private readonly DialogService _dialogService;
    
    private bool _isCategoriesViewShown;
    private bool _isEntriesViewShown;

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
        
        this.ReadCategoriesViewModel = new ReadCategoriesViewModel(categoryService, this._dialogService, this._createUpdateCategoryViewModel);
        this.ReadEntriesViewModel = new ReadEntriesViewModel(entryService, this._dialogService, this._createUpdateEntryViewModel);
        
        // Initialize the commands.
        this.ShowCategoriesViewCommand = new DelegateCommand(this.ShowCategoriesView);
        this.ShowEntriesViewCommand = new DelegateCommand(this.ShowEntriesView);
        
        // Initialize the main view that is shown on startup.
        this.IsCategoriesViewShown = false;
        this.IsEntriesViewShown = true;
    }
    
    // ==============
    // Command actions
    // ==============

    public void ShowCategoriesView()
    {
        this.IsCategoriesViewShown = true;
        this.IsEntriesViewShown = false;
    }

    public void ShowEntriesView()
    {
        this.IsCategoriesViewShown = false;
        this.IsEntriesViewShown = true;
    }
}