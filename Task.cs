namespace IELTSAppProject
{
    public abstract class Task<TAnswer> // TAnswer - строка, List или массив byte
    {
        private static int IdCounter = 0;

        public int id;
        public TAnswer Answer { get; set; }
        public TAnswer UserAnswer { get; set; }
        public string TaskText { get; set; }

        // Поля для фильтрации - надо бы попридумывать
        public bool WithMistake { get; set; } // Была ли в этом задании допущена и не исправлена ошибка

        public Task(string taskText)
        {
            id = IdCounter++;
            this.TaskText = taskText;
            WithMistake = false;
        }
    }
}
