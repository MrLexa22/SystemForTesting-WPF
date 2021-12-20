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
    /// Логика взаимодействия для NewGrup.xaml
    /// </summary>
    public partial class NewGrup : Window
    {
        public string lastlog = "";
        private readonly DoubleAnimation _oa;
        void layoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        public NewGrup(string nazv)
        {
            InitializeComponent();
            this.Opacity = 0.1;
            _oa = new DoubleAnimation();
            _oa.From = Opacity;
            _oa.To = 1;
            _oa.Duration = new Duration(TimeSpan.FromMilliseconds(1000d));
            rec.MouseLeftButtonDown += new MouseButtonEventHandler(layoutRoot_MouseLeftButtonDown);
            if (nazv == "Добавление")
            {
                delete.Visibility = Visibility.Hidden;
                lastlog = nazv;
            }
            else
            {
                lastlog = nazv;
                reg.Content = "Изменить";
                login.Text = nazv;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            string message = $@"Вы уверены, что желаете удалить группу {lastlog}?"+"\n"+"Все студенты группы будут удалены!!";
            string caption = "Ворота";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show(message, caption, buttons, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                string path = "";
                path = Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + lastlog;
                Directory.Delete(path, true);

                string[] allfiles = Directory.GetDirectories(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели");
                foreach (string filename in allfiles)
                {
                    string[] allfiles1 = Directory.GetDirectories(filename);
                    foreach (string filename1 in allfiles1)
                    {
                        if (new DirectoryInfo(filename1).Name == lastlog)
                            Directory.Delete(filename1,true);
                    }
                }

                this.Close();
            }
        }

        private void reg_Click(object sender, RoutedEventArgs e)
        {
            if (lastlog == login.Text.Trim())
            {
                MessageBox.Show("Данные остались неизменными!");
                this.Close();
            }
            else if (lastlog == "Добавление")
            {
                if (login.Text.Contains(" "))
                {
                    MessageBox.Show("Пробелы в названии группы недопустимы!");
                    return;
                }
                else if (login.Text.Length < 2)
                {
                    MessageBox.Show("Минимальная длина названия группы равна 3");
                    return;
                }
                else if (login.Text.Length > login.Text.Trim().Length)
                {
                    MessageBox.Show("Пробелы в начале и конце названия недопустимы!");
                    return;
                }

                if (Directory.Exists(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты" + @"\" + login.Text.ToString()))
                {
                    MessageBox.Show("Данная группа уже существует!");
                    return;
                }

                try
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + login.Text.ToString());
                    Directory.Delete(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + login.Text.ToString(), true);
                }
                catch
                {
                    MessageBox.Show("Название гругппы содержит недопустимые символы");
                    return;
                }

                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты" + @"\" + login.Text.ToString());
                MessageBox.Show("Группа добавлена");
                this.Close();
            }
            else
            {
                if (login.Text.Contains(" "))
                {
                    MessageBox.Show("Пробелы в названии группы недопустимы!");
                    return;
                }
                else if (login.Text.Length < 2)
                {
                    MessageBox.Show("Минимальная длина названия группы равна 3");
                    return;
                }

                if (Directory.Exists(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты" + @"\" + login.Text.ToString()))
                {
                    MessageBox.Show("Данная группа уже существует!");
                    return;
                }

                try
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + login.Text.ToString());
                    Directory.Delete(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + login.Text.ToString(), true);
                }
                catch
                {
                    MessageBox.Show("Название гругппы содержит недопустимые символы");
                    return;
                }

                DirectoryCopy(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты" + @"\" + lastlog, Directory.GetCurrentDirectory() + @"\Пользователи\Студенты" + @"\" + login.Text.ToString(), true);
                Directory.Delete(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты" + @"\" + lastlog,true);

                string[] all1 = Directory.GetDirectories(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\");
                foreach (string filename in all1)
                {
                    string[] all2 = Directory.GetDirectories(filename);
                    foreach (string filename1 in all2)
                    {
                        if (new DirectoryInfo(filename1).Name == lastlog)
                            Directory.Move(filename1, filename + @"\" + login.Text);
                    }
                }

                MessageBox.Show("Группа изменена");
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
}
