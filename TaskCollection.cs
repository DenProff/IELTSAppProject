using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTSAppProject
{
    public delegate void Checking(object source, EventArgs args);
    public class TaskCollection
    {
        int VariantId { get; set; } // id данной подборки
        string VariantName { get; set; } // Название данной подборки
        string DateOfAccess {  get; set; } // Дата последнего обращения к данной подборке в формате: xx.xx.xxxx
        List<int> TaskIdList { get; set; } // Список из id, которые нужно подгружать из json

        public event Checking Solved; // Событие, генерируемое при нажатии пользователем кнопки "проверить всё"
    }
}
