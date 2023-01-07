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
        // Fields
        // ==============

        private MainWindow _mainWindow = null!;     // Not null after App_OnStartup().
        
        // ==============
        // On startup
        // ==============

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            // Initialize the current culture to have access on the correct date and time formats.
            this.UseCurrentCultureInWpf();
            
            // Create the main window first to avoid closing the application after the 'Select database file' dialog
            // is closed.
            this._mainWindow = new MainWindow();

            // Select the database to open.
            // This is mandatory to continue with starting the app. If no valid database file is selected upon
            // the initialization, the App will be closed with a information message.
            string? databasePath = this.SelectDatabaseFile();
            
            if (databasePath != null)
            {
                // Start the actual application, i.e. show the main window.
                this.ShowMainWindow(databasePath);
            }
            else
            {
                // Close the application since no valid database path was selected.
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
            // Show the dialog to select (or open/create) a database file.
            DatabaseConfigurator databaseConfigurator = new DatabaseConfigurator();
            SelectDatabaseFileViewModel viewModel = new SelectDatabaseFileViewModel(databaseConfigurator);
            this.ShowSelectDatabaseFileDialog(viewModel);
            
            // Return the database file selected in the dialog.
            return viewModel.SelectedDatabaseFile;
        }

        private void ShowSelectDatabaseFileDialog(SelectDatabaseFileViewModel viewModel)
        {
            SelectDatabaseFileWindow view = new SelectDatabaseFileWindow
            {
                DataContext = viewModel
            };
            viewModel.OnRequestClose += (_, _) => view.Close();
            view.ShowDialog();
        }

        private void ShowMainWindow(string databasePath)
        {
            MainWindowViewModel viewModel = new MainWindowViewModel(databasePath);
            this._mainWindow.DataContext = viewModel;
            this._mainWindow.Show();
        }
    }
}