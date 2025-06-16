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
            //data = task;

            //ReadingTask newTask = null;

            //try
            //{
            //    string projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            //    string file = Path.Combine(projectDir, "resourcesTask", "tasks", "tasks.json");
            //    if (!File.Exists(file))
            //    {
            //        MessageBox.Show("Файл не найден!");
            //        return;
            //    }
            if (data != null)
            {
                this.RecomTime = data.RecommendedTime.ToString() + " мин.";
                //    // Сериализуем объект в JSON
                //    string jsonObject = JsonConvert.SerializeObject(data);

                //    // Записываем JSON в файл (перезаписываем, если существует)
                //    File.WriteAllText(file, jsonObject);

                //    // Читаем JSON из файла (после закрытия потока)
                //    string jsonFromFile = File.ReadAllText(file);

                //    // Десериализуем обратно в объект
                //    newTask = JsonConvert.DeserializeObject<ReadingTask>(jsonFromFile);

                this.DataContext = data;
            }
            
            //}
            //catch (IOException ex)
            //{
            //    MessageBox.Show($"Ошибка доступа к файлу: {ex.Message}");
            //}
            //catch (JsonException ex)
            //{
            //    MessageBox.Show($"Ошибка формата JSON: {ex.Message}");
            //}

            //подписка на событие клика
            convertToDocx.Click += (sender, e) => Conversion.ConvertReading(data);
            convertToDocx.Click += (sender, e) => MessageBox.Show("Файл/ы с заданием скачан и находится на вашем рабочем столе");
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
            string projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string file = Path.Combine(projectDir, "resourcesTask", "Collections", "userCollections.json");
            string jsonData = File.ReadAllText(file);

            ReadingTask data = (ReadingTask)this.DataContext;

            List<int> idList = new List<int>() { data.id };

            //создание экземпляра TaskCollection
            TaskCollection userTask = new TaskCollection(data.id, name, today,
                         idList, false, false, false, true, false, false);

            List<TaskCollection> list = JsonConvert.DeserializeObject<List<TaskCollection>>(jsonData) ?? new List<TaskCollection>();

            list.Add(userTask);

            string updatedJson = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText(file, updatedJson);

            MessageBox.Show("Данная подбока добавлена в раздел \"Мои подборки заданий\"");
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
        public bool Check()
        {
            ReadingTask task = (ReadingTask)this.DataContext;
            bool result = false;
            int correctAnswers = 0;
            string file;
            string projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            if (task.Answer[0] == answer1.Text)
            {
                rightAnswer1.Text = "Правильный ответ!";
                correctAnswers++;
            }
            else
            {
                rightAnswer1.Text = "Неправильный ответ!";
                result = true;
            }


            if (task.Answer[1] == answer2.Text)
            {
                rightAnswer2.Text = "Правильный ответ!";
                correctAnswers++;
            }
            else
            {
                rightAnswer2.Text = "Неправильный ответ!";
                result = true;
            }


            if (task.Answer[2] == answer3.Text)
            {
                rightAnswer3.Text = "Правильный ответ!";
                correctAnswers++;
            }
            else
            {
                rightAnswer3.Text = "Неправильный ответ!";
                result = true;
            }


            if (task.Answer[3] == answer4.Text)
            {
                rightAnswer4.Text = "Правильный ответ!";
                correctAnswers++;
            }
            else
            {
                rightAnswer4.Text = "Неправильный ответ!";
                result = true;
            }


            if (task.Answer[4] == answer5.Text)
            {
                rightAnswer5.Text = "Правильный ответ!";
                correctAnswers++;
            }
            else
            {
                rightAnswer5.Text = "Неправильный ответ!";
                result = true;
            }


            if (IsAnswerCorrect(task.Answer[5], answer61, answer62, answer63))
            {
                rightAnswer6.Text = "Правильный ответ!";
                correctAnswers++;
            }
            else
            {
                result = true;
                rightAnswer6.Text = "Неправильный ответ!";
            }


            if (IsAnswerCorrect(task.Answer[6], answer71, answer72, answer73))
            {
                rightAnswer7.Text = "Правильный ответ!";
                correctAnswers++;
            }
            else
            {
                rightAnswer7.Text = "Неправильный ответ!";
                result = true;
            }

            if (IsAnswerCorrect(task.Answer[7], answer81, answer82, answer83))
            {
                rightAnswer8.Text = "Правильный ответ!";
                correctAnswers++;
            }
            else
            {
                rightAnswer8.Text = "Неправильный ответ!";
                result = true;
            }

            if (IsAnswerCorrect(task.Answer[8], answer91, answer92, answer93))
            {
                rightAnswer9.Text = "Правильный ответ!";
                correctAnswers++;
            }
            else
            {
                rightAnswer9.Text = "Неправильный ответ!";
                result = true;
            }

            if (IsAnswerCorrect(task.Answer[9], answer01, answer02, answer03))
            {
                rightAnswer0.Text = "Правильный ответ!";
                correctAnswers++;
            }
            else
            {
                rightAnswer0.Text = "Неправильный ответ!";
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

            if (result)
            {
                projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                file = Path.Combine(projectDir, "resourcesTask", "Collections", "tasksWithMistakes.json");
                string jsonData = File.ReadAllText(file);

                DateTime now = DateTime.Today; //берем сегодняшнюю дату

                string today = now.ToString("dd.MM.yyyy"); //преводим ее в строку

                List<Tuple<int, string, string>> list = JsonConvert.DeserializeObject<List<Tuple<int, string, string>>>(jsonData) ?? new List<Tuple<int, string, string>>();

                Tuple<int, string, string> tuple = new Tuple<int, string, string>(((ReadingTask)this.DataContext).id, ((ReadingTask)this.DataContext).TaskType, today);

                list.Add(tuple);

                string updatedMistakesJson = JsonConvert.SerializeObject(list, Formatting.Indented);
                File.WriteAllText(file, updatedMistakesJson);
            }

            // Обновление количества правильных ответов
            double[] statisticsArray = JsonControl.StatisticsArray;

            if (correctAnswers == 10)
                statisticsArray[4]++;
            statisticsArray[3] += correctAnswers;
            statisticsArray[5]++;

            file = Path.Combine(projectDir, "statistics", "", "statistics.json");
            string updatedStatisticsJson = JsonConvert.SerializeObject(statisticsArray, Formatting.Indented);
            File.WriteAllText(file, updatedStatisticsJson);

            return result;
        }
        public static string[] resourcesKeysArray =
{
        "describeTextBlockFirstTestPage",
        "solveEnterVariantBTN",
        "notSolveEnterVariantBTN",
        "help",
        "prevPage",
        "convertBTN",
        "addToCollection",
        "recomendedTime"
        }; // Массив с ключами для ресурсов - необходимо для реализации многоязычности
    }
}
