using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace IELTSAppProject
{
    public abstract class JsonControl
    {
        public static GeneralizedTask[] TaskArray // Свойство, возвращающее массив с заданиями
        {
            get => (GeneralizedTask[])GetArray("resourcesTask", "tasks", "tasks.json");
            private set { }
        }

        public static TaskCollection[] CollectionArray // Свойство, возвращающее массив с подборками заданий
        {
            get => (TaskCollection[]) GetArray("resourcesTask", "Collections", "taskCollections.json");
            private set { }
        }

        public static Tuple<int, string, string>[] MistakesArray // Свойство, возвращающее массив с заданиями, в которых была допущена ошибка
        {
            get => (Tuple<int, string, string>[])GetArray("resourcesTask", "Collections", "tasksWithMistakes.json");
            private set { }
        }

        public static TaskCollection[] UserCollectionsArray // Свойство, возвращающее массив с подборками для пользователя
        {
            get => (TaskCollection[])GetArray("resourcesTask", "Collections", "userCollections.json");
            private set { }
        }

        public static double[] StatisticsArray // Свойство, возвращающее массив со статистикой
        {
            get => (double[])GetArray("statistics", "", "statistics.json");
            private set { }
        }

        public static object GetArray(string firstDirectory, string secondDirectory, string currentJson) // Функция десериализации json
        {
            string projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            string file = Path.Combine(projectDir, firstDirectory, secondDirectory, currentJson);
            if (!File.Exists(file))
                throw new Exception("Файл с json не найден!");

            // Чтение JSON из файла
            string jsonFromFile = File.ReadAllText(file);

            // Десериализация
            if (currentJson == "taskCollections.json" || currentJson == "userCollections.json")
            {
                return (object)JsonConvert.DeserializeObject<TaskCollection[]>(jsonFromFile);
            }
            if (currentJson == "tasks.json")
            {
                return (object)JsonConvert.DeserializeObject<GeneralizedTask[]>(jsonFromFile);
            }
            if (currentJson == "tasksWithMistakes.json")
            {
                return (object)JsonConvert.DeserializeObject<Tuple<int, string, string>[]>(jsonFromFile);
            }
            else // (currentJson == "statistics.json")
            {
                return (object)JsonConvert.DeserializeObject<double[]>(jsonFromFile);
            }
        }
    }
}
