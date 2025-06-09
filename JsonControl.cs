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
            get
            {
                string projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                string file = Path.Combine(projectDir, "resourcesTask", "tasks", "tasks.json");
                if (!File.Exists(file))
                    throw new Exception("Файл с json не найден!");

                // Чтение JSON из файла
                string jsonFromFile = File.ReadAllText(file);

                // Десериализация
                return JsonConvert.DeserializeObject<GeneralizedTask[]>(jsonFromFile);
            }
            private set { }
        }

        public static TaskCollection[] CollectionArray // Свойство, возвращающее массив с заданиями
        {
            get
            {
                string projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                string file = Path.Combine(projectDir, "resourcesTask", "Collections", "taskCollections.json");
                if (!File.Exists(file))
                    throw new Exception("Файл с json не найден!");

                // Чтение JSON из файла
                string jsonFromFile = File.ReadAllText(file);

                // Десериализация
                return JsonConvert.DeserializeObject<TaskCollection[]>(jsonFromFile);
            }
            private set { }
        }

        public void AddTask() 
        {
            // Функция для добавления новых заданий
        }
    }
}
