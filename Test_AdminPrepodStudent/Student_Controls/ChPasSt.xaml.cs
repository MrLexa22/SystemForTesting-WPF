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
using System.Windows.Shapes;

namespace Test_AdminPrepodStudent.Student_Controls
{
    /// <summary>
    /// Логика взаимодействия для ChPasSt.xaml
    /// </summary>
    public partial class ChPasSt : Window
    {
        public string old_passw = "";
        private readonly DoubleAnimation _oa;
        public ChPasSt()
        {
            InitializeComponent();
            rec.MouseLeftButtonDown += new MouseButtonEventHandler(layoutRoot_MouseLeftButtonDown);
            this.Opacity = 0.1;
            _oa = new DoubleAnimation();
            _oa.From = Opacity;
            _oa.To = 1;
           
            _oa.Duration = new Duration(TimeSpan.FromMilliseconds(1000d));
            login.Text = Globals.Login;
            using (BinaryReader reader = new BinaryReader(File.Open(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + Globals.Grupa + @"\" +Globals.Login+ @"\info.bin", FileMode.Open)))
            {
                while (reader.PeekChar() > -1)
                {
                    old_passw = reader.ReadString();
                    fam.Text = reader.ReadString();
                    ima.Text = reader.ReadString();
                    otch.Text = reader.ReadString();
                    cb.Text = reader.ReadString();
                    break;
                }
            }
        }

        void layoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void old_pas_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (old_pas.Password.Length > 0)
            {
                new_pas.IsEnabled = true;
            }
            else
            {
                new_pas.Password = "";
                new_pas.IsEnabled = false;
            }
        }

        private void new_pas_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (new_pas.Password.Length > 0)
            {
                reg.IsEnabled = true;
            }
            else
            {
                reg.IsEnabled = false;
            }
        }

        private void reg_Click(object sender, RoutedEventArgs e)
        {
            if (old_pas.Password != old_passw)
            {
                MessageBox.Show("Вы ввели неправильный старый пароль!");
                return;
            }
            else if (new_pas.Password.Length < 8)
            {
                MessageBox.Show("Длина пароля должна быть минимум 8 символов");
                return;
            }
            else if (old_pas.Password == new_pas.Password)
            {
                MessageBox.Show("Вы ввели такой же пароль, как и был");
                return;
            }
            File.Delete(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + Globals.Grupa + @"\" + Globals.Login + @"\info.bin");
            using (BinaryWriter writer = new BinaryWriter(File.Open(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + Globals.Grupa + @"\" + Globals.Login + @"\info.bin", FileMode.OpenOrCreate)))
            {
                writer.Write(new_pas.Password.ToString());
                writer.Write(fam.Text.ToString());
                writer.Write(ima.Text.ToString());
                writer.Write(otch.Text.ToString());
                writer.Write(cb.Text);
            }
            MessageBox.Show("Пароль изменён!");
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BeginAnimation(OpacityProperty, _oa);
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
