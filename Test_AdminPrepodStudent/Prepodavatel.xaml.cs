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
using Test_AdminPrepodStudent.Prepodavatel_Controls;

namespace Test_AdminPrepodStudent
{
    /// <summary>
    /// Логика взаимодействия для Prepodavatel.xaml
    /// </summary>
    public partial class Prepodavatel : UserControl
    {
        public string log = "";
        private readonly DoubleAnimation _oa;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BeginAnimation(OpacityProperty, _oa);
        }

        public Prepodavatel(string login)
        {
            InitializeComponent();
            this.Opacity = 0.1;
            _oa = new DoubleAnimation();
            _oa.From = Opacity;
            _oa.To = 1;
            _oa.Duration = new Duration(TimeSpan.FromMilliseconds(1000d));

            string path = Directory.GetCurrentDirectory();
            using (BinaryReader reader = new BinaryReader(File.Open(path + @"\Пользователи\Преподаватели\" + login + @"\info.bin", FileMode.Open)))
            {
                while (reader.PeekChar() > -1)
                {
                    string pa = reader.ReadString();
                    lab_fam.Content = reader.ReadString();
                    lab_ima.Content = reader.ReadString();
                    lab_otch.Content = reader.ReadString();
                    //cb.SelectedIndex = 1;
                    break;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            control.Content = new CreateTest();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            control.Content = new Chernovik();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            control.Content = new LookTests();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            control.Content = new CopyTests();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            control.Content = new LookResults();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            ChPassPrep p = new ChPassPrep();
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            p.Owner = window;
            p.ShowDialog();
        }
    }
}
