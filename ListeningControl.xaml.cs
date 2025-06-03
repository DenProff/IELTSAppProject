using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using Newtonsoft.Json;
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
    /// Логика взаимодействия для ListeningControl.xaml
    /// </summary>
    public partial class ListeningControl : UserControl, ICheckable
    {
        public ListeningControl()
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

            ListeningTask newTask = null;

            try
            {
                string projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                string file = Path.Combine(projectDir, "resourcesTask", "tasks", "listeningtask1.json");
                if (!File.Exists(file))
                {
                    MessageBox.Show("Файл не найден!");
                    return;
                }

                // Читаем JSON из файла
                string jsonFromFile = File.ReadAllText(file);

                // Десериализуем обратно в объект
                newTask = JsonConvert.DeserializeObject<ListeningTask>(jsonFromFile);

                this.DataContext = newTask;
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Ошибка доступа к файлу: {ex.Message}");
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Ошибка формата JSON: {ex.Message}");
            }
        }

        private void SimpleAudioPlayer_Loaded(object sender, RoutedEventArgs e)
        {

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
                    Process.Start("hh.exe", $"{chmPath}::/listening.htm");
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

        public bool Check() // Данный метод должен реализовывать проверку ответов пользователя
        {
            throw new NotImplementedException();
        }
    }
}
