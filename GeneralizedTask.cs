using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;
using System;

namespace IELTSAppProject
{
    public abstract class GeneralizedTask<TAnswer> : IComparable<GeneralizedTask<TAnswer>>// TAnswer - строка, List или массив byte
    {
        private static int IdCounter = 0; // ЕГО НАДО ИЗ JSON !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        public int id;
        public TAnswer Answer { get; set; }
        public TAnswer UserAnswer { get; set; }
        public string TaskText { get; set; }
        public double RecommendedTime{ get; set; }

        // Поля для фильтрации - надо бы попридумывать
        public bool WithMistake { get; set; } // Была ли в этом задании допущена и не исправлена ошибка

        public GeneralizedTask(string taskText, TAnswer ans, double recTime)
        {
            RecommendedTime = recTime;
            Answer = ans;
            id = IdCounter++;
            this.TaskText = taskText;
            WithMistake = false;
        }

        //переопределение метода для бинарного поиска
        public int CompareTo(GeneralizedTask<TAnswer> other)
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
