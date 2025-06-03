using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
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
using DocumentFormat.OpenXml.Drawing.Charts;

namespace IELTSAppProject
{
    /// <summary>
    /// Логика взаимодействия для CollectionPage.xaml
    /// </summary>
    public partial class CollectionPage : Page
    {
        public event EventHandler TaskCollectionDone; // Событие, генерируемое, когда пользователь хочет проверить все ответы
        public CollectionPage(TaskCollection taskCollection)
        {
            InitializeComponent();

            Task[] taskArray = JsonControl.TaskArray; // Десериализация в список json-файла со всеми заданиями

            foreach (int taskId in taskCollection) // Перебор id, хранящихся в поле-списке TaskCollection (id заданий, которые надо подгрузить)
            {
                // Подтягивание заданий из json по id при помощи бинарного поиска - не реализовано

                int index = SearchForIndexById(ref taskArray, taskId); // Поиск индекса задания с нужным id

                UserControl userControl = FindUserControlType(taskArray[index]); // Создание UserControl-a на основе задания с найденным id
                TasksContainer.Items.Add(userControl); // Добавление UserControl-а в выделенное в xaml-е пространство
            }
        }

        private void Check_Click(object sender, RoutedEventArgs e) // Обработчик события кнопки "Проверить всё"
        {
            TaskCollectionDone?.Invoke(this, EventArgs.Empty); // Вызов методов проверки, которые были подписаны на событие TaskCollectionDone в
                                                               // конструкторе выше
        }

        private void Convert_Click(object sender, RoutedEventArgs e) // Обработчик события кнопки "Конвертировать всё в docx"
        {

        }

        private int SearchForIndexById(ref Task[] array, int id) // Бинарный поиск по id; возвращается индекс элемента с искомым id в массиве
        {
            int left = 0;
            int right = array.Length;
            int mid;
            while (left < right)
            {
                mid = left + (right - left) / 2;
                if (array[mid].Id >= id)
                {
                    right = mid;
                }
                else
                {
                    left = mid + 1;
                }
            }
            if (left == id)
                return left;

            else throw new Exception("Задания с нужным id нет. Возможно была нарушена упорядоченность id по возрастанию в файле json.");
        }

        public UserControl FindUserControlType(Task task) // Определяет тип на основе которого нужно создать UserControl и возвращает создаваемый UserControl
        {
            UserControl newUserControlObject = new UserControl();
            if (task is SpeakingTask)
            {
                newUserControlObject = new SpeakingUserControl(task);
            }
            else if (task is ListeningTask)
            {
                newUserControlObject = new ListeningControl(task);
            }
            else if (task is ReadingTask)
            {
                newUserControlObject = new ReadingControl(task);
            }
            else if (task is WritingTask)
            {
                newUserControlObject = new WritingUserControl(task);
            }
            return newUserControlObject;
        }
    }
}
