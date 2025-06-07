using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using System.Diagnostics;
using Path = System.IO.Path;
using DocumentFormat.OpenXml.Office2013.Word;

namespace IELTSAppProject
{
    /// <summary>
    /// Логика взаимодействия для ReadingControl.xaml
    /// </summary>
    public partial class ReadingUserControl : UserControl, ICheckable
    {

        public string RecomTime { get; set; }
        public ReadingUserControl(ReadingTask data)
        {
            InitializeComponent();

            this.Loaded += (sender, e) =>
            {
                this.Focus();
                this.Focusable = true;
                Keyboard.Focus(this);
            };

            ////////запись в json
            //List<string> first = new List<string>();
            //List<string> second = new List<string>();
            //List<string> third = new List<string>();
            //List<string> fourth = new List<string>();
            //List<string> fifth = new List<string>();
            //List<string> answer = new List<string>();

            //AddToList(ref first, "they are a bacterial immune system", "they are DNA from viruses", "they aren't bacterial in origin");
            //AddToList(ref second, "biologists", "geneticists", "biologists and geneticists");
            //AddToList(ref third, "determines", "gains awarness", "adapts");
            //AddToList(ref fourth, "long history of existence", "immortality", "heritability");
            //AddToList(ref fifth, "It requires no energy to function.",
            //    "It provides immunity to offspring without prior exposure.", "It works only in laboratory conditions.");
            //AddToList(ref answer, "True", "False", "True", "False", "Not Stated", "they aren't bacterial in origin",
            //    "biologists and geneticists", "gains awarness", "heritability",
            //    "It provides immunity to offspring without prior exposure.");

            //ReadingTask task = new ReadingTask(5, "ReadingTask",
            //    "How bacteria invented gene editing\r\n\r\nThis week the UK Human Fertilisation and Embryology Authority okayed a proposal to modify human embryos through gene editing." +
            //    " The research, which will be carried out at the Francis Crick Institute in London, should improve our understanding of human development. It will also undoubtedly attract" +
            //    " controversy - particularly with claims that manipulating embryonic genomes is a first step towards designer babies. Those concerns shouldn't be ignored. After all, gene" +
            //    " editing of the kind that will soon be undertaken at the Francis Crick Institute doesn't occur naturally in humans or other animals.\r\nIt is, however, a lot more common" +
            //    " in nature than you might think, and it's been going on for a surprisingly long time - revelations that have challenged what biologists thought they knew about the way evolution" +
            //    " works. We're talking here about one particular gene editing technique called CRISPR-Cas, or just CRISPR. It's relatively fast, cheap and easy to edit genes with CRISPR" +
            //    " - factors that explain why the technique has exploded in popularity in the last few years. But CRISPR wasn't dreamed up from scratch in a laboratory. This gene editing" +
            //    " tool actually evolved in single-celled microbes.\r\nCRISPR went unnoticed by biologists for decades. It was only at the tail end of the 1980s that researchers studying" +
            //    " Escherichia coli noticed that there were some odd repetitive sequences at the end of one of the bacterial genes. Later, these sequences would be named Clustered Regularly" +
            //    " Interspaced Short Palindromic Repeats - CRISPRs. For several years the significance of these CRISPRs was a mystery, even when researchers noticed that they were always" +
            //    " separated from one another by equally odd 'spacer' gene sequences.\r\nThen, a little over a decade ago, scientists made an important discovery. Those 'spacer' sequences" +
            //    " look odd because they aren't bacterial in origin. Many are actually snippets of DNA from viruses that are known to attack bacteria. In 2005, three research groups independently" +
            //    " reached the same conclusion: CRISPR and its associated genetic sequences were acting as a bacterial immune system. In simple terms, this is how it works. A bacterial cell generates" +
            //    " special proteins from genes associated with the CRISPR repeats (these are called CRISPR associated - Cas - proteins). If a virus invades the cell, these Cas proteins bind to the" +
            //    " viral DNA and help cut out a chunk. Then, that chunk of viral DNA gets carried back to the bacterial cell's genome where it is inserted - becoming a spacer. From now on, the " +
            //    "bacterial cell can use the spacer to recognise that particular virus and attack it more effectively.\r\nThese findings were a revelation. Geneticists quickly realised that" +
            //    " the CRISPR system effectively involves microbes deliberately editing their own genomes - suggesting the system could form the basis of a brand new type of genetic engineering " +
            //    "technology. They worked out the mechanics of the CRISPR system and got it working in their lab experiments. It was a breakthrough that paved the way for this week's announcement" +
            //    " by the HFEA. Exactly who took the key steps to turn CRISPR into a useful genetic tool is, however, the subject of a huge controversy. Perhaps that's inevitable - credit for" +
            //    " developing CRISPR gene editing will probably guarantee both scientific fame and financial wealth.\r\nBeyond these very important practical applications, though, there's" +
            //    " another CRISPR story. It's the account of how the discovery of CRISPR has influenced evolutionary biology. Sometimes overlooked is the fact that it wasn't just geneticists" +
            //    " who were excited by CRISPR's discovery - so too were biologists. They realised CRISPR was evidence of a completely unexpected parallel between the way humans and bacteria " +
            //    "fight infections. We've known for a long time that part of our immune system \"learns\" about the pathogens it has seen before so it can adapt and fight infections better in" +
            //    " future. Vertebrate animals were thought to be the only organisms with such a sophisticated adaptive immune system. In light of the discovery of CRISPR, it seemed some bacteria" +
            //    " had their own version. In fact, it turned out that lots of bacteria have their own version. At the last count, the CRISPR adaptive immune system was estimated to be present in" +
            //    " about 40% of bacteria. Among the other major group of single-celled microbes - the archaea - CRISPR is even more common. It's seen in about 90% of them. If it's that common today," +
            //    " CRISPR must have a history stretching back over millions - possibly even billions - of years. \"It's clearly been around for a while,\" says Darren Griffin at the University of" +
            //    " Kent.\r\nThe animal adaptive immune system, then, isn't nearly as unique as we thought. And there's one feature of CRISPR that makes it arguably even better than our adaptive" +
            //    " immune system: CRISPR is heritable. When we are infected by a pathogen, our adaptive immune system learns from the experience, making our next encounter with that pathogen less of" +
            //    " an ordeal. This is why vaccination is so effective: it involves priming us with a weakened version of a pathogen to train our adaptive immune system. Your children, though, won't" +
            //    " benefit from the wealth of experience locked away in your adaptive immune system. They have to experience an infection - or be vaccinated - first hand before they can learn to deal" +
            //    " with a given pathogen.\r\nCRISPR is different. When a microbe with CRISPR is attacked by a virus, the record of the encounter is hardwired into the microbe's DNA as a new spacer." +
            //    " This is then automatically passed on when the cell divides into daughter cells, which means those daughter cells know how to fight the virus even before they've seen it. We don't know" +
            //    " for sure why the CRISPR adaptive immune system works in a way that seems, at least superficially, superior to ours. But perhaps our biological complexity is the problem, says Griffin." +
            //    " \"In complex organisms any minor [genetic] changes cause profound effects on the organism,\" he says. Microbes might be sturdy enough to constantly edit their genomes during their" +
            //    " lives and cope with the consequences - but animals probably aren't. The discovery of this heritable immune system was, however, a biologically astonishing one. It means that some" +
            //    " microbes write their lifetime experiences of their environment into their genome and then pass the information to their offspring – and that is something that evolutionary biologists" +
            //    " did not think happened.\r\n\r\nDarwin's theory of evolution is based on the idea that natural selection acts on the naturally occurring random variation in a population. Some organisms " +
            //    "are better adapted to the environment than others, and more likely to survive and reproduce, but this is largely because they just happened to be born that way. But before Darwin, other" +
            //    " scientists had suggested different mechanisms through which evolution might work. One of the most famous ideas was proposed by a French scientist called Jean-Bapteste Lamarck. He thought" +
            //    " organisms actually changed during their life, acquiring useful new adaptations non-randomly in response to their environmental experiences. They then passed on these changes to their offspring." +
            //    "\r\nPeople often use giraffes to illustrate Lamarck's hypothesis. The idea is that even deep in prehistory, the giraffe's ancestor had a penchant for leaves at the top of trees. This early" +
            //    " giraffe had a relatively short neck, but during its life it spent so much time stretching to reach leaves that its neck lengthened slightly. The crucial point, said Lamarck, was that this slightly" +
            //    " longer neck was somehow inherited by the giraffe's offspring. These giraffes also stretched to reach high leaves during their lives, meaning their necks lengthened just a little bit more, and so on." +
            //    " Once Darwin's ideas gained traction, Lamarck's ideas became deeply unpopular. But the CRISPR immune system - in which specific lifetime experiences of the environment are passed on to the next generation" +
            //    " - is one of a tiny handful of natural phenomena that arguably obeys Lamarckian principles.\r\n\"The realisation that Lamarckian type of evolution does occur and is common enough, was as startling to" +
            //    " biologists as it seems to a layperson,\" says Eugene Koonin at the National Institutes of Health in Bethesda, Maryland, who explored the idea with his colleagues in 2009, and does so again in a paper due" +
            //    " to be published later this year. This isn't to say that all of Lamarck's thoughts on evolution are back in vogue. \"Lamarck had additional ideas that were important to him, such as the inherent drive to" +
            //    " perfection that to him was a key feature of evolution,\" says Koonin. No modern evolutionary biologist goes along with that idea. But the discovery of the CRISPR system still implies that evolution isn't" +
            //    " purely the result of Darwinian random natural selection. It can sometimes involve elements of non-random Lamarckism too – a \"continuum\", as Koonin puts it. In other words, the CRISPR story has had a" +
            //    " profound scientific impact far beyond the doors of the genetic engineering lab. It truly was a transformative discovery.",
            //    20, answer,
            //    "You should spend about 20 minutes on Questions, which are based on Reading Passage 1 below.",
            //    "The research carried out at the Francis Crick Institute in London is likely to be controversial.",
            //    "Gene editing, like the one in the upcoming research, can happen naturally in humans or other animals.",
            //    "CRISPR-Cas is a gene editing technique.", "CRISPR was noticed when the researchers saw some odd repetitive sequences at the ends of all bacterial genes.",
            //    "A group of American researchers made an important revelation about the CRISPR.",
            //    "'Spacer' sequences look odd because:", "The ones, who were excited about the CRISPR's discovery, were:",
            //    "Word \"learns\" in the line 44, 6th paragraph means:", "What makes CRISPR better than even our adaptive immune system?",
            //    "What is a key advantage of CRISPR over the vertebrate immune system?",
            //    first, second, third, fourth, fifth);

         
            //string projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            //string file = Path.Combine(projectDir, "resourcesTask", "tasks", "tasks.json");
            //string jsonData = File.ReadAllText(file);


            //List<GeneralizedTask> list = JsonConvert.DeserializeObject<List<GeneralizedTask>>(jsonData) ?? new List<GeneralizedTask>();

            //list.Add(task);

            //string updatedJson = JsonConvert.SerializeObject(list, Formatting.Indented);
            //File.WriteAllText(file, updatedJson);
            //data = task;

            //ReadingTask newTask = null;

            //try
            //{
            //    string projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            //    string file = Path.Combine(projectDir, "resourcesTask", "tasks", "tasks.json");
            //    if (!File.Exists(file))
            //    {
            //        MessageBox.Show("Файл не найден!");
            //        return;
            //    }
            if (data != null)
            {
                this.RecomTime = data.RecommendedTime.ToString() + " мин.";
                //    // Сериализуем объект в JSON
                //    string jsonObject = JsonConvert.SerializeObject(data);

                //    // Записываем JSON в файл (перезаписываем, если существует)
                //    File.WriteAllText(file, jsonObject);

                //    // Читаем JSON из файла (после закрытия потока)
                //    string jsonFromFile = File.ReadAllText(file);

                //    // Десериализуем обратно в объект
                //    newTask = JsonConvert.DeserializeObject<ReadingTask>(jsonFromFile);

                this.DataContext = data;
            }
            
            //}
            //catch (IOException ex)
            //{
            //    MessageBox.Show($"Ошибка доступа к файлу: {ex.Message}");
            //}
            //catch (JsonException ex)
            //{
            //    MessageBox.Show($"Ошибка формата JSON: {ex.Message}");
            //}

            //подписка на событие клика
            convertToDocx.Click += (sender, e) => Conversion.ConvertReading(data);
            convertToDocx.Click += (sender, e) => MessageBox.Show("Файл/ы с заданием скачан и находится на вашем рабочем столе");
        }
        
