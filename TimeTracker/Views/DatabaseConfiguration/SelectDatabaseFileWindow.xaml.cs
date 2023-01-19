using System.Windows;
using TimeTracker.Utils;

namespace TimeTracker.Views.DatabaseConfiguration;

public partial class SelectDatabaseFileWindow : Window
{
    public SelectDatabaseFileWindow()
    {
        InitializeComponent();
        
        // Add language resources
        this.Resources.MergedDictionaries.Add(LanguageUtil.LocalizedResourceDictionary);
    }
}