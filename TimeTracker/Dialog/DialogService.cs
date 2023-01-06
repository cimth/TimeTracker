using System.Windows;
using TimeTracker.Models.Entities;
using TimeTracker.Utils;
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
        
        // Initialize the dialog. Explicitly add resources to make the localized strings accessible.
        this._currentDialog = new CreateUpdateEntryWindow
        {
            DataContext = viewModel
        };
        this._currentDialog.Resources.MergedDictionaries.Add(LanguageUtil.LocalizedResourceDictionary);
        
        // Show the dialog.
        this._currentDialog.ShowDialog();
    }

    // ==============
    // Create / Update Categories
    // ==============

    public void ShowCreateUpdateCategoryDialog(CreateUpdateCategoryViewModel viewModel, Category? category = null)
    {
        // Initialize the View Model with the selected category or with a new category.
        viewModel.Initialize(category);

        // Initialize the dialog. Explicitly add resources to make the localized strings accessible.
        this._currentDialog = new CreateUpdateCategoryWindow
        {
            DataContext = viewModel
        };
        this._currentDialog.Resources.MergedDictionaries.Add(LanguageUtil.LocalizedResourceDictionary);
        
        // Show the dialog.
        this._currentDialog.ShowDialog();
    }
    
    // ==============
    // Confirm dialog
    // ==============
    
    public bool? ShowConfirmDialog(ConfirmDialogViewModel viewModel)
    {
        // Initialize the dialog. Explicitly add resources to make the localized strings accessible.
        Window dialog = new ConfirmDialog
        {
            DataContext = viewModel
        };
        dialog.Resources.MergedDictionaries.Add(LanguageUtil.LocalizedResourceDictionary);
        
        // Show the dialog.
        return dialog.ShowDialog();
    }
    
    // ==============
    // Message dialog
    // ==============
    
    public void ShowMessageDialog(object viewModel)
    {
        // Initialize the dialog. Explicitly add resources to make the localized strings accessible.
        Window dialog = new MessageDialog()
        {
            DataContext = viewModel
        };
        dialog.Resources.MergedDictionaries.Add(LanguageUtil.LocalizedResourceDictionary);
        
        // Show the dialog.
        dialog.ShowDialog();
    }
}