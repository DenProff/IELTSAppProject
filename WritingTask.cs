using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTSAppProject
{
    public class WritingTask : GeneralizedTask
    {
        public string[] Answer { get; set; } // Хранит идеальные эссе для 6ти тем: 2 General, 2 Academic, 2 business - строго в этом порядке
        public string UserAnswer { get; set; } // Пользователь выбирает всего одну тему, поэтому хранить нужно только её

        public WritingTask(int id, string taskType, string taskText, double recTime, string[] idealEssays) : base(id, taskText, recTime, taskType)
        {
            Answer = idealEssays;
        }
    }
}
