using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;
using System;

namespace IELTSAppProject
{
    public abstract class GeneralizedTask : IComparable<GeneralizedTask>
    {
        private static int IdCounter = 0; // ЕГО НАДО ИЗ JSON !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        public int id;
        public string TaskText { get; set; }
        public double RecommendedTime{ get; set; }

        // Поля для фильтрации - надо бы попридумывать
        public bool WithMistake { get; set; } // Была ли в этом задании допущена и не исправлена ошибка

        public GeneralizedTask(string taskText, double recTime)
        {
            RecommendedTime = recTime;
            id = IdCounter++;
            this.TaskText = taskText;
            WithMistake = false;
        }

        //переопределение метода для бинарного поиска
        public int CompareTo(GeneralizedTask other)
        {
            if (this.id < other.id)
                return -1;
            if (this.id > other.id)
                return 1;
            else 
                return 0;
        }
    }
}
