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

        public string AudioPathIdealAnswer { get; set; } // Путь к аудиофайлу с идеальным ответом
        public string AudioPathUserAnswer { get; set; } // Путь к аудиофайлу с ответом пользователя

        public SpeakingTask(string taskText, double recTime, string taskType, string idealAns, string userAns) : base(taskText, recTime, taskType)
        {
            AudioPathIdealAnswer = idealAns;
            AudioPathUserAnswer = userAns;
        }
    }
}
