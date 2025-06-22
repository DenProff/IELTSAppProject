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
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Wordprocessing;
using NAudio.Wave;

namespace IELTSAppProject
{
    /// <summary>
    /// Логика взаимодействия для CollectionPage.xaml
    /// </summary>
    public delegate (bool hasErrors, int correctAnswers) TaskCollectionDoneDelegate();
    public partial class CollectionPage : Page
    {
        public event TaskCollectionDoneDelegate TaskCollectionDone; // Событие, генерируемое, когда пользователь хочет проверить все ответы
        public CollectionPage(TaskCollection taskCollection)
        {
            InitializeComponent();

            if (taskCollection.isSpeaking && !SoundControl.IsRecordingDeviceAvailable)
            {
                MessageBox.Show(SetLanguageResources.GetString(Properties.Settings.Default.Language, "NoRecordingDeviceWarning"));
                Dispatcher.BeginInvoke(new Action(() => NavigationService?.GoBack()), DispatcherPriority.ContextIdle);
                return;
            }

            this.DataContext = taskCollection; //Ставит экземпляр TaskCollection в качестве контекстных данных

            TaskCollection[] collectionArray = JsonControl.UserCollectionsArray; //получаем массив с подборками пользователей

            if (collectionArray.Any(elem => elem.TaskIdList.SequenceEqual(taskCollection.TaskIdList))) //ищем, если нашлись в массиве текущие индексы заданий
                //то делаем кнопку для удаления подборки из раздела персональных подборок доступной
            {
                delete_btn.Visibility = Visibility.Visible;
                delete_btn.IsEnabled = true;
            }

            this.Opacity = 0; // Делает страницу прозрачной

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

            GeneralizedTask[] taskArray = JsonControl.TaskArray; // Десериализация в список json-файла со всеми заданиями

            VariantId.Text += " " + taskCollection.VariantId;
            VariantName.Text  += " " + taskCollection.VariantName;

            foreach (int taskId in taskCollection) // Перебор id, хранящихся в поле-списке TaskCollection (id заданий, которые надо подгрузить)
            {
                // Подтягивание заданий из json по id при помощи бинарного поиска - не реализовано

                int index = SearchForIndexById(ref taskArray, taskId); // Поиск индекса задания с нужным id

                ICheckable userControl = FindUserControlType(taskArray[index]); // Создание UserControl-a на основе задания с найденным id
                TasksContainer.Items.Add(userControl); // Добавление UserControl-а в выделенное в xaml-е пространство

                TaskCollectionDone += () =>            // Подписка метода проверки добавляемого UserControl-а
                {
                    if (userControl is ListeningUserControl || userControl is ReadingUserControl)
                        return userControl.Check();
                    else
                        return (userControl.Check().hasErrors, 0);
                };
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
            TaskCollection taskCollection = (TaskCollection)DataContext;
            int totalCorrect = 0;

            if (taskCollection.isVariants)
            {
                foreach (var item in TasksContainer.Items)
                {
                    if (item is ICheckable checkable)
                    {
                        var result = checkable.Check();

                        if (item is ListeningUserControl || item is ReadingUserControl)
                            totalCorrect += result.correctAnswers;
                    }
                }
                UpdateExamStatistics(totalCorrect);
            }
            else
            {
                TaskCollectionDone?.Invoke(); // Вызов методов проверки, которые были подписаны на событие TaskCollectionDone в
                                              // конструкторе выше
            }

            Check.IsEnabled = false;
        }

        // Обновление статистики по экзаменам
        private void UpdateExamStatistics(int totalCorrectAnswers)
        {
            string path = System.IO.Path.Combine(
                Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName,
                "statistics",
                "statistics.json");

            double[] stats = JsonControl.StatisticsArray;
            stats[6]++; // +1 к количеству экзаменов
            stats[7] += totalCorrectAnswers; // Добавляем сумму правильных ответов
            File.WriteAllText(path, JsonConvert.SerializeObject(stats, Newtonsoft.Json.Formatting.Indented));
        }

        public static int SearchForIndexById(ref GeneralizedTask[] array, int id) // Бинарный поиск по id; возвращается индекс элемента с искомым id в массиве
        {
            int left = 0;
            int right = array.Length - 1;
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
            if (array[left].id == id)
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

        private void turnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
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
            string file = System.IO.Path.Combine(projectDir, "resourcesTask", "Collections", "userCollections.json");
            string jsonData = File.ReadAllText(file);

            TaskCollection data = (TaskCollection)this.DataContext;

            //создание экземпляра TaskCollection
            TaskCollection userTask = new TaskCollection(data.VariantId, name, today,
                         data.TaskIdList, data.isListening, data.isSpeaking, data.isWriting,
                         data.isReading, data.isFastRepeat, data.isVariants);

            List<TaskCollection> list = JsonConvert.DeserializeObject<List<TaskCollection>>(jsonData) ?? new List<TaskCollection>();

            list.Add(userTask);

            string updatedJson = JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(file, updatedJson);

            AddMessage();
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

        //Удаление подборки из раздела персональных подборок пользователя
        private void delete_btn_Click(object sender, RoutedEventArgs e)
        {
            TaskCollection taskCollection = (TaskCollection)this.DataContext; //получаем текущий экземпляр подборки

            //Работа с десериализация Json
            string projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string file = System.IO.Path.Combine(projectDir, "resourcesTask", "Collections", "userCollections.json");
            string jsonData = File.ReadAllText(file);

            List<TaskCollection> list = JsonConvert.DeserializeObject<List<TaskCollection>>(jsonData) ?? new List<TaskCollection>();

            //Удаляем элемент, который содержит нужные индексы заданий
            list.Remove(list.FirstOrDefault(elem => elem.TaskIdList.SequenceEqual(taskCollection.TaskIdList))); 

            //Сериализация Json
            string updatedJson = JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(file, updatedJson);

            MessageBox.Show("Подборка удалена из раздела \"Мои подборки заданий\"\nПерезайдите " +
                "в этот раздел заново, чтобы увидеть изменения");

            delete_btn.IsEnabled = false; //делаем кнопку недоступной повторно
        }

        public static void AddMessage() //метод вывода сообщения о добавлении подборки в персональные
        {
            MessageBox.Show("Данная подбока добавлена в раздел \"Мои подборки заданий\"");
        }

        private void help_Click(object sender, RoutedEventArgs e)
        {
            OpenChmHelp();
        }
    }
}
