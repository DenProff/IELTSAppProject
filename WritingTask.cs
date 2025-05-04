using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTSAppProject
{
    public class WritingTask : Task<string>
    {
        public WritingTask(string taskText, string answer) : base(taskText) // Запись поля
        {
            Answer = answer;
        }
    }
}
