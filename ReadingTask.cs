using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.ComponentModel;

namespace IELTSAppProject
{
    public class ReadingTask : Task<List<string>>, INotifyPropertyChanged
    {
        

        public string TextForReading { get; set; } //текст для прочтения и решения заданий по нему

        //поля к заданиям
        public string Task1 { get; set; } 
        public string Task2 { get; set; }
        public string Task3 { get; set; }
        public string Task4 { get; set; }
        public string Task5 { get; set; }
        public string Task6 { get; set; }
        public string Task7 { get; set; }
        public string Task8 { get; set; }
        public string Task9 { get; set; }
        public string Task0 { get; set; }

        //списки для вариантов ответов (a,b or c)
        public List<string> TaskAnswerList6 { get; set; }
        public List<string> TaskAnswerList7 { get; set; }
        public List<string> TaskAnswerList8 { get; set; }
        public List<string> TaskAnswerList9 { get; set; }
        public List<string> TaskAnswerList0 { get; set; }

        //свойства вариантов ответов из списков
        public string TaskAnswer1
        {
            get => TaskAnswerList6[0];
        }

        public string TaskAnswer2
        {
            get => TaskAnswerList6[1];
        }

        public string TaskAnswer3
        {
            get => TaskAnswerList6[2];
        }

        public string TaskAnswer4
        {
            get => TaskAnswerList7[0];
        }

        public string TaskAnswer5
        {
            get => TaskAnswerList7[1];
        }

        public string TaskAnswer6
        {
            get => TaskAnswerList7[2];
        }

        public string TaskAnswer7
        {
            get => TaskAnswerList8[0];
        }

        public string TaskAnswer8
        {
            get => TaskAnswerList8[1];
        }

        public string TaskAnswer9
        {
            get => TaskAnswerList8[2];
        }

        public string TaskAnswer10
        {
            get => TaskAnswerList9[0];
        }

        public string TaskAnswer11
        {
            get => TaskAnswerList9[1];
        }

        public string TaskAnswer12
        {
            get => TaskAnswerList9[2];
        }

        public string TaskAnswer13
        {
            get => TaskAnswerList0[0];
        }

        public string TaskAnswer14
        {
            get => TaskAnswerList0[1];
        }

        public string TaskAnswer15
        {
            get => TaskAnswerList0[2];
        }

        //список правильных ответов

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public ReadingTask(string taskText, List<string> answer, double recTime, string textForReading, string task1, string task2, string task3,
            string task4, string task5, string task6, string task7, string task8, string task9, string task0, List<string> firstList, List<string> secondList,
            List<string> thirdList, List<string> fourthList, List<string> fifthList) : base(taskText, answer, recTime)
        {
            TextForReading = textForReading;
            Task1 = task1;
            Task2 = task2;
            Task3 = task3;
            Task4 = task4;
            Task5 = task5;
            Task6 = task6;
            Task7 = task7;
            Task8 = task8;
            Task9 = task9;
            Task0 = task0;
            TaskAnswerList6 = firstList;
            TaskAnswerList7 = secondList;
            TaskAnswerList8 = thirdList;
            TaskAnswerList9 = fourthList;
            TaskAnswerList0 = fifthList;
        }
    }
}
