using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace IELTSAppProject
{
    public class ReadingTask : Task<List<string>>
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

        //поля для ответов на вопросы
        public string TaskAnswer1 { get; set; }
        public string TaskAnswer2 { get; set; }
        public string TaskAnswer3 { get; set; }
        public string TaskAnswer4 { get; set; }
        public string TaskAnswer5 { get; set; }

        public string TaskAnswer6 { get; set; }
        public string TaskAnswer7 { get; set; }
        public string TaskAnswer8 { get; set; }
        public string TaskAnswer9 { get; set; }
        public string TaskAnswer0 { get; set; }




        public ReadingTask(string taskText, List<string> answer, double recTime, string textForReading, string task1, string task2, string task3,
            string task4, string task5, string task6, string task7, string task8, string task9, string task0) : base(taskText, answer, recTime)
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
        }
    }
}
