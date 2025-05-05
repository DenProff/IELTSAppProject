using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTSAppProject
{
    public class ListeningTask : Task<List<string>>
    {
        public string AudioPath { get; set; }

        public List<string> Options { get; set; }

        public ListeningTask(string taskText, List<string> answer, double recTime) : base(taskText, answer, recTime)
        {
        }
    }
}
