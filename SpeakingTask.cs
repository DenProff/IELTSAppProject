using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IELTSAppProject
{
    public class SpeakingTask : Task<byte[]>
    {
        public SpeakingTask(string taskText, byte[] ans, double recTime) : base(taskText, ans, recTime) // Запись поля
        {
            Answer = ans;
        }
    }
}
