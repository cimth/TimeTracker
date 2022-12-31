using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace TimeTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            this.UseCurrentCultureInWpf();
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
    }
}