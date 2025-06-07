using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;
using Newtonsoft.Json;
using System;
using JsonSubTypes;

namespace IELTSAppProject
{
    [JsonConverter(typeof(JsonSubtypes), "TaskType")] // Указывает поле-дискриминатор
    [JsonSubtypes.KnownSubType(typeof(ListeningTask), "ListeningTask")]
    [JsonSubtypes.KnownSubType(typeof(ReadingTask), "ReadingTask")]
    [JsonSubtypes.KnownSubType(typeof(SpeakingTask), "SpeakingTask")]
    [JsonSubtypes.KnownSubType(typeof(WritingTask), "WritingTask")]
    public class GeneralizedTask
    {
        public int id;
        public string TaskText { get; set; } // Текст задания
        public double RecommendedTime{ get; set; } // Рекомендуемое время для выполнения задания
        public string TaskType { get; set; } // Для работы json
        public bool WithMistake { get; set; } // Была ли в этом задании допущена и не исправлена ошибка

        public GeneralizedTask(int id, string taskText, double recTime, string taskType)
        {
            TaskType = taskType;
            RecommendedTime = recTime;
            this.id = id;
            this.TaskText = taskText;
            WithMistake = false;
        }
    }
}
