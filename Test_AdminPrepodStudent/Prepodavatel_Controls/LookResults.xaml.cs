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

namespace Test_AdminPrepodStudent.Prepodavatel_Controls
{
    /// <summary>
    /// Логика взаимодействия для LookResults.xaml
    /// </summary>
    public partial class LookResults : UserControl
    {
        private readonly DoubleAnimation _oa;
        List<Resu_pr> tests = new List<Resu_pr>();
        public LookResults()
        {
            InitializeComponent(); this.Opacity = 0.1;
            _oa = new DoubleAnimation();
            _oa.From = Opacity;
            _oa.To = 1;
            _oa.Duration = new Duration(TimeSpan.FromMilliseconds(1000d));
            string[] allfiles = Directory.GetDirectories(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login);
            foreach (string filename in allfiles)
            {
                if (new DirectoryInfo(filename).Name != "Temp_Tests")
                    grup.Items.Add(new DirectoryInfo(filename).Name);
            }
        }

        private void grup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tests.Clear();
            Testi.ItemsSource = "";
            predmet.IsEnabled = true;
            predmet.Items.Clear();
            if (grup.SelectedIndex != -1)
            {
                string[] allfiles = Directory.GetDirectories(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\" + grup.SelectedItem.ToString());
                foreach (string filename in allfiles)
                {
                    predmet.Items.Add(new DirectoryInfo(filename).Name);
                }
            }
        }

        private void predmet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (predmet.SelectedIndex != -1)
            {
                tests.Clear();
                Testi.ItemsSource = "";
                string nazv_test = ""; string polb = ""; string mxb = ""; string f = ""; string im = "";
                string[] all3 = Directory.GetDirectories(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + grup.SelectedItem.ToString());
                foreach (string disciplins in all3)
                {
                    if (Directory.Exists(disciplins + @"\Тесты\" + predmet.SelectedItem.ToString()))
                    {
                        using (BinaryReader reader = new BinaryReader(File.Open(disciplins + @"\info.bin", FileMode.Open)))
                        {
                            while (reader.PeekChar() > -1)
                            {
                                string pa = reader.ReadString();
                                f = reader.ReadString();
                                im = reader.ReadString();
                                pa = reader.ReadString();
                                break;
                            }
                        }

                        string[] all1 = Directory.GetDirectories(disciplins + @"\Тесты\" + predmet.SelectedItem.ToString());
                        foreach (string tests1 in all1)
                        {
                            nazv_test = new DirectoryInfo(tests1).Name;

                            using (BinaryReader reader = new BinaryReader(File.Open(tests1 + @"\Результат.bin", FileMode.Open)))
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
                            tests.Add(new Resu_pr() { NazvTesta = nazv_test, MaxBalls = mxb, PoluchBalls = polb, Procents = pr.ToString() + "%",stud=f+" "+im });
                        }
                    }
                }
                Testi.ItemsSource = tests;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BeginAnimation(OpacityProperty, _oa);
        }
    }
    class Resu_pr
    {
        public string NazvTesta { get; set; }
        public string PoluchBalls { get; set; }
        public string stud { get; set; }
        public string MaxBalls { get; set; }
        public string Procents { get; set; }
    }
}
