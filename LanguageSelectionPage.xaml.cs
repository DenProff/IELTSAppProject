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
using Path = System.IO.Path;

namespace IELTSAppProject
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class LanguageSelectionPage : Page
    {
        public LanguageSelectionPage()
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

        private void RussianLanguage_Click(object sender, RoutedEventArgs e) => LanguageChange.SetLanguage("ru");

        private void ChineseLanguage_Click(object sender, RoutedEventArgs e) => LanguageChange.SetLanguage("zh-CN");

        private void EnglishLanguage_Click(object sender, RoutedEventArgs e) => LanguageChange.SetLanguage("en");

        private void SpanishLanguage_Click(object sender, RoutedEventArgs e) => LanguageChange.SetLanguage("es");

        private void TurnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
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
                    Process.Start("hh.exe", $"{chmPath}::/languageSelection.htm");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void help_Click(object sender, RoutedEventArgs e)
        {
            OpenChmHelp();
        }

        public static string[] resourcesKeysArray =
{
        "chooseCurrentLanguage",
        "espLanguage",
        "engLanguage",
        "rusLanguage",
        "chiLanguage"
        }; // Массив с ключами для ресурсов - необходимо для реализации многоязычности
    }
}
