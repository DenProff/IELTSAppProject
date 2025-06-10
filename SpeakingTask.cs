using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

namespace IELTSAppProject
{
    public class SpeakingTask : GeneralizedTask
    {
        public SpeakingTask(int id, string taskText, double recTime, string taskType/*, string idealAns, string userAns*/) : base(id, taskText, recTime, taskType){}
    }
}
