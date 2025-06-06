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

            this.KeyDown += (sender, e) =>
            {
                if (e.Key == Key.F1)
                {
                    OpenChmHelp();
                    e.Handled = true;
                }
            };

            ////запись в json
            List<string> first = new List<string>();
            List<string> second = new List<string>();
            List<string> third = new List<string>();
            List<string> fourth = new List<string>();
            List<string> fifth = new List<string>();
            List<string> answer = new List<string>();

            AddToList(ref first, "1", "2", "3");
            AddToList(ref second, "1", "2", "3");
            AddToList(ref third, "1", "2", "3");
            AddToList(ref fourth, "а", "б", "в");
            AddToList(ref fifth, "a", "b", "c");
            AddToList(ref answer, "True", "False", "Not Stated", "True", "False", "2",
                "3", "1", "а", "c");

            ReadingTask task = new ReadingTask("текст",10, answer, "текст", "задание", "задание", "задание", "задание", "задание", "задание", "задание",
                "задание", "задание", "задание", first, second, third, fourth, fifth);
            data = task;

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
            //checkAnswer.Click += (sender, e) => ValidateAnswers(data);
            convertToDocx.Click += (sender, e) => Сonversion.ConvertReading(data);
            
        }
        

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

        

           

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        //добавление в списки
        public static void AddToList(ref List<string> list, params string[] answers)
        {
            for (int i = 0; i < answers.Length; i++)
                list.Add(answers[i]);
        }


        //проявление результатов ответа по клику
        private void checkAnswer_Click(object sender, RoutedEventArgs e)
        {
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
        }

        //открытие справки
        private void help_Click(object sender, RoutedEventArgs e)
        {
            OpenChmHelp();
        }

        //проверка ответов
        public bool Check() // Данный метод должен реализовывать проверку ответов пользователя
        {
            ReadingTask task = (ReadingTask)this.DataContext;
            bool result = false;
            if (task.Answer[0] == answer1.Text)
                rightAnswer1.Text = "Правильный ответ!";
            else
            {
                rightAnswer1.Text = "Неправильный ответ!";
                result = true;
            }


            if (task.Answer[1] == answer2.Text)
                rightAnswer2.Text = "Правильный ответ!";
            else
            {
                rightAnswer2.Text = "Неправильный ответ!";
                result = true;
            }


            if (task.Answer[2] == answer3.Text)
                rightAnswer3.Text = "Правильный ответ!";
            else
            {
                rightAnswer3.Text = "Неправильный ответ!";
                result = true;
            }


            if (task.Answer[3] == answer4.Text)
                rightAnswer4.Text = "Правильный ответ!";
            else
            {
                rightAnswer4.Text = "Неправильный ответ!";
                result = true;
            }


            if (task.Answer[4] == answer5.Text)
                rightAnswer5.Text = "Правильный ответ!";
            else
            {
                rightAnswer5.Text = "Неправильный ответ!";
                result = true;
            }


            if (IsAnswerCorrect(task.Answer[5], answer61, answer62, answer63))
                rightAnswer6.Text = "Правильный ответ!";
            else
            {
                result = true;
                rightAnswer6.Text = "Неправильный ответ!";
            }


            if (IsAnswerCorrect(task.Answer[6], answer71, answer72, answer73))
                rightAnswer7.Text = "Правильный ответ!";
            else
            {
                rightAnswer7.Text = "Неправильный ответ!";
                result = true;
            }

            if (IsAnswerCorrect(task.Answer[7], answer81, answer82, answer83))
                rightAnswer8.Text = "Правильный ответ!";
            else
            {
                rightAnswer8.Text = "Неправильный ответ!";
                result = true;
            }

            if (IsAnswerCorrect(task.Answer[8], answer91, answer92, answer93))
                rightAnswer9.Text = "Правильный ответ!";
            else
            {
                rightAnswer9.Text = "Неправильный ответ!";
                result = true;
            }

            if (IsAnswerCorrect(task.Answer[6], answer71, answer72, answer73))
                rightAnswer0.Text = "Правильный ответ!";
            else
            {
                rightAnswer0.Text = "Неправильный ответ!";
                result = true;
            }

            return result;
        }
    }
}
