using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTSAppProject
{
    public class ReadingTask : Task<List<string>>
    {
        public ReadingTask(string taskText, List<string> answer, double recTime) : base(taskText, answer, recTime)
        {
        }
    }
}
