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
using NAudio.Wave;
using Path = System.IO.Path;

namespace IELTSAppProject
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {

            this.Loaded += (sender, e) =>
            {
                this.Focus();
                this.Focusable = true;
                Keyboard.Focus(this);
            };

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

            InitializeComponent();

            foreach (UIElement elem in mainScreen.Children)
            {
                if (elem is Button)
                {
                    if (elem == lessTime)
                        ((Button)elem).Click += Button_Click;
                    if (elem == teacherMode)
                        ((Button)elem).Click += Button_Click_1;
                    if (elem == developerMode)
                        ((Button)elem).Click += Button_Click_2;
                    if (elem == continueWork)
                        ((Button)elem).Click += Button_Click_3;
                    if (elem == chooseLanguage)
                        ((Button)elem).Click += Button_Click_4;
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
                    Process.Start("hh.exe", $"{chmPath}::/applicationModes.htm");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TaskCatalogPage page = new TaskCatalogPage();
            page.actualTasks.IsChecked = true;
            NavigationService?.Navigate(page);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new TaskCatalogPage());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ReadingPage());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new FirstTestPage());

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Path.GetFullPath(Path.Combine(baseDir, @"..\.."));
            string audio = Path.Combine(projectRoot, "audio", "cocoJambo.mp3");
            byte[] data = SoundControl.ConvertMp3ToWavBytes(audio);
            SpeakingTask task = new SpeakingTask("Is it worth buying an apartment on the first floor?", data, 5);
            NavigationService?.Navigate(new SpeakingPage(task));
        }

        private void help_Click(object sender, RoutedEventArgs e)
        {
            OpenChmHelp();
        }
    }
}
