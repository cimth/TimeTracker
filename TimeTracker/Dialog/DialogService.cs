using System.Windows;
using TimeTracker.Models.Entities;
using TimeTracker.ViewModels.CreateUpdate;
using TimeTracker.Views.CreateUpdate;

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
        // Set the entry to update if necessary.
        // If the entry is null, the dialog is opened for creating a new entry.
        if (entry != null)
        {
            viewModel.Entry = entry;
        }
        
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
        // Set the category to update if necessary.
        // If the category is null, the dialog is opened for creating a new category.
        if (category != null)
        {
            viewModel.Category = category;
        }
        
        // Open the dialog.
        this._currentDialog = new CreateUpdateCategoryWindow
        {
            DataContext = viewModel
        };
        this._currentDialog.ShowDialog();
    }
}