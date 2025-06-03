using System;
using System.Collections.Generic;
using System;
using System.IO;
using System.Collections.Generic;
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
        public CollectionPage(TaskCollection taskCollection)
        {
            InitializeComponent();

            Task[] taskList = JsonControl.TaskList; // Десериализация в список json-файла со всеми заданиями

            foreach (int taskId in taskCollection) // Перебор id, хранящихся в поле-списке TaskCollection
            {
                // Подтягивание заданий из json по id при помощи бинарного поиска - не реализовано
                int index = SearchForIndexById(ref taskList, taskId); // Поиск индекса задания с нужным id

            }
        }

        private void Check_Click(object sender, RoutedEventArgs e) // Обработчик события кнопки "Проверить всё"
        {

        }

        private void Convert_Click(object sender, RoutedEventArgs e) // Обработчик события кнопки "Конвертировать всё в docx"
        {

        }

        private int SearchForIndexById(ref Task[] list, int id) // Бинарный поиск по id; возвращается индекс элемента с искомым id в массиве
        {
            int left = 0;
            int right = list.Length;
            int mid;
            while (left < right)
            {
                mid = left + (right - left) / 2;
                if (list[mid].Id >= id)
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
    }
}
