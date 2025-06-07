using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTSAppProject
{
    public delegate void Checking(object source, EventArgs args);
    public class TaskCollection : IEnumerable<int> // Класс подборки
    {
        public int VariantId { get; set; } // id данной подборки
        public string VariantName { get; set; } // Название данной подборки
        public string DateOfAccess {  get; set; } // Дата последнего обращения к данной подборке в формате: xx.xx.xxxx
        public List<int> TaskIdList { get; set; } // Список из id, которые нужно подгружать из json
        public bool isListening { get; set; } //есть ли в подборке Listening
        public bool isSpeaking { get; set; } //есть ли в подборке Speaking
        public bool isWriting { get; set; } //есть ли в подборке Writing
        public bool isReading { get; set; } //есть ли в подборке Reading
        public bool isFastRepeat { get; set; } //есть ли в подборке задания для быстрого повторения
        public bool isVariants { get; set; } //есть ли в подборке варианты экзамена

        public event Checking Solved; // Событие, генерируемое при нажатии пользователем кнопки "проверить всё"

        public TaskCollection(int varId, string varName, string date, List<int> taskIdList)
        {
            VariantId = varId;
            VariantName = varName;
            DateOfAccess = date;
            TaskIdList = taskIdList;
        }


        // yield return - важно для оптимизации, поэтому ниже реализована перечисляемость для коллекции

        public IEnumerable<int> ForEachMethod(List<int> listWithId)
        {
            foreach (int taskId in listWithId)
            {
                yield return taskId;
            }
        }

        public IEnumerator<int> GetEnumerator()
        {
            return ForEachMethod(TaskIdList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
