using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTSAppProject
{
    public delegate void Checking(object source, EventArgs args);

    [JsonObject(MemberSerialization.OptIn)]
    public class TaskCollection : IEnumerable<int> // Класс подборки
    {
        [JsonProperty] public int VariantId { get; set; } // id данной подборки
        [JsonProperty] public string VariantName { get; set; } // Название данной подборки
        [JsonProperty] public string DateOfAccess {  get; set; } // Дата последнего обращения к данной подборке в формате: xx.xx.xxxx
        [JsonProperty] public List<int> TaskIdList { get; set; } // Список из id, которые нужно подгружать из json
        [JsonProperty] public bool isListening { get; set; } //есть ли в подборке Listening
        [JsonProperty] public bool isSpeaking { get; set; } //есть ли в подборке Speaking
        [JsonProperty] public bool isWriting { get; set; } //есть ли в подборке Writing
        [JsonProperty] public bool isReading { get; set; } //есть ли в подборке Reading
        [JsonProperty] public bool isFastRepeat { get; set; } //есть ли в подборке задания для быстрого повторения
        [JsonProperty] public bool isVariants { get; set; } //есть ли в подборке варианты экзамена

        public TaskCollection() { }

        public TaskCollection(int varId, string varName, string date, List<int> taskIdList, bool listening,
            bool speaking, bool writing, bool reading, bool fastRepeat, bool variant)
        {
            VariantId = varId;
            VariantName = varName;
            DateOfAccess = date;
            TaskIdList = taskIdList;
            isListening = listening;
            isSpeaking = speaking;
            isWriting = writing;
            isReading = reading;
            isFastRepeat = fastRepeat;
            isVariants = variant;
        }


        // yield return - важно для оптимизации, поэтому ниже реализована перечисляемость для коллекции

        public IEnumerable<int> ForEachMethod(List<int> listWithId)
        {
            foreach (int taskId in listWithId)
            {
                yield return taskId;
            }
        }

        public IEnumerator<int> GetEnumerator()
        {
            return ForEachMethod(TaskIdList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
