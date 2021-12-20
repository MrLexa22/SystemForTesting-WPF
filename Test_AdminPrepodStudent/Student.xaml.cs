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
using Test_AdminPrepodStudent.Student_Controls;

namespace Test_AdminPrepodStudent
{
    /// <summary>
    /// Логика взаимодействия для Student.xaml
    /// </summary>
    public partial class Student : UserControl
    {
        public string log = "";
        private readonly DoubleAnimation _oa;
        public Student(string login, string paths)
        {
            InitializeComponent();
            this.Opacity = 0.1;
            _oa = new DoubleAnimation();
            _oa.From = Opacity;
            _oa.To = 1;
            _oa.Duration = new Duration(TimeSpan.FromMilliseconds(1000d));
            string grupa = new DirectoryInfo(System.IO.Path.GetDirectoryName(paths)).Name;
            Globals.Grupa = grupa;
            using (BinaryReader reader = new BinaryReader(File.Open(paths+ @"\info.bin", FileMode.Open)))
            {
                while (reader.PeekChar() > -1)
                {
                    string pa = reader.ReadString();
                    lab_fam.Content = reader.ReadString();
                    lab_ima.Content = reader.ReadString();
                    lab_otch.Content = reader.ReadString();
                    break;
                }
            }
            gr.Content = grupa;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            control.Content = new GoTest();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //control.Content = new Chernovik();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //control.Content = new LookTests();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            control.Content = new ResultsTest();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BeginAnimation(OpacityProperty, _oa);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            ChPasSt p = new ChPasSt();
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            p.Owner = window;
            p.ShowDialog();
        }
    }
}
