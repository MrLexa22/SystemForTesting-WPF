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
    /// Логика взаимодействия для CopyTests.xaml
    /// </summary>
    public partial class CopyTests : UserControl
    {
        private readonly DoubleAnimation _oa;
        public CopyTests()
        {
            InitializeComponent();
            this.Opacity = 0.1;
            _oa = new DoubleAnimation();
            _oa.From = Opacity;
            _oa.To = 1;
            _oa.Duration = new Duration(TimeSpan.FromMilliseconds(1000d));
            next_shag.Visibility = Visibility.Hidden;
            predmet.IsEnabled = false;
            login.IsEnabled = false;
            string[] allfiles = Directory.GetDirectories(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login);
            foreach (string filename in allfiles)
            {
                if (new DirectoryInfo(filename).Name != "Temp_Tests")
                    grup.Items.Add(new DirectoryInfo(filename).Name);
            }
        }

        private void grup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
            login.IsEnabled = true;
            login.Items.Clear();
            if (predmet.SelectedIndex != -1)
            {
                string[] allfiles = Directory.GetDirectories(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\" + grup.SelectedItem.ToString() + @"\"+predmet.SelectedItem.ToString());
                foreach (string filename in allfiles)
                {
                    login.Items.Add(new DirectoryInfo(filename).Name);
                }
            }
        }

        private void create_Click(object sender, RoutedEventArgs e)
        {
            if(grup.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите группу, откуда будете копировать тест");
                return;
            }
            else if (predmet.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите дисциплину, откуда будете копировать тест");
                return;
            }
            else if (login.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите название теста, который будете копировать");
                return;
            }

            grup.IsEnabled = false;
            predmet.IsEnabled = false;
            login.IsEnabled = false;
            next_shag.Visibility = Visibility.Visible;
            create.Visibility = Visibility.Hidden;

            predmet_Copy.IsEnabled = false;
            login_Copy.IsEnabled = false;

            string[] allfiles = Directory.GetDirectories(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login);
            foreach (string filename in allfiles)
            {
                if (new DirectoryInfo(filename).Name != "Temp_Tests" && new DirectoryInfo(filename).Name != grup.SelectedItem.ToString())
                    grup_Copy.Items.Add(new DirectoryInfo(filename).Name);
            }
        }

        private void grup_Copy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            predmet_Copy.IsEnabled = true;
            predmet_Copy.Items.Clear();
            if (grup_Copy.SelectedIndex != -1)
            {
                string[] allfiles = Directory.GetDirectories(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\" + grup_Copy.SelectedItem.ToString());
                foreach (string filename in allfiles)
                {
                    predmet_Copy.Items.Add(new DirectoryInfo(filename).Name);
                }
            }
        }

        private void predmet_Copy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (predmet.SelectedIndex != -1)
            {
                login_Copy.Text = "";
                login_Copy.IsEnabled = true;
            }
        }

        private void create_Copy_Click(object sender, RoutedEventArgs e)
        {
            if(login_Copy.Text.Length > login_Copy.Text.Trim().Length)
            {
                MessageBox.Show("Название теста не может содержать пробелы в начале теста и в конце!");
                return;
            }
            else if (Directory.Exists(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\" + grup_Copy.SelectedItem.ToString() + @"\" +predmet_Copy.SelectedItem.ToString() + @"\"+login_Copy.Text))
            {
                MessageBox.Show("Такой тест уже существует у этой группы!");
                return;
            }
            try
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\" + grup_Copy.SelectedItem.ToString() + @"\" + predmet_Copy.SelectedItem.ToString() + @"\" + login_Copy.Text);
            }
            catch
            {
                MessageBox.Show("Название теста содержит некорректные символы!");
                return;
            }
            DirectoryCopy(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\" + grup.SelectedItem.ToString() + @"\" + predmet.SelectedItem.ToString() + @"\"+login.SelectedItem.ToString(), Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\" + grup_Copy.SelectedItem.ToString() + @"\" + predmet_Copy.SelectedItem.ToString() + @"\" + login_Copy.Text,true);
            MessageBox.Show("Тест успешно скопирован!");

            grup_Copy.Items.Clear();
            predmet_Copy.Items.Clear();
            login_Copy.Text = "";

            next_shag.Visibility = Visibility.Hidden;
            create.Visibility = Visibility.Visible;
            create.IsEnabled = true;
            grup.IsEnabled = true;
            predmet.IsEnabled = true;
            login.IsEnabled = true;
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

        private void create_Copy1_Click(object sender, RoutedEventArgs e)
        {
            grup_Copy.Items.Clear();
            predmet_Copy.Items.Clear();
            login_Copy.Text = "";

            next_shag.Visibility = Visibility.Hidden;
            create.Visibility = Visibility.Visible;
            create.IsEnabled = true;
            grup.IsEnabled = true;
            predmet.IsEnabled = true;
            login.IsEnabled = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BeginAnimation(OpacityProperty, _oa);
        }
    }
}
