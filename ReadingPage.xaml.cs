using System;
using System.Collections.Generic;
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
using System.IO;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;

namespace IELTSAppProject
{
    /// <summary>
    /// Логика взаимодействия для ReadingPage.xaml
    /// </summary>
    public partial class ReadingPage : Page
    {
        public ReadingPage()
        {
            InitializeComponent();

            foreach (UIElement elem in readingPart.Children)
            {
                if (elem is Button)
                {
                    if (elem == addToCollection)
                        ((Button)elem).Click += Button_Click;
                    if (elem == convertToDocx)
                        ((Button)elem).Click += convertToDocx_Click;
                }
            }

            //запись в json
            List<string> first = new List<string>();
            List<string> second = new List<string>();
            List<string> third = new List<string>();
            List<string> fourth = new List<string>();
            List<string> fifth = new List<string>();
            List<string> answer = new List<string>();

            AddToList(ref first, "вариант", "вариант", "вариант");
            AddToList(ref second, "вариант", "вариант", "вариант");
            AddToList(ref third, "вариант", "вариант", "вариант");
            AddToList(ref fourth, "вариант", "вариант", "вариант");
            AddToList(ref fifth, "вариант", "вариант", "вариант");
            AddToList(ref answer, "вариант", "вариант", "вариант");

            ReadingTask task = new ReadingTask("текст", answer, 10, "текст", "задание", "задание", "задание", "задание", "задание", "задание", "задание",
                "задание", "задание", "задание", first, second, third, fourth, fifth);


            try
            {
                string jsonObject = JsonConvert.SerializeObject(task);

                string file = "D:/Максим/ВЫШКА ПРОГА/Проект/Какое-то начало 2/точное начало/resourcesTask/VseNahui/tasks.json";
                File.WriteAllText("D:/Максим/ВЫШКА ПРОГА/Проект/Какое-то начало 2/точное начало/resourcesTask/VseNahui/tasks.json", string.Empty);

                File.AppendAllText("D:/Максим/ВЫШКА ПРОГА/Проект/Какое-то начало 2/точное начало/resourcesTask/VseNahui/tasks.json", jsonObject);

                //поиск нужного элемента из json

                JsonTextReader reader = new JsonTextReader(new StreamReader("D:/Максим/ВЫШКА ПРОГА/Проект/Какое-то начало 2/точное начало/resourcesTask/VseNahui/tasks.json"));
                reader.SupportMultipleContent = true;
                JsonSerializer serializer = new JsonSerializer();
                ReadingTask newTask = serializer.Deserialize<ReadingTask>(reader);
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
           
            


            foreach (UIElement elem in readingPart.Children)
            {
                if (elem is Button)
                {
                    if (elem == checkAnswer)
                    {
                        //RightAnswer1(newTask);
                        //RightAnswer2(newTask);
                        //RightAnswer3(newTask);
                        //RightAnswer4(newTask);
                        //RightAnswer5(newTask);
                        //RightAnswer6(newTask);
                        //RightAnswer7(newTask);
                        //RightAnswer8(newTask);
                        //RightAnswer9(newTask);
                        //RightAnswer0(newTask);
                        ((Button)elem).Click += checkAnswer_Click;
                    }
                }
            }
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void convertToDocx_Click(object sender, RoutedEventArgs e)
        {

        }

        public static void AddToList(ref List<string> list, string firstStroke, string secondStroke, string thirdStroke)
        {
            list.Add(firstStroke);
            list.Add(secondStroke);
            list.Add(thirdStroke);
        }

        
        //проявление резульаттов ответа по клику
        private void checkAnswer_Click(object sender, RoutedEventArgs e)
        {
            this.rightAnswer0.IsEnabled = true;
            this.rightAnswer9.IsEnabled = true;
            this.rightAnswer8.IsEnabled = true;
            this.rightAnswer7.IsEnabled = true;
            this.rightAnswer6.IsEnabled = true;
            this.rightAnswer5.IsEnabled = true;
            this.rightAnswer4.IsEnabled = true;
            this.rightAnswer3.IsEnabled = true;
            this.rightAnswer2.IsEnabled = true;
            this.rightAnswer1.IsEnabled = true;
        }

        private void help_Click(object sender, RoutedEventArgs e)
        {

        }

        ////методы появление надписей правильно или нет
        //public static string RightAnswer1(ReadingTask elem)
        //{
        //    return "Правильный ответ!";

        //    return "Неправильный ответ!";
        //}

        //public static string RightAnswer2(ReadingTask elem)
        //{
        //     "Правильный ответ!";

        //    return "Неправильный ответ!";
        //}

        //public static string RightAnswer3(ReadingTask elem)
        //{
        //    return "Правильный ответ!";

        //    return "Неправильный ответ!";
        //}

        //public static string RightAnswer4(ReadingTask elem)
        //{
        //    return "Правильный ответ!";

        //    return "Неправильный ответ!";
        //}

        //public static string RightAnswer5(ReadingTask elem)
        //{
        //    return "Правильный ответ!";

        //    return "Неправильный ответ!";
        //}

        //public static string RightAnswer6(ReadingTask elem)
        //{
        //    return "Правильный ответ!";

        //    return "Неправильный ответ!";
        //}

        //public static string RightAnswer7(ReadingTask elem)
        //{
        //    return "Правильный ответ!";

        //    return "Неправильный ответ!";
        //}

        //public static string RightAnswer8(ReadingTask elem)
        //{
        //    return "Правильный ответ!";

        //    return "Неправильный ответ!";
        //}

        //public static string RightAnswer9(ReadingTask elem)
        //{
        //    return "Правильный ответ!";

        //    return "Неправильный ответ!";
        //}

        //public static string RightAnswer0(ReadingTask elem)
        //{
        //    return "Правильный ответ!";

        //    return "Неправильный ответ!";
        //}
    }
}
