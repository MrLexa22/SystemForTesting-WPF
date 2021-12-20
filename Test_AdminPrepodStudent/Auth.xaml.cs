using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Test_AdminPrepodStudent
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : UserControl
    {
        private readonly DoubleAnimation _oa;

        public Auth()
        {
            InitializeComponent();
            this.Opacity = 0.1;
            _oa = new DoubleAnimation();
            _oa.From = Opacity;
            _oa.To = 1;
            _oa.Duration = new Duration(TimeSpan.FromMilliseconds(1000d));           
        }
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            BeginAnimation(OpacityProperty, _oa);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string path = Directory.GetCurrentDirectory(); string pas = "";
            if (login.Text == "admin")
            {
                if(password.Password == "password")
                {
                    Globals.Login = login.Text;
                    MainWindow window = (MainWindow)Application.Current.MainWindow;
                    window.ext.Visibility = Visibility.Visible;
                    window.contentControl.Content = new Admin1();
                }
                else
                {
                    MessageBox.Show("Авторизация не удалась!");
                    return;
                }
            }
            else if(Directory.Exists(path+@"\Пользователи\Преподаватели\"+login.Text))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(path + @"\Пользователи\Преподаватели\" + login.Text + @"\info.bin", FileMode.Open)))
                {
                    while (reader.PeekChar() > -1)
                    {
                        pas = reader.ReadString();
                        break;
                    }
                }
                if(password.Password == pas)
                {
                    //MessageBox.Show("Авторизация успешна!");
                    Globals.Login = login.Text;
                    MainWindow window = (MainWindow)Application.Current.MainWindow;
                    window.ext.Visibility = Visibility.Visible;
                    window.contentControl.Content = new Prepodavatel(login.Text);
                }
                else
                {
                    MessageBox.Show("Авторизация не удалась!");
                    return;
                }
            }
            else
            {
                string[] allfiles = Directory.GetDirectories(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты");
                string g = "";
                foreach (string filename in allfiles)
                {
                    string[] allfiles1 = Directory.GetDirectories(filename);
                    foreach (string filename1 in allfiles1)
                    {
                        if (new DirectoryInfo(filename1).Name == login.Text)
                        {
                            using (BinaryReader reader = new BinaryReader(File.Open(filename1 + @"\info.bin", FileMode.Open)))
                            {
                                while (reader.PeekChar() > -1)
                                {
                                    pas = reader.ReadString();
                                    g = filename1;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (password.Password == pas)
                {
                    //MessageBox.Show("Авторизация успешна!");
                    Globals.Login = login.Text;
                    MainWindow window = (MainWindow)Application.Current.MainWindow;
                    window.ext.Visibility = Visibility.Visible;
                    window.contentControl.Content = new Student(login.Text, g);
                }
                else
                {
                    MessageBox.Show("Авторизация не удалась!");
                    return;
                }
            }
        }
    }
}
