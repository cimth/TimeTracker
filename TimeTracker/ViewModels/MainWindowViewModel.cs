using System.Windows;
using System.Windows.Input;
using TimeTracker.Dialog;
using TimeTracker.ViewModels.Command;
using TimeTracker.Views.CreateUpdate;

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

    private readonly DialogService _dialogService;

    // ==============
    // Initialization
    // ==============
    
    public MainWindowViewModel()
    {
        this._dialogService = new DialogService();
        
        // Initialize the Commands of this View Model.
        this.ShowCreateUpdateEntryDialogCommand = new DelegateCommand(ShowCreateUpdateEntryDialog);
        this.ShowCreateUpdateCategoryDialogCommand = new DelegateCommand(ShowCreateUpdateCategoryDialog);
    }
    
    // ==============
    // Command actions
    // ==============

    private void ShowCreateUpdateEntryDialog()
    {
        Window dialog = new CreateUpdateEntryWindow();
        this._dialogService.ShowDialog(dialog, null);
    }

    private void ShowCreateUpdateCategoryDialog()
    {
        Window dialog = new CreateUpdateCategoryWindow();
        this._dialogService.ShowDialog(dialog, null);
    }
}