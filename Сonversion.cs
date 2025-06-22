using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IELTSAppProject
{
    public abstract class Conversion
    {

        public static void ConvertReading(ReadingTask elem)
        {
            //Путь к файлу
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\ReadingTask{elem?.id}.docx";

            //Создание документа
            using (WordprocessingDocument doc = WordprocessingDocument.Create(filePath,
                WordprocessingDocumentType.Document))
            {
                //Основная часть документа
                MainDocumentPart mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());

                if (elem != null)
                {
                    //Тип задания
                    body.AppendChild(new Paragraph(new Run(new Text($"Тип задания: {elem.TaskType}"))));
                    //Заголовок
                    body.AppendChild(new Paragraph(
                        new Run(new RunProperties(new FontSize() { Val = "28"}, new Bold()), new Text($"Id задания - {elem.id}"),
                        new Run(new Break()), // Перенос
                        new Run(new RunProperties(new FontSize() { Val = "28" }, new Bold()), new Text($"{elem.TextForReading}")))));

                    body.AppendChild(new Paragraph(new Run(new Text($"Рекомендованное время {elem.RecommendedTime} мин"))));

                    //Текст для задания
                    body.AppendChild(new Paragraph(new Run(new Text(elem.TaskText))));

                    //Задания (True/False/Not Stated)
                    body.AppendChild(new Paragraph(new Run(new Text(elem.Task1)), new Run(new Break()), new Run(new Text("True      " +
                        "False      Not Stated"))));
                    body.AppendChild(new Paragraph(new Run(new Text(elem.Task2)), new Run(new Break()), new Run(new Text("True      " +
                        "False      Not Stated"))));
                    body.AppendChild(new Paragraph(new Run(new Text(elem.Task3)), new Run(new Break()), new Run(new Text("True      " +
                        "False      Not Stated"))));
                    body.AppendChild(new Paragraph(new Run(new Text(elem.Task4)), new Run(new Break()), new Run(new Text("True      " +
                        "False      Not Stated"))));
                    body.AppendChild(new Paragraph(new Run(new Text(elem.Task5)), new Run(new Break()), new Run(new Text("True      " +
                        "False      Not Stated"))));

                    //Задания (Выбор варианта ответа)
                    body.AppendChild(new Paragraph(new Run(new Text(elem.Task6)), new Run(new Break()), new Run(new Text(elem.TaskAnswerList6[0])),
                        new Run(new Break()), new Run(new Text(elem.TaskAnswerList6[1])), new Run(new Break()), new Run(new Text(elem.TaskAnswerList6[2]))));
                    body.AppendChild(new Paragraph(new Run(new Text(elem.Task7)), new Run(new Break()), new Run(new Text(elem.TaskAnswerList7[0])),
                        new Run(new Break()), new Run(new Text(elem.TaskAnswerList7[1])), new Run(new Break()), new Run(new Text(elem.TaskAnswerList7[2]))));
                    body.AppendChild(new Paragraph(new Run(new Text(elem.Task8)), new Run(new Break()), new Run(new Text(elem.TaskAnswerList8[0])),
                        new Run(new Break()), new Run(new Text(elem.TaskAnswerList8[1])), new Run(new Break()), new Run(new Text(elem.TaskAnswerList8[2]))));
                    body.AppendChild(new Paragraph(new Run(new Text(elem.Task9)), new Run(new Break()), new Run(new Text(elem.TaskAnswerList9[0])),
                        new Run(new Break()), new Run(new Text(elem.TaskAnswerList9[1])), new Run(new Break()), new Run(new Text(elem.TaskAnswerList9[2]))));
                    body.AppendChild(new Paragraph(new Run(new Text(elem.Task0)), new Run(new Break()), new Run(new Text(elem.TaskAnswerList0[0])),
                        new Run(new Break()), new Run(new Text(elem.TaskAnswerList0[1])), new Run(new Break()), new Run(new Text(elem.TaskAnswerList0[2]))));
                }
                
            }
            
        }

        public static void ConvertListening(ListeningTask elem)
        {
            //Путь к файлу
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\ListeningTask{elem.id}.docx";

            //Создание документа
            using (WordprocessingDocument doc = WordprocessingDocument.Create(filePath,
                WordprocessingDocumentType.Document))
            {
                //Основная часть документа
                MainDocumentPart mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());
                
                //Тип задания
                body.AppendChild(new Paragraph(new Run(new Text($"Тип задания: {elem.TaskType}"))));

                body.AppendChild(new Paragraph(new Run(new Text($"Рекомендованное время {elem.RecommendedTime} мин"))));

                if (elem != null)
                {
                    //Заголовок
                    body.AppendChild(new Paragraph(
                        new Run(new RunProperties(new FontSize() { Val = "32" }, new Bold()), new Text($"Id задания - {elem.id}"),
                        new Run(new Break()), // Перенос
                        new Run(new RunProperties(new FontSize() { Val = "32" }, new Bold()), new Text($"{elem.TaskText}")))));

                    //Задания (True/False/Not Stated)
                    body.AppendChild(new Paragraph(new Run(new Text(elem.Task1), new Run(new Break()), new Run(new Text(elem.TaskAnswerList1[0])),
                        new Run(new Break()), new Run(new Text(elem.TaskAnswerList1[1])), new Run(new Break()), new Run(new Text(elem.TaskAnswerList1[2])))));
                    body.AppendChild(new Paragraph(new Run(new Text(elem.Task2)), new Run(new Break()), new Run(new Text(elem.TaskAnswerList2[0])),
                        new Run(new Break()), new Run(new Text(elem.TaskAnswerList2[1])), new Run(new Break()), new Run(new Text(elem.TaskAnswerList2[2]))));
                    body.AppendChild(new Paragraph(new Run(new Text(elem.Task3)), new Run(new Break()), new Run(new Text(elem.TaskAnswerList3[0])),
                        new Run(new Break()), new Run(new Text(elem.TaskAnswerList3[1])), new Run(new Break()), new Run(new Text(elem.TaskAnswerList3[2]))));
                    body.AppendChild(new Paragraph(new Run(new Text(elem.Task4)), new Run(new Break()), new Run(new Text(elem.TaskAnswerList4[0])),
                        new Run(new Break()), new Run(new Text(elem.TaskAnswerList4[1])), new Run(new Break()), new Run(new Text(elem.TaskAnswerList4[2]))));
                    body.AppendChild(new Paragraph(new Run(new Text(elem.Task5)), new Run(new Break()), new Run(new Text(elem.TaskAnswerList5[0])),
                        new Run(new Break()), new Run(new Text(elem.TaskAnswerList5[1])), new Run(new Break()), new Run(new Text(elem.TaskAnswerList5[2]))));


                    //Задания (Выбор варианта ответа)
                    body.AppendChild(new Paragraph(new Run(new Text(elem.Task6)), new Run(new Break()), new Run(new Text("__________________"))));
                    body.AppendChild(new Paragraph(new Run(new Text(elem.Task7)), new Run(new Break()), new Run(new Text("__________________"))));
                    body.AppendChild(new Paragraph(new Run(new Text(elem.Task8)), new Run(new Break()), new Run(new Text("__________________"))));
                    body.AppendChild(new Paragraph(new Run(new Text(elem.Task9)), new Run(new Break()), new Run(new Text("__________________"))));
                    body.AppendChild(new Paragraph(new Run(new Text(elem.Task0)), new Run(new Break()), new Run(new Text("__________________"))));
                }
                
            }
        }


        public static void ConvertWriting(WritingTask elem)
        {
            //Путь к файлу
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\WritingTask{elem.id}.docx";

            //Создание документа
            using (WordprocessingDocument doc = WordprocessingDocument.Create(filePath,
                WordprocessingDocumentType.Document))
            {
                //Основная часть документа
                MainDocumentPart mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());

                //Тип задания
                body.AppendChild(new Paragraph(new Run(new Text($"Тип задания: {elem.TaskType}"))));

                body.AppendChild(new Paragraph(new Run(new Text($"Рекомендованное время {elem.RecommendedTime} мин"))));

                if (elem != null)
                {
                    //Заголовок
                    body.AppendChild(new Paragraph(
                        new Run(new RunProperties(new FontSize() { Val = "32" }, new Bold()), new Text($"Id задания - {elem.id}"),
                        new Run(new Break()), // Перенос
                        new Run(new RunProperties(new FontSize() { Val = "32" }, new Bold()), new Text($"{elem.TaskText}")))));

                    body.AppendChild(new Paragraph(new Run(new RunProperties(new FontSize() { Val = "20"}), new Text($"Темы"))));

                    body.AppendChild(new Paragraph(new Run(new Text($"{elem.Topics[0]}"))));
                    body.AppendChild(new Paragraph(new Run(new Text($"{elem.Topics[1]}"))));
                    body.AppendChild(new Paragraph(new Run(new Text($"{elem.Topics[2]}"))));
                    body.AppendChild(new Paragraph(new Run(new Text($"{elem.Topics[3]}"))));
                    body.AppendChild(new Paragraph(new Run(new Text($"{elem.Topics[4]}"))));
                    body.AppendChild(new Paragraph(new Run(new Text($"{elem.Topics[5]}"))));


                }
                
            }
        }

        public static void ConvertSpeaking(SpeakingTask elem)
        {
            //Путь к файлу
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\SpeakingTask{elem.id}.docx";

            //Создание документа
            using (WordprocessingDocument doc = WordprocessingDocument.Create(filePath,
                WordprocessingDocumentType.Document))
            {
                //Основная часть документа
                MainDocumentPart mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());
                
                //Тип задания
                body.AppendChild(new Paragraph(new Run(new Text($"Тип задания: {elem.TaskType}"))));

                body.AppendChild(new Paragraph(new Run(new Text($"Рекомендованное время {elem.RecommendedTime} мин"))));

                if (elem != null)
                {
                    //Заголовок
                    body.AppendChild(new Paragraph(
                        new Run(new RunProperties(new FontSize() { Val = "30" }, new Bold()), new Text($"Id задания - {elem.id}"),
                        new Run(new Break()), // Перенос
                        new Run(new RunProperties(new FontSize() { Val = "30" }, new Bold()), new Text($"{elem.TaskText}")))));
                }
                
            }
        }

        public static void ConvertMessage() //метод для вывода сообщения о конвертации
        {
            MessageBox.Show(SetLanguageResources.GetString(Properties.Settings.Default.Language, "fileInstalled"));
        }

    }
}
