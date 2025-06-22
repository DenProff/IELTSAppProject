using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace IELTSAppProject
{
    /// <summary>
    /// Логика взаимодействия для FirstTestPage.xaml
    /// </summary>
    public partial class FirstTestPage : Page
    {
        public FirstTestPage()
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
                SetLanguageResources.SetLanguageResourcesMethod(Properties.Settings.Default.Language, this);
            }

            // Подписка на смену языка - событие в классе LanguageChange
            LanguageChange.LanguageChanged += () => SetLanguageResources.SetLanguageResourcesMethod(Properties.Settings.Default.Language, this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<int> list = new List<int> { 0, 2, 9, 11 };
            TaskCollection task = new TaskCollection(0, "TestVariant", "12.06.2025", list, 
                true, true, true, true, false, true);
            NavigationService.Navigate(new CollectionPage(task));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ChooseModulePage());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        public void help_Click(object sender, RoutedEventArgs e)
        {
            OpenChmHelp();
        }

        private void OpenChmHelp()
        {
            string chmPath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Help",
                "referenceData.chm"
            );

            if (File.Exists(chmPath))
            {
                try
                {
                    // Открыть страницу "settings.html" внутри CHM
                    Process.Start("hh.exe", $"{chmPath}::/suggestionToSolveTestCase.htm");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