        //проверка ответов
        private bool IsAnswerCorrect(string expected, params RadioButton[] options)
        {
            return options.Any(btn => btn.IsChecked == true && btn.Content?.ToString() == expected);
        }

        //открытие справки
        private void OpenChmHelp()
        {
            string chmPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Help",
                "referenceData.chm"
            );

            if (File.Exists(chmPath))
            {
                try
                {
                    // Открыть страницу "reading.htm" внутри CHM
                    Process.Start("hh.exe", $"{chmPath}::/reading.htm");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        //добавление в списки
        public static void AddToList(ref List<string> list, params string[] answers)
        {
            for (int i = 0; i < answers.Length; i++)
                list.Add(answers[i]);
        }

        //открытие справки
        private void help_Click(object sender, RoutedEventArgs e)
        {
            OpenChmHelp();
        }

        //проверка ответов
        public bool Check()
        {
            ReadingTask task = (ReadingTask)this.DataContext;
            bool result = false;
            if (task.Answer[0] == answer1.Text)
                rightAnswer1.Text = "Правильный ответ!";
            else
            {
                rightAnswer1.Text = "Неправильный ответ!";
                result = true;
            }


            if (task.Answer[1] == answer2.Text)
                rightAnswer2.Text = "Правильный ответ!";
            else
            {
                rightAnswer2.Text = "Неправильный ответ!";
                result = true;
            }


            if (task.Answer[2] == answer3.Text)
                rightAnswer3.Text = "Правильный ответ!";
            else
            {
                rightAnswer3.Text = "Неправильный ответ!";
                result = true;
            }


            if (task.Answer[3] == answer4.Text)
                rightAnswer4.Text = "Правильный ответ!";
            else
            {
                rightAnswer4.Text = "Неправильный ответ!";
                result = true;
            }


            if (task.Answer[4] == answer5.Text)
                rightAnswer5.Text = "Правильный ответ!";
            else
            {
                rightAnswer5.Text = "Неправильный ответ!";
                result = true;
            }


            if (IsAnswerCorrect(task.Answer[5], answer61, answer62, answer63))
                rightAnswer6.Text = "Правильный ответ!";
            else
            {
                result = true;
                rightAnswer6.Text = "Неправильный ответ!";
            }


            if (IsAnswerCorrect(task.Answer[6], answer71, answer72, answer73))
                rightAnswer7.Text = "Правильный ответ!";
            else
            {
                rightAnswer7.Text = "Неправильный ответ!";
                result = true;
            }

            if (IsAnswerCorrect(task.Answer[7], answer81, answer82, answer83))
                rightAnswer8.Text = "Правильный ответ!";
            else
            {
                rightAnswer8.Text = "Неправильный ответ!";
                result = true;
            }

            if (IsAnswerCorrect(task.Answer[8], answer91, answer92, answer93))
                rightAnswer9.Text = "Правильный ответ!";
            else
            {
                rightAnswer9.Text = "Неправильный ответ!";
                result = true;
            }

            if (IsAnswerCorrect(task.Answer[9], answer01, answer02, answer03))
                rightAnswer0.Text = "Правильный ответ!";
            else
            {
                rightAnswer0.Text = "Неправильный ответ!";
                result = true;
            }

            this.rightAnswer0.Visibility = Visibility.Visible;
            this.rightAnswer9.Visibility = Visibility.Visible;
            this.rightAnswer8.Visibility = Visibility.Visible;
            this.rightAnswer7.Visibility = Visibility.Visible;
            this.rightAnswer6.Visibility = Visibility.Visible;
            this.rightAnswer5.Visibility = Visibility.Visible;
            this.rightAnswer4.Visibility = Visibility.Visible;
            this.rightAnswer3.Visibility = Visibility.Visible;
            this.rightAnswer2.Visibility = Visibility.Visible;
            this.rightAnswer1.Visibility = Visibility.Visible;

            return result;
        }
    }
}
