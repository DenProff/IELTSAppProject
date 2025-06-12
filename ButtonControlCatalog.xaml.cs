using DocumentFormat.OpenXml.VariantTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace IELTSAppProject
{
    /// <summary>
    /// Логика взаимодействия для ButtonControlCatalog.xaml
    /// </summary>
    /// 

    public class PassThroughConverter : IMultiValueConverter //конвертер для binding
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3 || values.Any(v => v == null))
                return string.Empty;

            return string.Format("Вариант: {0} Название: {1} Дата публикации: {2}",
                                values[0], values[1], values[2]); ;
        } 

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
    
    public partial class ButtonControlCatalog : UserControl
    {
        public event EventHandler<TaskCollection> NavigationRequested;
        public ButtonControlCatalog(TaskCollection task)
        {
            InitializeComponent();

            this.DataContext = task;
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Language)) // Дополнительная безопасность, чтобы если что не было исключений
            {
                SetLanguageResources.SetLanguageResourcesMethod(Properties.Settings.Default.Language, resourcesKeysArray, this);
            }

            // Подписка на смену языка - событие в классе LanguageChange
            LanguageChange.LanguageChanged += () => SetLanguageResources.SetLanguageResourcesMethod(Properties.Settings.Default.Language, resourcesKeysArray, this);
        }



        private void collection_Click(object sender, RoutedEventArgs e)
        {
            TaskCollection task = (TaskCollection)this.DataContext;

            NavigationRequested?.Invoke(this, task); //применение события
        }

        //конвертация
        private void convertCollection_Click(object sender, RoutedEventArgs e)
        {
            List<UserControl> list = PrepareToConvert();

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

        //метод для получения списка заданий
        public List<UserControl> PrepareToConvert()
        {
            GeneralizedTask[] taskArray = JsonControl.TaskArray; // Десериализация в список json-файла со всеми заданиями

            TaskCollection task = (TaskCollection)this.DataContext;

            List<UserControl> list = new List<UserControl>();

            foreach (int taskId in task) // Перебор id, хранящихся в поле-списке TaskCollection (id заданий, которые надо подгрузить)
            {
                // Подтягивание заданий из json по id при помощи бинарного поиска - не реализовано

                int index = CollectionPage.SearchForIndexById(ref taskArray, taskId); // Поиск индекса задания с нужным id

                list.Add((UserControl)CollectionPage.FindUserControlType(taskArray[index])); // Создание UserControl-a на основе задания с найденным id
            }

            return list;
        }
        public static string[] resourcesKeysArray =
{
        "convertBTN"
        }; // Массив с ключами для ресурсов - необходимо для реализации многоязычности
    }
}
