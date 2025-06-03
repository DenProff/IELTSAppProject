using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IELTSAppProject
{
    public class SpeakingTask : GeneralizedTask
    {
        public byte[] Answer { get; set; }
        public byte[] UserAnswer { get; set; }

        public SpeakingTask(string taskText, byte[] ans, double recTime) : base(taskText, recTime) // Запись поля
        {
            Answer = ans;
        }
    }
}
