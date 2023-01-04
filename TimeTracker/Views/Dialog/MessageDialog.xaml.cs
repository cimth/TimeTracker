using System.Windows;

namespace TimeTracker.Views.Dialog;

public partial class MessageDialog : Window
{
    // ==============
    // Initialization
    // ==============
    
    public MessageDialog()
    {
        InitializeComponent();
    }
    
    // ==============
    // Button actions
    // ==============

    private void OK_OnClick(object sender, RoutedEventArgs e)
    {
        this.DialogResult = true;
        this.Close();
    }
}