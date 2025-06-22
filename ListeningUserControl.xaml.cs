using DocumentFormat.OpenXml.Drawing;
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
    public partial class ListeningUserControl : UserControl, ICheckable
    {
        public ListeningUserControl(ListeningTask data)
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

            this.DataContext = data; // Запись в Binding информации из свойств объекта data
            idTextBox.Text = $"id: {data.id}";
            recommendedTimeTextBlock.Text = $"Рекомендуемое время выполнения: {data.RecommendedTime} мин";

            listeningConvert.Click += (sender, e) => Conversion.ConvertListening(data);
            listeningConvert.Click += (sender, e) => Conversion.ConvertMessage();
        }

        public (bool, int) Check() // Данный метод должен реализовывать проверку ответов пользователя
        {
            ListeningTask task = (ListeningTask)DataContext;
            bool hasErrors = false;
            int correctAnswers = 0;
            string file;
            string projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            // Создаем массивы для хранения элементов
            RadioButton[][] radioButtonsGroups = new[]
            {
                new[] { answer1A, answer1B, answer1C },
                new[] { answer2A, answer2B, answer2C },
                new[] { answer3A, answer3B, answer3C },
                new[] { answer4A, answer4B, answer4C },
                new[] { answer5A, answer5B, answer5C }
            };

            // Создаем массивы для хранения элементов
            TextBox[] textBoxesGroups = new[]
            {
                answer6, answer7, answer8, answer9, answer0
            };

            TextBlock[] resultTextBlocks = new[]
            {
                rightAnswer1, rightAnswer2, rightAnswer3, rightAnswer4, rightAnswer5, rightAnswer6, rightAnswer7, rightAnswer8, rightAnswer9, rightAnswer0
            };

            // Проверяем все группы RadioButton в цикле
            for (int i = 0; i < 5; i++)
            {
                bool isCorrect = IsAnswerCorrect(task.Answer[i], radioButtonsGroups[i]);
                resultTextBlocks[i].Text = isCorrect ? "Right answer!" : "Wrong answer!";
                resultTextBlocks[i].Foreground = isCorrect ? Brushes.Green : Brushes.Red;
                resultTextBlocks[i].Visibility = Visibility.Visible;
                if (!isCorrect) hasErrors = true;
                else correctAnswers++;
            }

            // Проверяем все группы TextBox в цикле
            for (int i = 5; i < 10; i++)
            {
                bool isCorrect = task.Answer[i] == textBoxesGroups[i-5].Text.ToLower();
                resultTextBlocks[i].Text = isCorrect ? "Right answer!" : "Wrong answer!";
                resultTextBlocks[i].Foreground = isCorrect ? Brushes.Green : Brushes.Red;
                resultTextBlocks[i].Visibility = Visibility.Visible;
                if (!isCorrect) hasErrors = true;
                else correctAnswers++;
            }

            projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            file = Path.Combine(projectDir, "resourcesTask", "Collections", "tasksWithMistakes.json");
            string jsonData = File.ReadAllText(file);

            List<Tuple<int, string, string>> list = JsonConvert.DeserializeObject<List<Tuple<int, string, string>>>(jsonData) ?? new List<Tuple<int, string, string>>(); //десериализация файла с ошибками

            if (hasErrors)
            {
                DateTime now = DateTime.Today; //берем сегодняшнюю дату

                string today = now.ToString("dd.MM.yyyy"); //преводим ее в строку

                Tuple<int, string, string> tuple = new Tuple<int, string, string>(((ListeningTask)this.DataContext).id, ((ListeningTask)this.DataContext).TaskType, today);

                if (!list.Contains(list.FirstOrDefault(elem => elem.Item2 == "ListeningTask" && elem.Item1 == ((ListeningTask)DataContext).id))) //если в этом разделе ошибок еще нет такой подборки
                    list.Add(tuple);
            }
            if (!hasErrors && list.Any(elem => elem.Item2 == "ListeningTask" && elem.Item1 == ((ListeningTask)DataContext).id)) //если решено верно, но подборка есть в разделе ошибок
            {
                list.Remove(list.FirstOrDefault(elem => elem.Item2 == "ListeningTask" && elem.Item1 == ((ListeningTask)DataContext).id));
            }

            string updatedMistakesJson = JsonConvert.SerializeObject(list, Formatting.Indented); //сериализация файла с ошибками
            File.WriteAllText(file, updatedMistakesJson);

            // Обновление количества правильных ответов
            double[] statisticsArray = JsonControl.StatisticsArray;

            if (correctAnswers == 10)
                statisticsArray[1]++;
            statisticsArray[0] += correctAnswers;
            statisticsArray[2]++;

            file = Path.Combine(projectDir, "statistics", "", "statistics.json");
            string updatedStatisticsJson = JsonConvert.SerializeObject(statisticsArray, Formatting.Indented);
            File.WriteAllText(file, updatedStatisticsJson);

            return (hasErrors, correctAnswers);
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

        private void Check_Click(object sender, RoutedEventArgs e)
        {
            Check();
        }

        // Метод для проверки правильности ответа в RadioButton
        private bool IsAnswerCorrect(string expected, params RadioButton[] options)
        {
            return options.Any(btn => btn.IsChecked == true && (bool)btn.Content?.ToString().StartsWith(expected));
        }

        private void AddToCollection_Click(object sender, RoutedEventArgs e)
        {
            string name = ""; //название подборки

            DialogInputWindow window = new DialogInputWindow();

            window.ShowDialog(); //показ диалогового окна для ввода названия подборки

            if (window.DialogResult != true) //если пользователь нажал отмена
                return;

            name = window.UserInput;

            DateTime now = DateTime.Today; //берем сегодняшнюю дату

            string today = now.ToString("dd.MM.yyyy"); //преводим ее в строку

            //работа с json
            string projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string file = Path.Combine(projectDir, "resourcesTask", "Collections", "userCollections.json");
            string jsonData = File.ReadAllText(file);

            ListeningTask data = (ListeningTask)this.DataContext;

            List<int> idList = new List<int>() { data.id };

            //создание экземпляра TaskCollection
            TaskCollection userTask = new TaskCollection(data.id, name, today,
                         idList, true, false, false, false, false, false);

            List<TaskCollection> list = JsonConvert.DeserializeObject<List<TaskCollection>>(jsonData) ?? new List<TaskCollection>();

            list.Add(userTask);

            string updatedJson = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText(file, updatedJson);

            CollectionPage.AddMessage();
        }
    }
}
