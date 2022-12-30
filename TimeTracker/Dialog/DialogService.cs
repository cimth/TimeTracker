using System.Windows;

namespace TimeTracker.Dialog;

public class DialogService
{
    public void ShowDialog(Window dialogView, object viewModel)
    {
        dialogView.DataContext = viewModel;
        dialogView.ShowDialog();
    }
}