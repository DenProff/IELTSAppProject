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
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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

            //List<int> list1 = new List<int> { 0, 1 };
            //List<int> list2 = new List<int> { 1, 6 };
            //List<int> list3 = new List<int> { 0, 6 };
            //List<int> list4 = new List<int> { 3, 7 };
            //List<int> list5 = new List<int> { 0, 2, 7 };

            //string projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            //string file = Path.Combine(projectDir, "resourcesTask", "Collections", "taskCollections.json");
            //string jsonData = File.ReadAllText(file);

            //List<TaskCollection> list = new List<TaskCollection>();

            //TaskCollection collection1 = new TaskCollection(1, "Райт + рид", "09.06.2025", list1, false, 
            //    false, true, true, false, false);
            //TaskCollection collection2 = new TaskCollection(2, "Рид + лист", "09.06.2025", list2, true, 
            //    false, false, true, true, false);
            //TaskCollection collection3 = new TaskCollection(3, "Райт + лист", "09.06.2025", list3, true,
            //    false, true, false, false, false);
            //TaskCollection collection4 = new TaskCollection(4, "Рид + лист2", "09.06.2025", list4, true,
            //    false, false, true, false, false);
            //TaskCollection collection5 = new TaskCollection(5, "Райт + рид + лист", "09.06.2025", list5, true,
            //    false, true, true, false, false);

            //list.Add(collection1);
            //list.Add(collection2);
            //list.Add(collection3);
            //list.Add(collection4);
            //list.Add(collection5);


            //string shit = JsonConvert.SerializeObject(list, Formatting.Indented);
            //File.WriteAllText(file, shit);


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
            page.speakCheackBox.IsChecked = true;
            page.readingCheckBox.IsChecked = true;
            page.writingCheckBox.IsChecked = true;
            page.listeningCheckBox.IsChecked = true;
            NavigationService?.Navigate(page);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new TaskCatalogPage());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ReadingUserControl(null));
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // NavigationService?.Navigate(new FirstTestPage());
            NavigationService?.Navigate(new WritingUserControl(new WritingTask(0, "WritingTask", "Текст задание это статическая константа", 40, new string[]{"idealAns1", "idealAns2", "idealAns3", "idealAns4", "idealAns5", "idealAns6"})));
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            //NavigationService?.Navigate(new CollectionPage(new TaskCollection(5, "zalupa variant", "date", new List<int>{ 0, 1, 2 })));
            //NavigationService?.Navigate(new LanguageSelectionPage());
        }

        private void help_Click(object sender, RoutedEventArgs e)
        {
            OpenChmHelp();
        }
    }
}
