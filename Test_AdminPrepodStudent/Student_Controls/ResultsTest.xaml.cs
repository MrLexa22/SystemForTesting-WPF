using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Test_AdminPrepodStudent.Student_Controls
{
    /// <summary>
    /// Логика взаимодействия для ResultsTest.xaml
    /// </summary>
    public partial class ResultsTest : UserControl
    {
        private readonly DoubleAnimation _oa;
        List<Resu_st> tests = new List<Resu_st>();
        public ResultsTest()
        {
            InitializeComponent();
            this.Opacity = 0.1;
            _oa = new DoubleAnimation();
            _oa.From = Opacity;
            _oa.To = 1;
            _oa.Duration = new Duration(TimeSpan.FromMilliseconds(1000d));
            tests.Clear();
            Testi.ItemsSource = "";
            string fam = ""; string ima = ""; //Фамилия Имя преподавателя
            string disp = ""; //Название дисциплины
            string nametest = ""; //Название теста
            string pj = ""; //Путь до теста

            string path = Directory.GetCurrentDirectory() + @"\Пользователи";
            string[] allfiles = Directory.GetDirectories(path + @"\Преподаватели");
            foreach (string logins in allfiles)
            {
                string[] all2 = Directory.GetDirectories(logins);
                foreach (string grups in all2)
                {
                    if (new DirectoryInfo(grups).Name == Globals.Grupa)
                    {
                        using (BinaryReader reader = new BinaryReader(File.Open(logins + @"\info.bin", FileMode.Open)))
                        {
                            while (reader.PeekChar() > -1)
                            {
                                string pas = reader.ReadString();
                                fam = reader.ReadString();
                                ima = reader.ReadString();
                                string otch1 = reader.ReadString();
                                string gr = reader.ReadString();
                                break;
                            }
                        }

                        string[] all3 = Directory.GetDirectories(grups);
                        foreach (string disciplins in all3)
                        {
                            disp = new DirectoryInfo(disciplins).Name;  //Наименование дисциплины
                            string[] all4 = Directory.GetDirectories(disciplins);
                            foreach (string name_test in all4)
                            {
                                nametest = new DirectoryInfo(name_test).Name;
                                pj = name_test; string polb = ""; string mxb = "";
                                if (File.Exists(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + Globals.Grupa + @"\" + Globals.Login + @"\Тесты\" + disp + @"\" + nametest + @"\Результат.bin"))
                                {
                                    using (BinaryReader reader = new BinaryReader(File.Open(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + Globals.Grupa + @"\" + Globals.Login + @"\Тесты\" + disp + @"\" + nametest + @"\Результат.bin", FileMode.Open)))
                                    {
                                        while (reader.PeekChar() > -1)
                                        {
                                            polb = reader.ReadString();
                                            mxb = reader.ReadString();
                                            break;
                                        }
                                    }
                                    double pr = (Convert.ToInt32(polb) * 100) / Convert.ToInt32(mxb);
                                    pr = Math.Round(pr, 2);
                                    tests.Add(new Resu_st() { NazvTesta = nametest, Predmet = disp, Prepod = fam + " " + ima, PoluchBalls = polb, MaxBalls = mxb, Procents = pr.ToString() + "%" });
                                }
                            }
                        }
                    }
                }
            }
            Testi.ItemsSource = tests;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BeginAnimation(OpacityProperty, _oa);
        }
    }
    class Resu_st
    {
        public string NazvTesta { get; set; }
        public string Predmet { get; set; }
        public string Prepod { get; set; }
        public string PoluchBalls { get; set; }
        public string MaxBalls { get; set; }
        public string Procents { get; set; }
    }
}
