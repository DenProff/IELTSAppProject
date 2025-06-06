using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;
using Newtonsoft.Json;
using System;
using JsonSubTypes;

namespace IELTSAppProject
{
    [JsonConverter(typeof(JsonSubtypes), "TaskType")] // Указываем поле-дискриминатор
    [JsonSubtypes.KnownSubType(typeof(ListeningTask), "ListeningTask")]
    [JsonSubtypes.KnownSubType(typeof(ReadingTask), "ReadingTask")]
    public class GeneralizedTask
    {
        private static int IdCounter = 0; // ЕГО НАДО ИЗ JSON !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        public int id;
        public string TaskText { get; set; }
        public double RecommendedTime{ get; set; }

        public string TaskType { get; set; }

        // Поля для фильтрации - надо бы попридумывать
        public bool WithMistake { get; set; } // Была ли в этом задании допущена и не исправлена ошибка

        public GeneralizedTask(string taskText, double recTime, string taskType)
        {
            TaskType = taskType;
            RecommendedTime = recTime;
            id = IdCounter++;
            this.TaskText = taskText;
            WithMistake = false;
        }

        public GeneralizedTask()
        {
        }
    }
}
