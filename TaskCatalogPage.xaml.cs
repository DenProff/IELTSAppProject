
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;
using DocumentFormat.OpenXml.Drawing;

namespace IELTSAppProject
{
    /// <summary>
    /// Логика взаимодействия для TaskCatalogPage.xaml
    /// </summary>
    public partial class TaskCatalogPage : Page
    {
        
        public TaskCatalogPage()
        {
            InitializeComponent();

            this.Loaded += (sender, e) =>
            {
                this.Focus();
                this.Focusable = true;
                Keyboard.Focus(this);
            };

            this.KeyDown += (sender, e) =>
            {
                if (e.Key == Key.F1)
                {
                    OpenChmHelp();
                    e.Handled = true;
                }
            };
        }

        private void OpenChmHelp()
        {
            string chmPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Help",
                "referenceData.chm"
            );

            if (File.Exists(chmPath))
            {
                try
                {
                    // Открыть страницу "settings.html" внутри CHM
                    Process.Start("hh.exe", $"{chmPath}::/taskСollections.htm");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void turnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void statistic_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mistakes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void collections_Click(object sender, RoutedEventArgs e)
        {

        }

        private void language_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new LanguageSelectionPage());
        }

        private void convert_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addToCollection_Click(object sender, RoutedEventArgs e)
        {

        }

        private void help_Click(object sender, RoutedEventArgs e)
        {
            OpenChmHelp();
        }
    }
}
