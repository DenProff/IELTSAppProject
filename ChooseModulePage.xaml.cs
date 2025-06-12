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
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

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
            // Загрузка сохранённого язык
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Language)) // Дополнительная безопасность, чтобы если что не было исключений
            {
                SetLanguageResources.SetLanguageResourcesMethod(Properties.Settings.Default.Language, resourcesKeysArray, this);
            }

            // Подписка на смену языка - событие в классе LanguageChange
            LanguageChange.LanguageChanged += () => SetLanguageResources.SetLanguageResourcesMethod(Properties.Settings.Default.Language, resourcesKeysArray, this);
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
            NavigationService?.GoBack();
        }

        private void statistic_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new StatisticPage());
        }

        private void mistakes_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new MistakesPage());
        }

        private void collections_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new UserCollectionPage());
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
            NavigationService?.Navigate(page);
        }

        private void testVar_Click(object sender, RoutedEventArgs e)
        {
            List<int> list = new List<int> { 0, 2, 9, 11 };
            TaskCollection task = new TaskCollection(0, "TestVariant", "12.06.2025", list,
                true, true, true, true, false, true);
            NavigationService.Navigate(new CollectionPage(task));
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
        public static string[] resourcesKeysArray =
        {
        "myStatistics",
        "myMistakes",
        "myCollections",
        "chooseLanguage",
        "help",
        "prevPage"
        }; // Массив с ключами для ресурсов - необходимо для реализации многоязычности
    }
}
