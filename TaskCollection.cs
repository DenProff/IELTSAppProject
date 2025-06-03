using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTSAppProject
{
    public delegate void Checking(object source, EventArgs args);
    public class TaskCollection : IEnumerable<Task>
    {
        public int VariantId { get; set; } // id данной подборки
        public string VariantName { get; set; } // Название данной подборки
        public string DateOfAccess {  get; set; } // Дата последнего обращения к данной подборке в формате: xx.xx.xxxx
        public List<int> TaskIdList { get; set; } // Список из id, которые нужно подгружать из json

        public event Checking Solved; // Событие, генерируемое при нажатии пользователем кнопки "проверить всё"


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
