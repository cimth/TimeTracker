using System.Windows;
using TimeTracker.Utils;

namespace TimeTracker.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // Add language resources
            this.Resources.MergedDictionaries.Add(LanguageUtil.LocalizedResourceDictionary);
        }
    }
}