using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

namespace IELTSAppProject
{
    public class SpeakingTask : GeneralizedTask, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public byte[] Answer { get; set; }
        public byte[] UserAnswer { get; set; }

        public SpeakingTask(string taskText, byte[] ans, double recTime, string taskType) : base(taskText, recTime, taskType) // Запись поля
        {
            Answer = ans;
        }
    }
}
