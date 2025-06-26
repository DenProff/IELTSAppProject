using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using System.Diagnostics;
using Path = System.IO.Path;
using DocumentFormat.OpenXml.Office2013.Word;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Windows.Media;

namespace IELTSAppProject
{
    /// <summary>
    /// Логика взаимодействия для ReadingControl.xaml
    /// </summary>
    public partial class ReadingUserControl : UserControl, ICheckable
    {

        public string RecomTime { get; set; }
        public ReadingUserControl(ReadingTask data)
        {
            InitializeComponent();

            this.Loaded += (sender, e) =>
            {
                this.Focus();
                this.Focusable = true;
                Keyboard.Focus(this);
            };

            // Загрузка сохранённого язык
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Language)) // Дополнительная безопасность, чтобы если что не было исключений
            {
                SetLanguageResources.SetLanguageResourcesMethod(Properties.Settings.Default.Language, this);
            }

            // Подписка на смену языка - событие в классе LanguageChange
            LanguageChange.LanguageChanged += () => SetLanguageResources.SetLanguageResourcesMethod(Properties.Settings.Default.Language, this);

            ////////запись в json
            //List<string> first = new List<string>();
            //List<string> second = new List<string>();
            //List<string> third = new List<string>();
            //List<string> fourth = new List<string>();
            //List<string> fifth = new List<string>();
            //List<string> answer = new List<string>();

            //AddToList(ref first, "they are a bacterial immune system", "they are DNA from viruses", "they aren't bacterial in origin");
            //AddToList(ref second, "biologists", "geneticists", "biologists and geneticists");
            //AddToList(ref third, "determines", "gains awarness", "adapts");
            //AddToList(ref fourth, "long history of existence", "immortality", "heritability");
            //AddToList(ref fifth, "It requires no energy to function.",
            //    "It provides immunity to offspring without prior exposure.", "It works only in laboratory conditions.");
            //AddToList(ref answer, "True", "False", "True", "False", "Not Stated", "they aren't bacterial in origin",
            //    "biologists and geneticists", "gains awarness", "heritability",
            //    "It provides immunity to offspring without prior exposure.");

            //string projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            //string file = Path.Combine(projectDir, "resourcesTask", "tasks", "tasks.json");
            //string jsonData = File.ReadAllText(file);

            //List<GeneralizedTask> list = JsonConvert.DeserializeObject<List<GeneralizedTask>>(jsonData) ?? new List<GeneralizedTask>();

            //list.Add(task);

            //string updatedJson = JsonConvert.SerializeObject(list, Formatting.Indented);
            //File.WriteAllText(file, updatedJson);

            if (data != null)
            {
                this.RecomTime = data.RecommendedTime.ToString() + " мин.";
               
                this.DataContext = data;
            }

