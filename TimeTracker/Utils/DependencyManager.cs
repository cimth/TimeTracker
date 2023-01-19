using TimeTracker.Dialog;
using TimeTracker.Models.Database;
using TimeTracker.Models.Services;
using TimeTracker.ViewModels.CreateUpdate;
using TimeTracker.ViewModels.Filter;
using TimeTracker.ViewModels.Read;

namespace TimeTracker.Utils;

public class DependencyManager
{
    // ==============
    // Properties
    // ==============
    
    // Note:
    // Initialize all Properties and Fields with "null!" since they are not null after calling
    // InitializeDependencies().
    
    // Models

    public CategoryService CategoryService { get; private set; } = null!;
    public EntryService EntryService { get; private set; } = null!;

    public DialogService DialogService { get; private set; } = null!;
    
    // View Models

    public CreateUpdateCategoryViewModel CreateUpdateCategoryViewModel { get; private set; } = null!;

    public CreateUpdateEntryViewModel CreateUpdateEntryViewModel { get; private set; } = null!;

    public ReadCategoriesViewModel ReadCategoriesViewModel { get; private set; } = null!;

    public ReadEntriesViewModel ReadEntriesViewModel { get; private set; } = null!;

    public FilterEntriesViewModel FilterEntriesViewModel { get; private set; } = null!;
    
    // ==============
    // Fields
    // ==============

    private DatabaseContext _databaseContext = null!;
    
    // ==============
    // Initialization
    // ==============
    
    public void InitializeDependencies(string databasePath)
    {
        this.InitializeDatabase(databasePath);
        this.InitializeModels();
        this.InitializeViewModels();
    }
    
    private void InitializeDatabase(string databasePath)
    {
        this._databaseContext = new DatabaseContext(databasePath);
        this._databaseContext.DoMigrations();
    }

    private void InitializeModels()
    {
        this.CategoryService = new CategoryService(this._databaseContext);
        this.EntryService = new EntryService(this._databaseContext);
        this.DialogService = new DialogService();
    }

    private void InitializeViewModels()
    {
        this.CreateUpdateCategoryViewModel = new CreateUpdateCategoryViewModel(this);
        this.CreateUpdateEntryViewModel = new CreateUpdateEntryViewModel(this);
        this.ReadCategoriesViewModel = new ReadCategoriesViewModel(this);
        this.ReadEntriesViewModel = new ReadEntriesViewModel(this);
        this.FilterEntriesViewModel = new FilterEntriesViewModel(this);
    }

    public void LoadData()
    {
        this.EntryService.ReadAll();
        this.CategoryService.ReadAll();
    }
}