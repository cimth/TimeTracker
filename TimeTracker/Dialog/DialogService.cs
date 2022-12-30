using System.Windows;
using TimeTracker.Models.Entities;
using TimeTracker.ViewModels.CreateUpdate;
using TimeTracker.Views.CreateUpdate;

namespace TimeTracker.Dialog;

public class DialogService
{
    public void ShowCreateUpdateEntryDialog(CreateUpdateEntryViewModel viewModel, Entry? entry = null)
    {
        // Set the category to update if necessary.
        // If the category is null, the dialog is opened for creating a new category.
        if (entry != null)
        {
            viewModel.Entry = entry;
        }
        
        // Open the dialog.
        Window dialog = new CreateUpdateEntryWindow
        {
            DataContext = viewModel
        };
        dialog.ShowDialog();
    }

    public void ShowCreateUpdateCategoryDialog(CreateUpdateCategoryViewModel viewModel, Category? category = null)
    {
        // Set the category to update if necessary.
        // If the category is null, the dialog is opened for creating a new category.
        if (category != null)
        {
            viewModel.Category = category;
        }
        
        // Open the dialog.
        Window dialog = new CreateUpdateCategoryWindow
        {
            DataContext = viewModel
        };
        dialog.ShowDialog();
    }
}