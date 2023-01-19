using System.Windows;

namespace TimeTracker.Views.Dialog;

public partial class ConfirmDialog : Window
{
    // ==============
    // Initialization
    // ==============
    
    public ConfirmDialog()
    {
        InitializeComponent();
    }
    
    // ==============
    // Button actions
    // ==============

    private void Yes_OnClick(object sender, RoutedEventArgs e)
    {
        this.DialogResult = true;
        this.Close();
    }
    
    private void No_OnClick(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
        this.Close();
    }
}