using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTSAppProject
{
    public class WritingTask : Task<string>
    {
        public WritingTask(string taskText, string answer, double recTime) : base(taskText, answer, recTime) // Запись поля
        {
            Answer = answer;
        }
    }
}
