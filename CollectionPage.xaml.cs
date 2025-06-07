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
using System.Windows.Threading;

namespace IELTSAppProject
{
    /// <summary>
    /// Логика взаимодействия для CollectionPage.xaml
    /// </summary>
    public delegate bool TaskCollectionDoneDelegate();
    public partial class CollectionPage : Page
    {
        public event TaskCollectionDoneDelegate TaskCollectionDone; // Событие, генерируемое, когда пользователь хочет проверить все ответы
        public CollectionPage(TaskCollection taskCollection)
        {
            InitializeComponent();

            this.Opacity = 0; // Делаем странцу прозрачной

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

            GeneralizedTask[] taskArray = JsonControl.TaskArray; // Десериализация в список json-файла со всеми заданиями

            foreach (int taskId in taskCollection) // Перебор id, хранящихся в поле-списке TaskCollection (id заданий, которые надо подгрузить)
            {
                // Подтягивание заданий из json по id при помощи бинарного поиска - не реализовано

                int index = SearchForIndexById(ref taskArray, taskId); // Поиск индекса задания с нужным id

                ICheckable userControl = FindUserControlType(taskArray[index]); // Создание UserControl-a на основе задания с найденным id
                TasksContainer.Items.Add(userControl); // Добавление UserControl-а в выделенное в xaml-е пространство

                TaskCollectionDone += userControl.Check; // Подписка метода проверки добавляемого UserControl-а
            }

            // После полной загрузки элементов, включаем видимость страницы и прокручиваем вверх до ее начала
            Dispatcher.BeginInvoke(new Action(() =>
            {
                this.Opacity = 1;
                MainScrollViewer.ScrollToHome();
            }), DispatcherPriority.Loaded);

            Convert.Click += (sender, e) => ConvertTasks(taskCollection);
        }

        private void Check_Click(object sender, RoutedEventArgs e) // Обработчик события кнопки "Проверить всё" - при нажатии на кнопку сработают
                                                                   // все методы Check
        {
            TaskCollectionDone?.Invoke(); // Вызов методов проверки, которые были подписаны на событие TaskCollectionDone в
                                                               // конструкторе выше
        }

        public static int SearchForIndexById(ref GeneralizedTask[] array, int id) // Бинарный поиск по id; возвращается индекс элемента с искомым id в массиве
        {
            int left = 0;
            int right = array.Length;
            int mid;
            while (left < right)
            {
                mid = left + (right - left) / 2;
                if (array[mid].id >= id)
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

        public static ICheckable FindUserControlType(GeneralizedTask task) // Определяет тип на основе которого нужно создать UserControl и возвращает создаваемый UserControl
        {
            ICheckable newUserControlObject;
            if (task is SpeakingTask)
            {
                newUserControlObject = (new SpeakingUserControl((SpeakingTask)task));
            }
            else if (task is ListeningTask)
            {
                newUserControlObject = (new ListeningUserControl((ListeningTask)task));
            }
            else if (task is ReadingTask)
            {
                newUserControlObject = (new ReadingUserControl((ReadingTask)task));
            }
            else
            {
                newUserControlObject = (new WritingUserControl((WritingTask)task));
            }
            return newUserControlObject; // Возвращается именно ICheckable, чтобы в конструкторе было удобно подписывать метод Check
        }

        //открытие справки
        private void OpenChmHelp()
        {
            string chmPath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Help",
                "referenceData.chm"
            );

            if (File.Exists(chmPath))
            {
                try
                {
                    // Открыть страницу "reading.htm" внутри CHM
                    Process.Start("hh.exe", $"{chmPath}::/partsOfExam.htm");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //метод для получения списка заданий
        public List<UserControl> PrepareToConvert(TaskCollection task)
        {
            GeneralizedTask[] taskArray = JsonControl.TaskArray; // Десериализация в список json-файла со всеми заданиями

            List<UserControl> list = new List<UserControl>();

            foreach (int taskId in task) // Перебор id, хранящихся в поле-списке TaskCollection (id заданий, которые надо подгрузить)
            {
                // Подтягивание заданий из json по id при помощи бинарного поиска - не реализовано

                int index = SearchForIndexById(ref taskArray, taskId); // Поиск индекса задания с нужным id

                list.Add((UserControl)FindUserControlType(taskArray[index])); // Создание UserControl-a на основе задания с найденным id
            }

            return list;
        }

        //метод для привязки к кнопке конвертации
        public void ConvertTasks(TaskCollection task)
        {
            List<UserControl> list = PrepareToConvert(task);

            foreach (var item in list)
            {
                if (item.DataContext is ReadingTask)
                    Conversion.ConvertReading((ReadingTask)item.DataContext);
                else if (item.DataContext is ListeningTask)
                    Conversion.ConvertListening((ListeningTask)item.DataContext);
                else if (item.DataContext is WritingTask)
                    Conversion.ConvertWriting((WritingTask)item.DataContext);
                else if (item.DataContext is SpeakingTask)
                    Conversion.ConvertSpeaking((SpeakingTask)item.DataContext);
            }
            MessageBox.Show("Файл/ы с заданием скачан и находится на вашем рабочем столе");
        }
    }
}
