using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для NewUser.xaml
    /// </summary>
    public partial class NewUser : Window
    {
        private readonly DoubleAnimation _oa;
        public string lastlog = ""; public string grr = ""; public string rolu = "";
        public NewUser(string log, string r, string gr)
        {
            InitializeComponent();
            this.Opacity = 0.1;
            _oa = new DoubleAnimation();
            _oa.From = Opacity;
            _oa.To = 1;
            _oa.Duration = new Duration(TimeSpan.FromMilliseconds(1000d));
            cb.Items.Add("Студент");
            cb.Items.Add("Преподаватель");
            cb.SelectedIndex = 1;

            lastlog = log;
            grr = gr;
            rolu = r;
            rec.MouseLeftButtonDown += new MouseButtonEventHandler(layoutRoot_MouseLeftButtonDown);
            string path = Directory.GetCurrentDirectory() + @"\Пользователи";
            string[] allfiles = Directory.GetDirectories(path + @"\Студенты");
            foreach (string filename in allfiles)
            {
                if (new DirectoryInfo(filename).Name != "Temp_Tests")
                    grup.Items.Add(new DirectoryInfo(filename).Name);
            }

            if (log == "Добавление")
            {
                grup.Visibility = Visibility.Hidden;

                delete.Visibility = Visibility.Hidden;
            }
            else
            {
                cb.IsEditable = false;
                cb.IsEnabled = false;
                reg.Content = "Изменить";
                if(cb.SelectedItem.ToString() == "Студент")
                    grup.Visibility = Visibility.Visible;
                login.Text = log;
                path = Directory.GetCurrentDirectory();
                int j = 1;
                if (r == "Студент")
                {
                    using (BinaryReader reader = new BinaryReader(File.Open(path+@"\Пользователи\Студенты\"+gr+@"\"+log+@"\info.bin", FileMode.Open)))
                    {
                        while (reader.PeekChar() > -1)
                        {
                            pas.Password = reader.ReadString();
                            fam.Text = reader.ReadString();
                            ima.Text = reader.ReadString();
                            otch.Text = reader.ReadString();
                            cb.SelectedIndex = 0;
                            grup.Text = gr;
                            break;
                        }
                    }
                }
                else if (r == "Преподаватель")
                {
                    using (BinaryReader reader = new BinaryReader(File.Open(path + @"\Пользователи\Преподаватели\" + log + @"\info.bin", FileMode.Open)))
                    {
                        while (reader.PeekChar() > -1)
                        {
                            pas.Password = reader.ReadString();
                            fam.Text = reader.ReadString();
                            ima.Text = reader.ReadString();
                            otch.Text = reader.ReadString();
                            cb.SelectedIndex = 1;
                            break;
                        }
                    }
                }
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

        private void cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cb.SelectedItem.ToString() == "Студент")
            {
                grup.Visibility = Visibility.Visible;
            }
            else
            {
                grup.SelectedIndex = -1;
                grup.Visibility = Visibility.Hidden;
            }
        }

        private void reg_Click(object sender, RoutedEventArgs e)
        {
            if (reg.Content.ToString() == "Зарегистрировать")
            {
                if (login.Text == "Admin" || login.Text == "admin")
                {
                    MessageBox.Show("Логин уже существует!");
                    return;
                }
                else if (login.Text.Contains(" "))
                {
                    MessageBox.Show("Пробелы в логине недопустимы!");
                    return;
                }
                else if (login.Text.Length < 3)
                {
                    MessageBox.Show("Минимальная длина логина равна 3");
                    return;
                }
                else if (pas.Password.Length > pas.Password.Trim().Length)
                {
                    MessageBox.Show("Пробелы в начале и конце пароля недопустыми!");
                    return;
                }
                else if (pas.Password.Length < 8)
                {
                    MessageBox.Show("Минимальная длина пароля равна 8");
                    return;
                }    
                else if (!Regex.IsMatch(fam.Text, @"^[а-яА-Я]+$") || fam.Text.Length < 0)
                {
                    MessageBox.Show("Фамилия заполнена некорректно");
                    return;
                }
                else if (!Regex.IsMatch(ima.Text, @"^[а-яА-Я]+$") || ima.Text.Length < 0)
                {
                    MessageBox.Show("Имя заполнено некорректно");
                    return;
                }
                else if (!Regex.IsMatch(otch.Text, @"^[а-яА-Я]+$") && otch.Text.Length > 0)
                {
                    MessageBox.Show("Отчество заполнено некорректно");
                    return;
                }

                else if (grup.Visibility == Visibility.Visible)
                {
                    if (grup.SelectedIndex == -1)
                    {
                        MessageBox.Show("Укажите группу студенту!");
                        return;
                    }    
                }

                string[] allfiles = Directory.GetDirectories(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты");
                foreach (string filename in allfiles)
                {
                    if (Directory.Exists(filename + @"\" + login.Text.ToString()))
                    {
                        MessageBox.Show("Данный логин уже существует!");
                        return;
                    }
                }

                if (Directory.Exists(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + login.Text.ToString()))
                {
                    MessageBox.Show("Данный логин уже существует!");
                    return;
                }

                try
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\" + login.Text.ToString());
                    Directory.Delete(Directory.GetCurrentDirectory() + @"\" + login.Text.ToString());
                }
                catch
                {
                    MessageBox.Show("Данный логин содержит некорректные символы!");
                    return;
                }
                string path = ""; string lastpath = "";
                if (cb.SelectedItem.ToString() == "Преподаватель")
                {
                    path = Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + login.Text.ToString();
                    lastpath = Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + lastlog;
                }

                else if (cb.SelectedItem.ToString() == "Студент")
                {
                    path = Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + grup.SelectedItem.ToString() + @"\" + login.Text.ToString();
                    lastpath = Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + grup.SelectedItem.ToString() + @"\" + lastlog;
                }

                Directory.CreateDirectory(path);
                if (cb.SelectedItem.ToString() == "Преподаватель")
                    Directory.CreateDirectory(path+@"\Temp_Tests");


                using (BinaryWriter writer = new BinaryWriter(File.Open(path+@"\info.bin", FileMode.OpenOrCreate)))
                {
                    writer.Write(pas.Password.ToString());
                    writer.Write(fam.Text.ToString());
                    writer.Write(ima.Text.ToString());
                    writer.Write(otch.Text.ToString());
                    if(grup.SelectedIndex != -1)
                        writer.Write(grup.SelectedItem.ToString());
                    else
                        writer.Write("");
                }

                MessageBox.Show("Аккаут создан!");
                this.Close();
            }

            if (reg.Content.ToString() == "Изменить")
            {
                if(login.Text == "Admin" || login.Text == "admin")
                {
                    MessageBox.Show("Логин уже существует!");
                    return;
                }
                else if (login.Text.Contains(" "))
                {
                    MessageBox.Show("Пробелы в логине недопустимы!");
                    return;
                }
                else if (login.Text.Length < 3)
                {
                    MessageBox.Show("Минимальная длина логина равна 3");
                    return;
                }
                else if (pas.Password.Length > pas.Password.Trim().Length)
                {
                    MessageBox.Show("Пробелы в начале и конце пароля недопустыми!");
                    return;
                }
                else if (pas.Password.Length < 8)
                {
                    MessageBox.Show("Минимальная длина пароля равна 8");
                    return;
                }
                else if (!Regex.IsMatch(fam.Text, @"^[а-яА-Я]+$") || fam.Text.Length < 0)
                {
                    MessageBox.Show("Фамилия заполнена некорректно");
                    return;
                }
                else if (!Regex.IsMatch(ima.Text, @"^[а-яА-Я]+$") || ima.Text.Length < 0)
                {
                    MessageBox.Show("Имя заполнено некорректно");
                    return;
                }
                else if (!Regex.IsMatch(otch.Text, @"^[а-яА-Я]+$") && otch.Text.Length > 0)
                {
                    MessageBox.Show("Отчество заполнено некорректно");
                    return;
                }

                else if (grup.Visibility == Visibility.Visible)
                {
                    if (grup.SelectedIndex == -1)
                    {
                        MessageBox.Show("Укажите группу студенту!");
                        return;
                    }
                }

                string jk = "";
                if (grup.SelectedItem != null)
                    jk = grup.SelectedItem.ToString();


                if (lastlog != login.Text)
                {
                    string[] allfiles = Directory.GetDirectories(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты");
                    foreach (string filename in allfiles)
                    {
                        if (Directory.Exists(filename + @"\" + login.Text.ToString()))
                        {
                            MessageBox.Show("Данный логин уже существует!");
                            return;
                        }
                    }

                    if (Directory.Exists(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + login.Text.ToString()))
                    {
                        MessageBox.Show("Данный логин уже существует!");
                        return;
                    }

                    try
                    {
                        Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\" + login.Text.ToString());
                        Directory.Delete(Directory.GetCurrentDirectory() + @"\" + login.Text.ToString());
                    }
                    catch
                    {
                        MessageBox.Show("Данный логин содержит некорректные символы!");
                        return;
                    }
                }

                string path = ""; string lastpath = "";
                if (cb.SelectedItem.ToString() == "Преподаватель")
                {
                    path = Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + login.Text.ToString();
                    lastpath = Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + lastlog;
                }

                else if (cb.SelectedItem.ToString() == "Студент")
                {
                    path = Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + jk + @"\" + login.Text.ToString();
                    lastpath = Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + grr + @"\" + lastlog;
                }

                Directory.CreateDirectory(path);

                File.Delete(path + @"\info.bin");
                if (lastpath != path)
                {
                    DirectoryCopy(lastpath, path, true);
                    Directory.Delete(lastpath, true);
                }

                using (BinaryWriter writer = new BinaryWriter(File.Open(path + @"\info.bin", FileMode.OpenOrCreate)))
                {
                    writer.Write(pas.Password.ToString());
                    writer.Write(fam.Text.ToString());
                    writer.Write(ima.Text.ToString());
                    writer.Write(otch.Text.ToString());
                    if (grup.SelectedIndex != -1)
                        writer.Write(grup.SelectedItem.ToString());
                    else
                        writer.Write("");
                }
                MessageBox.Show("Аккаут изменён!");
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

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            string message = $@"Вы уверены, что желаете удалить пользователя {lastlog}?";
            string caption = "Ворота";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show(message, caption, buttons, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) 
            {
                string path = "";
                if (rolu == "Преподаватель")
                    path = Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + lastlog;
                else if (rolu == "Студент")
                    path = Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + grr + @"\" + lastlog;
                Directory.Delete(path, true);
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BeginAnimation(OpacityProperty, _oa);
        }
    }
}
