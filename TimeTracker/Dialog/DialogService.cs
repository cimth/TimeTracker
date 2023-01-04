using System.Windows;
using TimeTracker.Models.Entities;
using TimeTracker.ViewModels.CreateUpdate;
using TimeTracker.ViewModels.Dialog;
using TimeTracker.Views.CreateUpdate;
using TimeTracker.Views.Dialog;

namespace TimeTracker.Dialog;

public class DialogService
{
    // ==============
    // Fields
    // ==============
    
    private Window? _currentDialog;
    
    // ==============
    // Common methods
    // ==============
    
    public void CloseCurrentDialog()
    {
        this._currentDialog?.Hide();
    }
    
    // ==============
    // Create / Update Entries
    // ==============
    
    public void ShowCreateUpdateEntryDialog(CreateUpdateEntryViewModel viewModel, Entry? entry = null)
    {
        // Initialize the View Model with the selected entry or with a new entry.
        viewModel.Initialize(entry);
        
        // Open the dialog.
        this._currentDialog = new CreateUpdateEntryWindow
        {
            DataContext = viewModel
        };
        this._currentDialog.ShowDialog();
    }

    // ==============
    // Create / Update Categories
    // ==============

    public void ShowCreateUpdateCategoryDialog(CreateUpdateCategoryViewModel viewModel, Category? category = null)
    {
        // Initialize the View Model with the selected category or with a new category.
        viewModel.Initialize(category);

        // Open the dialog.
        this._currentDialog = new CreateUpdateCategoryWindow
        {
            DataContext = viewModel
        };
        this._currentDialog.ShowDialog();
    }
    
    // ==============
    // Confirm dialog
    // ==============
    
    public bool? ShowConfirmDialog(ConfirmDialogViewModel viewModel)
    {
        // Init dialog.
        this._currentDialog = new ConfirmDialog
        {
            DataContext = viewModel
        };
        
        // Show the dialog.
        return this._currentDialog.ShowDialog();
    }
    
    // ==============
    // Message dialog
    // ==============
    
    public void ShowMessageDialog(object viewModel)
    {
        // Init dialog.
        this._currentDialog = new MessageDialog()
        {
            DataContext = viewModel
        };
        
        // Show the dialog.
        this._currentDialog.ShowDialog();
    }
}