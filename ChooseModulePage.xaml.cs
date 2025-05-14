using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace IELTSAppProject
{
    /// <summary>
    /// Логика взаимодействия для ChooseModulePage.xaml
    /// </summary>
    public partial class ChooseModulePage : Page
    {
        public ChooseModulePage()
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

            foreach (UIElement elem in chosingModule.Children)
            {
                if (elem is Button)
                {
                    if (elem == turnBack)
                        ((Button)elem).Click += turnBack_Click;
                    if (elem ==statistic)
                        ((Button)elem).Click += statistic_Click;
                    if (elem == mistakes)
                        ((Button)elem).Click += mistakes_Click;
                    if (elem == collections)
                        ((Button)elem).Click += collections_Click;
                    if (elem == language)
                        ((Button)elem).Click += language_Click;
                    if (elem == varOfExam)
                        ((Button)elem).Click += varOfExam_Click;
                    if (elem == newCollections)
                        ((Button)elem).Click += newCollections_Click;
                    if (elem == testVar)
                        ((Button)elem).Click += testVar_Click;
                    if (elem == moduleTasks)
                        ((Button)elem).Click += moduleTasks_Click;


                }
            }

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
                    Process.Start("hh.exe", $"{chmPath}::/generalInformation.htm");
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
            NavigationService?.Navigate(new MainPage());
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

        private void varOfExam_Click(object sender, RoutedEventArgs e)
        {
            TaskCatalogPage page = new TaskCatalogPage();
            page.varOfExam.IsChecked = true;
            NavigationService?.Navigate(page);
        }

        private void newCollections_Click(object sender, RoutedEventArgs e)
        {
            TaskCatalogPage page = new TaskCatalogPage();
            page.collectionsOfTasks.IsChecked = true;
            NavigationService?.Navigate(page);
        }

        private void testVar_Click(object sender, RoutedEventArgs e)
        {
            WritingPage page = new WritingPage();
            NavigationService?.Navigate(page);
        }

        private void moduleTasks_Click(object sender, RoutedEventArgs e)
        {
            TaskCatalogPage page = new TaskCatalogPage();
            page.speakCheackBox.IsChecked = true;
            page.readingCheckBox.IsChecked = true;
            page.writingCheckBox.IsChecked = true;
            page.listeningCheckBox.IsChecked = true;
            NavigationService?.Navigate(page);
        }

        public void help_Click(object sender, RoutedEventArgs e)
        {
            OpenChmHelp();
        }
    }
}
