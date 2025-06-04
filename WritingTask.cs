using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTSAppProject
{
    public class WritingTask : GeneralizedTask, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Answer { get; set; }
        public string UserAnswer { get; set; }

        public WritingTask(string taskText, string answer, double recTime) : base(taskText, recTime) // Запись поля
        {
            Answer = answer;
        }

    }
}
