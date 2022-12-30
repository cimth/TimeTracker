using System.Windows.Input;
using TimeTracker.Dialog;
using TimeTracker.Models.Database;
using TimeTracker.Models.Services;
using TimeTracker.ViewModels.Command;
using TimeTracker.ViewModels.CreateUpdate;

namespace TimeTracker.ViewModels;

public class MainWindowViewModel
{
    // ==============
    // Commands
    // ==============

    public ICommand ShowCreateUpdateEntryDialogCommand { get; }
    public ICommand ShowCreateUpdateCategoryDialogCommand { get; }
    
    // ==============
    // Fields
    // ==============

    private CreateUpdateCategoryViewModel _createUpdateCategoryViewModel;

    private DialogService _dialogService;

    // ==============
    // Initialization
    // ==============
    
    public MainWindowViewModel()
    {
        // Create the Model instances that are shared by the View Models.
        DatabaseContext dbContext = new DatabaseContext();
        dbContext.DoMigrations();
        
        CategoryService categoryService = new CategoryService(dbContext);

        this._dialogService = new DialogService();
        
        // Create the View Models.
        this._createUpdateCategoryViewModel = new CreateUpdateCategoryViewModel(categoryService);
        
        // Initialize the Commands of this View Model.
        this.ShowCreateUpdateEntryDialogCommand = new DelegateCommand(ShowCreateUpdateEntryDialog);
        this.ShowCreateUpdateCategoryDialogCommand = new DelegateCommand(ShowCreateUpdateCategoryDialog);
    }
    
    // ==============
    // Command actions
    // ==============

    private void ShowCreateUpdateEntryDialog()
    {
        this._dialogService.ShowCreateUpdateEntryDialog(null);
    }

    private void ShowCreateUpdateCategoryDialog()
    {
        this._dialogService.ShowCreateUpdateCategoryDialog(this._createUpdateCategoryViewModel);
    }
}