            //подписка на событие клика
            convertToDocx.Click += (sender, e) => Conversion.ConvertReading(data);
            convertToDocx.Click += (sender, e) => Conversion.ConvertMessage();
        }
        
        //проверка ответов
        private bool IsAnswerCorrect(string expected, params RadioButton[] options)
        {
            return options.Any(btn => btn.IsChecked == true && btn.Content?.ToString() == expected);
        }

        //открытие справки
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
                    // Открыть страницу "reading.htm" внутри CHM
                    Process.Start("hh.exe", $"{chmPath}::/reading.htm");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e) //добавление в подборку
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
            string projectDir = AppDomain.CurrentDomain.BaseDirectory;
            string file = Path.Combine(projectDir, "resourcesTask", "Collections", "userCollections.json");
            string jsonData = File.ReadAllText(file);
            if (!File.Exists(file))
            {
                MessageBox.Show("Файл не найден!");
                return;
            }

            ReadingTask data = (ReadingTask)this.DataContext;

            List<int> idList = new List<int>() { data.id };

            //создание экземпляра TaskCollection
            TaskCollection userTask = new TaskCollection(data.id, name, today,
                         idList, false, false, false, true, false, false);

            List<TaskCollection> list = JsonConvert.DeserializeObject<List<TaskCollection>>(jsonData) ?? new List<TaskCollection>();

            list.Add(userTask);

            string updatedJson = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText(file, updatedJson);

            CollectionPage.AddMessage();
        }

        //добавление в списки
        //public static void AddToList(ref List<string> list, params string[] answers)
        //{
        //    for (int i = 0; i < answers.Length; i++)
        //        list.Add(answers[i]);
        //}

        //открытие справки
        private void help_Click(object sender, RoutedEventArgs e)
        {
            OpenChmHelp();
        }

        //проверка ответов
        public (bool, int) Check()
        {
            ReadingTask task = (ReadingTask)this.DataContext;
            bool result = false;
            int correctAnswers = 0;
            string file;
            if (task.Answer[0] == answer1.Text)
            {
                rightAnswer1.Foreground = Brushes.Green;
                rightAnswer1.Text = "Right answer!";
                correctAnswers++;
            }
            else
            {
                rightAnswer1.Foreground = Brushes.Red;
                rightAnswer1.Text = "Wrong answer!";
                result = true;
            }


            if (task.Answer[1] == answer2.Text)
            {
                rightAnswer2.Foreground = Brushes.Green;
                rightAnswer2.Text = "Right answer!";
                correctAnswers++;
            }
            else
            {
                rightAnswer2.Foreground = Brushes.Red;
                rightAnswer2.Text = "Wrong answer!";
                result = true;
            }


            if (task.Answer[2] == answer3.Text)
            {
                rightAnswer3.Foreground = Brushes.Green;
                rightAnswer3.Text = "Right answer!";
                correctAnswers++;
            }
            else
            {
                rightAnswer3.Foreground = Brushes.Red;
                rightAnswer3.Text = "Wrong answer!";
                result = true;
            }


            if (task.Answer[3] == answer4.Text)
            {
                rightAnswer4.Foreground = Brushes.Green;
                rightAnswer4.Text = "Right answer!";
                correctAnswers++;
            }
            else
            {
                rightAnswer4.Foreground = Brushes.Red;
                rightAnswer4.Text = "Wrong answer!";
                result = true;
            }


            if (task.Answer[4] == answer5.Text)
            {
                rightAnswer5.Foreground = Brushes.Green;
                rightAnswer5.Text = "Right answer!";
                correctAnswers++;
            }
            else
            {
                rightAnswer5.Foreground = Brushes.Red;
                rightAnswer5.Text = "Wrong answer!";
                result = true;
            }


            if (IsAnswerCorrect(task.Answer[5], answer61, answer62, answer63))
            {
                rightAnswer6.Foreground = Brushes.Green;
                rightAnswer6.Text = "Right answer!";
                correctAnswers++;
            }
            else
            {
                rightAnswer6.Foreground = Brushes.Red;
                result = true;
                rightAnswer6.Text = "Wrong answer!";
            }


            if (IsAnswerCorrect(task.Answer[6], answer71, answer72, answer73))
            {
                rightAnswer7.Foreground = Brushes.Green;
                rightAnswer7.Text = "Right answer!";
                correctAnswers++;
            }
            else
            {
                rightAnswer7.Foreground = Brushes.Red;
                rightAnswer7.Text = "Wrong answer!";
                result = true;
            }

            if (IsAnswerCorrect(task.Answer[7], answer81, answer82, answer83))
            {
                rightAnswer8.Foreground = Brushes.Green;
                rightAnswer8.Text = "Right answer!";
                correctAnswers++;
            }
            else
            {
                rightAnswer8.Foreground = Brushes.Red;
                rightAnswer8.Text = "Wrong answer!";
                result = true;
            }

            if (IsAnswerCorrect(task.Answer[8], answer91, answer92, answer93))
            {
                rightAnswer9.Foreground = Brushes.Green;
                rightAnswer9.Text = "Right answer!";
                correctAnswers++;
            }
            else
            {
                rightAnswer9.Foreground = Brushes.Red;
                rightAnswer9.Text = "Wrong answer!";
                result = true;
            }

            if (IsAnswerCorrect(task.Answer[9], answer01, answer02, answer03))
            {
                rightAnswer0.Foreground = Brushes.Green;
                rightAnswer0.Text = "Right answer!";
                correctAnswers++;
            }
            else
            {
                rightAnswer0.Foreground = Brushes.Red;
                rightAnswer0.Text = "Wrong answer!";
                result = true;
            }

            this.rightAnswer0.Visibility = Visibility.Visible;
            this.rightAnswer9.Visibility = Visibility.Visible;
            this.rightAnswer8.Visibility = Visibility.Visible;
            this.rightAnswer7.Visibility = Visibility.Visible;
            this.rightAnswer6.Visibility = Visibility.Visible;
            this.rightAnswer5.Visibility = Visibility.Visible;
            this.rightAnswer4.Visibility = Visibility.Visible;
            this.rightAnswer3.Visibility = Visibility.Visible;
            this.rightAnswer2.Visibility = Visibility.Visible;
            this.rightAnswer1.Visibility = Visibility.Visible;

            string projectDir = AppDomain.CurrentDomain.BaseDirectory;
            file = Path.Combine(projectDir, "resourcesTask", "Collections", "tasksWithMistakes.json");
            string jsonData = File.ReadAllText(file);

            List<Tuple<int, string, string>> list = JsonConvert.DeserializeObject<List<Tuple<int, string, string>>>(jsonData) ?? new List<Tuple<int, string, string>>(); //десериализация файла с ошибками

            if (result)
            {
                DateTime now = DateTime.Today; //берем сегодняшнюю дату

                string today = now.ToString("dd.MM.yyyy"); //преводим ее в строку

                Tuple<int, string, string> tuple = new Tuple<int, string, string>(((ReadingTask)this.DataContext).id, ((ReadingTask)this.DataContext).TaskType, today);

                if (!list.Contains(list.FirstOrDefault(elem => elem.Item2 == "ReadingTask" && elem.Item1 == ((ReadingTask)DataContext).id))) //если в этом разделе ошибок еще нет такой подборки
                    list.Add(tuple);
            }
            if (!result && list.Any(elem => elem.Item2 == "ReadingTask" && elem.Item1 == ((ReadingTask)DataContext).id)) //если решено верно, но подборка есть в разделе ошибок
            {
                list.Remove(list.FirstOrDefault(elem => elem.Item2 == "ReadingTask" && elem.Item1 == ((ReadingTask)DataContext).id));
            }

            string updatedMistakesJson = JsonConvert.SerializeObject(list, Formatting.Indented); //сериализация файла с ошибками
            File.WriteAllText(file, updatedMistakesJson);

            // Обновление количества правильных ответов
            double[] statisticsArray = JsonControl.StatisticsArray;

            if (correctAnswers == 10)
                statisticsArray[4]++;
            statisticsArray[3] += correctAnswers;
            statisticsArray[5]++;

            file = Path.Combine(projectDir, "statistics", "", "statistics.json");
            string updatedStatisticsJson = JsonConvert.SerializeObject(statisticsArray, Formatting.Indented);
            File.WriteAllText(file, updatedStatisticsJson);

            return (result, correctAnswers);
        }
    }
}
