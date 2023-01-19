using System;
using System.Windows;
using System.Windows.Controls;

namespace TimeTracker.Views.Filter;

public partial class FilterEntriesView : UserControl
{
    public FilterEntriesView()
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