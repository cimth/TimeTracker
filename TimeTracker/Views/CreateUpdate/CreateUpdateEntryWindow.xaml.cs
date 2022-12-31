using System;
using System.Windows;
using System.Windows.Controls;

namespace TimeTracker.Views.CreateUpdate;

public partial class CreateUpdateEntryWindow : Window
{
    public CreateUpdateEntryWindow()
    {
        InitializeComponent();
    }

    private void TextBox_OnGotFocus(object sender, RoutedEventArgs e)
    {
        // Select complete input text when focused
        if (sender is TextBox tb)
        {
            tb.Dispatcher.BeginInvoke(new Action(() => tb.SelectAll()));
        }
    }
}