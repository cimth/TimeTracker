using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using TimeTracker.ViewModels;
using TimeTracker.Views;

namespace TimeTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // ==============
        // On startup
        // ==============

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            // Initialize the current culture to have access on the correct date and time formats.
            this.UseCurrentCultureInWpf();

            // Select the database to open.
            // This is mandatory to continue with starting the app. If no valid database file is selected upon
            // the initialization, the App will be closed with a information message.
            string? databasePath = this.SelectDatabase();
            if (databasePath != null)
            {
                this.ShowMainWindow(databasePath);
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        /**
         * Uses the current culture for WPF so that e.g. date and time formats are represented as the User
         * expects them.
         * 
         * See:
         * https://stackoverflow.com/questions/1062160/wpf-xaml-stringformat-datetime-output-in-wrong-culture
         */
        private void UseCurrentCultureInWpf()
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));   
        }

        private string? SelectDatabase()
        {
            // TODO: Show dialog to select a database file
            return "Data.db";
        }

        private void ShowMainWindow(string databasePath)
        {
            MainWindowViewModel viewModel = new MainWindowViewModel(databasePath);
            MainWindow mainWindow = new MainWindow
            {
                DataContext = viewModel
            };
            mainWindow.Show();
        }
    }
}