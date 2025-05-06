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

            AddToList(ref first, "", "", "");
            AddToList(ref second, "", "", "");
            AddToList(ref third, "", "", "");
            AddToList(ref fourth, "", "", "");
            AddToList(ref fifth, "", "", "");

            //ReadingTask task = new ReadingTask();

            

            //string jsonObject = JsonConvert.SerializeObject(task);

            //File.WriteAllText("tasks.json", string.Empty);

            //File.AppendAllText("tasks.json", jsonObject);

            //поиск нужного элемента из json

            JsonTextReader reader = new JsonTextReader(new StreamReader("tasks.json"));
            reader.SupportMultipleContent = true;
            JsonSerializer serializer = new JsonSerializer();
            ReadingTask newTask = serializer.Deserialize<ReadingTask>(reader);

            //вызовы методов для заполнения полей
            NumAndTask(newTask);
            Task1(newTask);
            Task2(newTask);
            Task3(newTask);
            Task4(newTask);
            Task5(newTask);
            Task6(newTask);
            Task7(newTask);
            Task8(newTask);
            Task9(newTask);
            Task0(newTask);
            TaskAnswer1(newTask);
            TaskAnswer2(newTask);
            TaskAnswer3(newTask);
            TaskAnswer4(newTask);
            TaskAnswer5(newTask);
            TaskAnswer6(newTask);
            TaskAnswer7(newTask);
            TaskAnswer8(newTask);
            TaskAnswer9(newTask);
            TaskAnswer10(newTask);
            TaskAnswer11(newTask);
            TaskAnswer12(newTask);
            TaskAnswer13(newTask);
            TaskAnswer14(newTask);
            TaskAnswer15(newTask);
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

        //методы для получения строк заданий
        public static string NumAndTask(ReadingTask elem)
        {
            return elem.id.ToString() + elem.TaskText;
        }

        public static string TaskText(ReadingTask elem)
        {
            return elem.TextForReading;
        }

        public static string Task1(ReadingTask elem)
        {
            return elem.Task1;
        }

        public static string Task2(ReadingTask elem)
        {
            return elem.Task2;
        }

        public static string Task3(ReadingTask elem)
        {
            return elem.Task3;
        }

        public static string Task4(ReadingTask elem)
        {
            return elem.Task4;
        }

        public static string Task5(ReadingTask elem)
        {
            return elem.Task5;
        }

        public static string Task6(ReadingTask elem)
        {
            return elem.Task6;
        }

        public static string Task7(ReadingTask elem)
        {
            return elem.Task7;
        }

        public static string Task8(ReadingTask elem)
        {
            return elem.Task8;
        }

        public static string Task9(ReadingTask elem)
        {
            return elem.Task9;
        }

        public static string Task0(ReadingTask elem)
        {
            return elem.Task0;
        }

        //методы для вывода вариантов ответов в секции выбора a,b or c
        public static string TaskAnswer1(ReadingTask elem)
        {
            return elem.TaskAnswer6[0];
        }

        public static string TaskAnswer2(ReadingTask elem)
        {
            return elem.TaskAnswer6[1];
        }

        public static string TaskAnswer3(ReadingTask elem)
        {
            return elem.TaskAnswer6[2];
        }

        public static string TaskAnswer4(ReadingTask elem)
        {
            return elem.TaskAnswer7[0];
        }

        public static string TaskAnswer5(ReadingTask elem)
        {
            return elem.TaskAnswer7[1];
        }

        public static string TaskAnswer6(ReadingTask elem)
        {
            return elem.TaskAnswer7[2];
        }

        public static string TaskAnswer7(ReadingTask elem)
        {
            return elem.TaskAnswer8[0];
        }

        public static string TaskAnswer8(ReadingTask elem)
        {
            return elem.TaskAnswer8[1];
        }

        public static string TaskAnswer9(ReadingTask elem)
        {
            return elem.TaskAnswer8[2];
        }

        public static string TaskAnswer10(ReadingTask elem)
        {
            return elem.TaskAnswer9[0];
        }

        public static string TaskAnswer11(ReadingTask elem)
        {
            return elem.TaskAnswer9[1];
        }

        public static string TaskAnswer12(ReadingTask elem)
        {
            return elem.TaskAnswer9[2];
        }

        public static string TaskAnswer13(ReadingTask elem)
        {
            return elem.TaskAnswer0[0];
        }

        public static string TaskAnswer14(ReadingTask elem)
        {
            return elem.TaskAnswer0[1];
        }

        public static string TaskAnswer15(ReadingTask elem)
        {
            return elem.TaskAnswer0[2];
        }

    }
}
