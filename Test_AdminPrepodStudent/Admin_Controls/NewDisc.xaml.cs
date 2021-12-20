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

namespace Test_AdminPrepodStudent.Admin_Controls
{
    /// <summary>
    /// Логика взаимодействия для NewDisc.xaml
    /// </summary>
    public partial class NewDisc : Window
    {
        private readonly DoubleAnimation _oa;
        public string last_discp = ""; public string last_gr = ""; public string last_prep = ""; public string g = "";
        List<Prepodavat> ludi = new List<Prepodavat>();
        public NewDisc(string deistv, string discl, string gro)
        {
            InitializeComponent();
            this.Opacity = 0.1;
            _oa = new DoubleAnimation();
            _oa.From = Opacity;
            _oa.To = 1;
            _oa.Duration = new Duration(TimeSpan.FromMilliseconds(1000d));
            g = deistv;
            string path = Directory.GetCurrentDirectory() + @"\Пользователи";
            string[] allfiles = Directory.GetDirectories(path + @"\Студенты");
            foreach (string filename in allfiles)
            {
                if (new DirectoryInfo(filename).Name != "Temp_Tests")
                    grup.Items.Add(new DirectoryInfo(filename).Name);
            }

            path = Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели";
            allfiles = Directory.GetDirectories(path);
            foreach (string filename in allfiles)
            {
                using (BinaryReader reader = new BinaryReader(File.Open(filename + @"\info.bin", FileMode.Open)))
                {
                    while (reader.PeekChar() > -1)
                    {
                        string pas = reader.ReadString();
                        string fam = reader.ReadString();
                        string ima = reader.ReadString();
                        string otch1 = reader.ReadString();
                        string gr = reader.ReadString();
                        ludi.Add(new Prepodavat() { Fam = fam, Ima = ima, Otch = otch1, Login = new DirectoryInfo(filename).Name});
                        break;
                    }
                }
            }
            prepod.ItemsSource = ludi;

            if (deistv != "Добавление")
            {
                string pas = ""; string fam = ""; string ima = ""; string otch = "";
                string filename = Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + deistv;
                using (BinaryReader reader = new BinaryReader(File.Open(filename + @"\info.bin", FileMode.Open)))
                {
                    while (reader.PeekChar() > -1)
                    {
                        pas = reader.ReadString();
                        fam = reader.ReadString();
                        ima = reader.ReadString();
                        otch = reader.ReadString();
                        string gr = reader.ReadString();
                        break;
                    }
                }
                last_discp = discl;
                login.Text = discl;

                prepod.SelectedValue = deistv;
                last_prep = deistv;

                grup.Text = gro;
                last_gr = gro;
            }

            rec.MouseLeftButtonDown += new MouseButtonEventHandler(layoutRoot_MouseLeftButtonDown);
            if (deistv == "Добавление")
            {
                delete.Visibility = Visibility.Hidden;
            }
            else
            {
                delete.Visibility = Visibility.Visible;
                reg.Content = "Изменить";
            }
        }

        void layoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void reg_Click(object sender, RoutedEventArgs e)
        {
            if (g == "Добавление")
            {
                if (login.Text.Trim().Length < 3)
                {
                    MessageBox.Show("Минимальная длина названия равна 3");
                    return;
                }
                else if (login.Text.Length > login.Text.Trim().Length)
                {
                    MessageBox.Show("Пробелы в начале и конце недопустимы");
                    return;
                }

                else if (grup.SelectedIndex == -1)
                {
                    MessageBox.Show("Укажите студенту!");
                    return;
                }

                else if (prepod.SelectedIndex == -1)
                {
                    MessageBox.Show("Укажите студенту!");
                    return;
                }

                if (Directory.Exists(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + prepod.SelectedValue.ToString() + @"\" + grup.SelectedItem.ToString() + @"\" + login.Text))
                {
                    MessageBox.Show("Указанный преподаватель уже ведёт эту дисциплину у этой группы");
                    return;
                }

                try
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Пользователи\" + login.Text);
                    Directory.Delete(Directory.GetCurrentDirectory() + @"\Пользователи\" + login.Text, true);
                }
                catch
                {
                    MessageBox.Show("Имя дисциплины содержит недопустимые символы!");
                    return;
                }

                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + prepod.SelectedValue.ToString() + @"\" + grup.SelectedItem.ToString() + @"\" + login.Text);
                MessageBox.Show("Дисциплина успешно создана!");
                this.Close();
            }
            else
            {
                if (login.Text.Trim().Length < 3)
                {
                    MessageBox.Show("Минимальная длина названия равна 3");
                    return;
                }
                else if (login.Text.Length > login.Text.Trim().Length)
                {
                    MessageBox.Show("Пробелы в начале и конце недопустимы");
                    return;
                }

                else if (grup.SelectedIndex == -1)
                {
                    MessageBox.Show("Укажите группу!");
                    return;
                }

                else if (prepod.SelectedIndex == -1)
                {
                    MessageBox.Show("Укажите преподавателя!");
                    return;
                }

                if (Directory.Exists(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + prepod.SelectedValue.ToString() + @"\" + grup.SelectedItem.ToString() + @"\" + login.Text))
                {
                    MessageBox.Show("Указанный преподаватель уже ведёт эту дисциплину у этой группы");
                    return;
                }

                try
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Пользователи\" + login.Text);
                    Directory.Delete(Directory.GetCurrentDirectory() + @"\Пользователи\" + login.Text, true);
                }
                catch
                {
                    MessageBox.Show("Имя дисциплины содержит недопустимые символы!");
                    return;
                }

                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + prepod.SelectedValue.ToString() + @"\" + grup.SelectedItem.ToString() + @"\" + login.Text);
                DirectoryCopy(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + last_prep + @"\" + last_gr + @"\" + last_discp, Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + prepod.SelectedValue.ToString() + @"\" + grup.SelectedItem.ToString() + @"\" + login.Text,true);
                Directory.Delete(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + last_prep + @"\" + last_gr + @"\" + last_discp,true);

                string[] all1 = Directory.GetDirectories(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\"+grup.SelectedItem.ToString()+@"\");
                foreach(string filename in all1)
                {
                    string[] all2 = Directory.GetDirectories(filename+@"\Тесты");
                    foreach(string filename1 in all2)
                    {
                        if (new DirectoryInfo(filename1).Name == last_discp)
                            Directory.Move(filename1, filename + @"\Тесты\" + login.Text);
                    }
                }

                this.Close();
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            string message = $@"Вы уверены, что желаете удалить дисциплину {last_discp}?" + "\n" + "Все тесты будут удалены!";
            string caption = "Ворота";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show(message, caption, buttons, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                string path = "";
                path = Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + last_prep+@"\"+last_gr+@"\"+last_discp;
                Directory.Delete(path, true);
                this.Close();
            }
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = System.IO.Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = System.IO.Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BeginAnimation(OpacityProperty, _oa);
        }
    }
    class Prepodavat
    {
        public string Fam { get; set; }
        public string Ima { get; set; }
        public string Otch { get; set; }
        public string Login { get; set; }
    }
}
