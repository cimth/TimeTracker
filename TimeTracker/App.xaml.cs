using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using TimeTracker.Models.DatabaseConfiguration;
using TimeTracker.ViewModels;
using TimeTracker.ViewModels.DatabaseConfiguration;
using TimeTracker.Views;
using TimeTracker.Views.DatabaseConfiguration;

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
            string? databasePath = this.SelectDatabaseFile();
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

        private string? SelectDatabaseFile()
        {
            // Initialize the dialog to select (or open/create) a database file.
            DatabaseConfigurator databaseConfigurator = new DatabaseConfigurator();
            SelectDatabaseFileViewModel viewModel = new SelectDatabaseFileViewModel(databaseConfigurator);
            SelectDatabaseFileWindow view = new SelectDatabaseFileWindow
            {
                DataContext = viewModel
            };
            
            // Call Hide() and NOT Close() when the dialog should be closed because else the whole application
            // would exit (since no other window is opened at this time).
            viewModel.OnRequestClose += (_, _) => view.Hide();
            
            // Show the dialog.
            view.ShowDialog();
            
            // Return the database file selected in the dialog.
            return viewModel.SelectedDatabaseFile;
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