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
    /// Логика взаимодействия для Chernovik.xaml
    /// </summary>
    public partial class Chernovik : UserControl
    {
        private readonly DoubleAnimation _oa;
        List<Global_Test> answ = new List<Global_Test>();
        public int f = 1;
        //public static CreateTest Instance { get; private set; }
        public Chernovik()
        {
            InitializeComponent();
            this.Opacity = 0.1;
            _oa = new DoubleAnimation();
            _oa.From = Opacity;
            _oa.To = 1;
            _oa.Duration = new Duration(TimeSpan.FromMilliseconds(1000d));
            questions.Visibility = Visibility.Hidden;
            //Global_Test.it = this;
            login.IsEnabled = false;
            predmet.IsEnabled = false; 
            end_redac.Visibility = Visibility.Hidden;
            end.Visibility = Visibility.Hidden;

            string[] allfiles = Directory.GetDirectories(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\Temp_Tests" + @"\");
            foreach (string filename in allfiles)
            {
                if (new DirectoryInfo(filename).Name != "Temp_Tests")
                    grup.Items.Add(new DirectoryInfo(filename).Name);
            }

            tip.Items.Add("Один вариант ответа");
            tip.Items.Add("Несколько вариантов ответа");
            tip.Items.Add("Текст");
        }

        public void ClearAll()
        {
            end_redac.Visibility = Visibility.Hidden;
            //Один вариант ответа
            tip_one.Visibility = Visibility.Hidden;
            tip_one_1.IsChecked = false; tip_one_2.IsChecked = false; tip_one_3.IsChecked = false; tip_one_4.IsChecked = false;
            tip_one_1_1.Text = ""; tip_one_1_2.Text = ""; tip_one_1_3.Text = ""; tip_one_1_4.Text = "";

            //Несколько вариантов ответа
            tip_two.Visibility = Visibility.Hidden;
            tip_two_1.IsChecked = false; tip_two_2.IsChecked = false; tip_two_3.IsChecked = false; tip_two_4.IsChecked = false;
            tip_two_1_1.Text = ""; tip_two_1_2.Text = ""; tip_two_1_3.Text = ""; tip_two_1_4.Text = "";

            //Текст
            tip_tri.Visibility = Visibility.Hidden;
            tip_tri_1_1.Text = "";
        }

        private void grup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            predmet.Items.Clear();
            if (grup.SelectedIndex != -1)
            {
                string[] allfiles = Directory.GetDirectories(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\Temp_Tests" + @"\" + grup.SelectedItem.ToString());
                foreach (string filename in allfiles)
                {
                    predmet.Items.Add(new DirectoryInfo(filename).Name);
                }
            }
            predmet.IsEnabled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (create.Content.ToString() == "Отобразить тест")
            {
                if(grup.SelectedIndex == -1 || predmet.SelectedIndex == -1 || login.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите все поля!");
                    return;
                }

                answ.Clear();
                voprosi.ItemsSource = "";
                int d = 1;
                string[] allfiles = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\Temp_Tests\" + grup.SelectedItem.ToString() + @"\" + predmet.SelectedItem.ToString() + @"\" + login.Text + @"\");
                foreach (string filename in allfiles)
                {
                    answ.Add(new Global_Test() { Path_toTest = filename, nomer = "Вопрос №" + d.ToString() });
                    d++;
                }
                voprosi.ItemsSource = answ;
                vope.Content = "Вопрос №" + d.ToString();


                login.IsEnabled = false; grup.IsEnabled = false; predmet.IsEnabled = false;
                end.Visibility = Visibility.Visible; end.IsEnabled = true;

                login.IsEnabled = false; grup.IsEnabled = false; predmet.IsEnabled = false;
                create.Visibility = Visibility.Hidden;
                questions.Visibility = Visibility.Visible;
            }
            else
            {
                login.IsEnabled = false; grup.IsEnabled = false; predmet.IsEnabled = false;
                create.Visibility = Visibility.Hidden;
                questions.Visibility = Visibility.Visible;
            }
        }

        private void tip_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearAll();
            if (tip.SelectedItem == null)
                return;

            if (tip.SelectedItem.ToString() == "Один вариант ответа")
            {
                tip_one.Visibility = Visibility.Visible;
            }
            else if (tip.SelectedItem.ToString() == "Несколько вариантов ответа")
            {
                tip_two.Visibility = Visibility.Visible;
            }
            else if (tip.SelectedItem.ToString() == "Текст")
            {
                tip_tri.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (nazv.Text.Length > nazv.Text.Trim().Length)
            {
                MessageBox.Show("Пробелы в начале и конце названия вопроса недопустимы");
                return;
            }
            else if (nazv.Text.Length < 2)
            {
                MessageBox.Show("Длина вопроса должна быть больше 2");
                return;
            }
            else if (tip.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите тип вопроса!");
                return;
            }
            try
            {
                int j = Convert.ToInt32(ball.Text);
            }
            catch
            {
                MessageBox.Show("Количество баллов указывается целым числом");
                return;
            }

            if (tip.SelectedItem.ToString() == "Один вариант ответа")
            {
                if (tip_one_1.IsChecked == false && tip_one_2.IsChecked == false && tip_one_3.IsChecked == false && tip_one_4.IsChecked == false)
                {
                    MessageBox.Show("Выберите правильный ответ!");
                    return;
                }
                else if (tip_one_1_1.Text == "" || tip_one_1_2.Text == "" || tip_one_1_3.Text == "" || tip_one_1_4.Text == "")
                {
                    MessageBox.Show("Заполните варианты ответов!");
                    return;
                }
            }

            if (tip.SelectedItem.ToString() == "Несколько вариантов ответа")
            {
                if (tip_two_1.IsChecked == false && tip_two_2.IsChecked == false && tip_two_3.IsChecked == false && tip_two_4.IsChecked == false)
                {
                    MessageBox.Show("Выберите правильный ответ!");
                    return;
                }
                else if (tip_two_1_1.Text == "" || tip_two_1_2.Text == "" || tip_two_1_3.Text == "" || tip_two_1_4.Text == "")
                {
                    MessageBox.Show("Заполните варианты ответов!");
                    return;
                }
            }

            if (tip.SelectedItem.ToString() == "Текст")
            {
                if (tip_tri_1_1.Text == "")
                {
                    MessageBox.Show("Заполните ответ!");
                    return;
                }
            }

            MessageBox.Show("Вопрос записан!");

            if (zaver.Content.ToString() == "Завершить вопрос")
            {
                int h = 1;
                while (File.Exists(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\Temp_Tests\" + grup.SelectedItem.ToString() + @"\" + predmet.SelectedItem.ToString() + @"\"  + login.Text + @"\" + h.ToString() + ".bin"))
                {
                    h++;
                }

                if (tip.SelectedItem.ToString() == "Один вариант ответа")
                {
                    using (BinaryWriter writer = new BinaryWriter(File.Open(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\Temp_Tests\" + grup.SelectedItem.ToString() + @"\" + predmet.SelectedItem.ToString() + @"\" + login.Text + @"\" + h.ToString() + ".bin", FileMode.OpenOrCreate)))
                    {
                        writer.Write(nazv.Text); //Вопрос
                        writer.Write(tip.SelectedItem.ToString()); //Тип
                        writer.Write(ball.Text); //Балл

                        if (tip_one_1.IsChecked == true) //Индекс правильного ответа
                            writer.Write("1");
                        else if (tip_one_2.IsChecked == true)
                            writer.Write("2");
                        else if (tip_one_3.IsChecked == true)
                            writer.Write("3");
                        else if (tip_one_4.IsChecked == true)
                            writer.Write("4");

                        writer.Write(tip_one_1_1.Text); //Варианты ответа
                        writer.Write(tip_one_1_2.Text);
                        writer.Write(tip_one_1_3.Text);
                        writer.Write(tip_one_1_4.Text);
                    }
                }
                else if (tip.SelectedItem.ToString() == "Несколько вариантов ответа")
                {
                    using (BinaryWriter writer = new BinaryWriter(File.Open(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\Temp_Tests\" + grup.SelectedItem.ToString() + @"\" + predmet.SelectedItem.ToString() + @"\" + login.Text + @"\" + h.ToString() + ".bin", FileMode.OpenOrCreate)))
                    {
                        writer.Write(nazv.Text); //Вопрос
                        writer.Write(tip.SelectedItem.ToString()); //Тип
                        writer.Write(ball.Text); //Балл

                        int t = 0;
                        if (tip_two_1.IsChecked == true) //Индекс правильного ответа
                            t++;
                        if (tip_two_2.IsChecked == true)
                            t++;
                        if (tip_two_3.IsChecked == true)
                            t++;
                        if (tip_two_4.IsChecked == true)
                            t++;

                        writer.Write(tip_two_1_1.Text); //Варианты ответа
                        writer.Write(tip_two_1_2.Text);
                        writer.Write(tip_two_1_3.Text);
                        writer.Write(tip_two_1_4.Text);

                        writer.Write(t); //Количество правильных ответов
                        if (tip_two_1.IsChecked == true) //Индекс правильного ответа
                            writer.Write("1");
                        if (tip_two_2.IsChecked == true)
                            writer.Write("2");
                        if (tip_two_3.IsChecked == true)
                            writer.Write("3");
                        if (tip_two_4.IsChecked == true)
                            writer.Write("4");
                    }
                }
                else if (tip.SelectedItem.ToString() == "Текст")
                {
                    using (BinaryWriter writer = new BinaryWriter(File.Open(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\Temp_Tests\" + grup.SelectedItem.ToString() + @"\" + predmet.SelectedItem.ToString() + @"\"  + login.Text + @"\" + h.ToString() + ".bin", FileMode.OpenOrCreate)))
                    {
                        writer.Write(nazv.Text); //Вопрос
                        writer.Write(tip.SelectedItem.ToString()); //Тип
                        writer.Write(ball.Text); //Балл
                        writer.Write(tip_tri_1_1.Text);
                    }
                }

                answ.Clear();
                voprosi.ItemsSource = "";
                int d = 1;
                string[] allfiles = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\Temp_Tests\" + grup.SelectedItem.ToString() + @"\" + predmet.SelectedItem.ToString() + @"\" + login.Text + @"\");
                foreach (string filename in allfiles)
                {
                    answ.Add(new Global_Test() { Path_toTest = filename, nomer = "Вопрос №" + d.ToString() });
                    d++;
                }
                voprosi.ItemsSource = answ;

                nazv.Text = "";
                tip.SelectedIndex = -1;
                ball.Text = "";
                vope.Content = "Вопрос №" + d.ToString();
                end.IsEnabled = true;

                ClearAll();
            }
            else if (zaver.Content.ToString() == "Изменить вопрос")
            {
                zaver.Content = "Завершить вопрос";
                string lol = vope.Content.ToString();
                lol = lol.Remove(0, 8);
                end_redac.Visibility = Visibility.Hidden;
                File.Delete(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\Temp_Tests\" + grup.SelectedItem.ToString() + @"\" + predmet.SelectedItem.ToString() + @"\" + login.Text + @"\" + lol + ".bin");
                if (tip.SelectedItem.ToString() == "Один вариант ответа")
                {
                    using (BinaryWriter writer = new BinaryWriter(File.Open(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\Temp_Tests\" + grup.SelectedItem.ToString() + @"\" + predmet.SelectedItem.ToString() + @"\" + login.Text + @"\" + lol + ".bin", FileMode.OpenOrCreate)))
                    {
                        writer.Write(nazv.Text); //Вопрос
                        writer.Write(tip.SelectedItem.ToString()); //Тип
                        writer.Write(ball.Text); //Балл

                        if (tip_one_1.IsChecked == true) //Индекс правильного ответа
                            writer.Write("1");
                        else if (tip_one_2.IsChecked == true)
                            writer.Write("2");
                        else if (tip_one_3.IsChecked == true)
                            writer.Write("3");
                        else if (tip_one_4.IsChecked == true)
                            writer.Write("4");

                        writer.Write(tip_one_1_1.Text); //Варианты ответа
                        writer.Write(tip_one_1_2.Text);
                        writer.Write(tip_one_1_3.Text);
                        writer.Write(tip_one_1_4.Text);
                    }
                }
                else if (tip.SelectedItem.ToString() == "Несколько вариантов ответа")
                {
                    using (BinaryWriter writer = new BinaryWriter(File.Open(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\Temp_Tests\" + grup.SelectedItem.ToString() + @"\" + predmet.SelectedItem.ToString() + @"\" + login.Text + @"\" + lol + ".bin", FileMode.OpenOrCreate)))
                    {
                        writer.Write(nazv.Text); //Вопрос
                        writer.Write(tip.SelectedItem.ToString()); //Тип
                        writer.Write(ball.Text); //Балл

                        int t = 0;
                        if (tip_two_1.IsChecked == true) //Индекс правильного ответа
                            t++;
                        if (tip_two_2.IsChecked == true)
                            t++;
                        if (tip_two_3.IsChecked == true)
                            t++;
                        if (tip_two_4.IsChecked == true)
                            t++;

                        writer.Write(tip_two_1_1.Text); //Варианты ответа
                        writer.Write(tip_two_1_2.Text);
                        writer.Write(tip_two_1_3.Text);
                        writer.Write(tip_two_1_4.Text);

                        writer.Write(t); //Количество правильных ответов
                        if (tip_two_1.IsChecked == true) //Индекс правильного ответа
                            writer.Write("1");
                        if (tip_two_2.IsChecked == true)
                            writer.Write("2");
                        if (tip_two_3.IsChecked == true)
                            writer.Write("3");
                        if (tip_two_4.IsChecked == true)
                            writer.Write("4");
                    }
                }
                else if (tip.SelectedItem.ToString() == "Текст")
                {
                    using (BinaryWriter writer = new BinaryWriter(File.Open(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\Temp_Tests\" + grup.SelectedItem.ToString() + @"\" + predmet.SelectedItem.ToString() + @"\" + login.Text + @"\" + lol + ".bin", FileMode.OpenOrCreate)))
                    {
                        writer.Write(nazv.Text); //Вопрос
                        writer.Write(tip.SelectedItem.ToString()); //Тип
                        writer.Write(ball.Text); //Балл
                        writer.Write(tip_tri_1_1.Text);
                    }
                }

                answ.Clear();
                voprosi.ItemsSource = "";
                int d = 1;
                string[] allfiles = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\Temp_Tests\" + grup.SelectedItem.ToString() + @"\" + predmet.SelectedItem.ToString() + @"\" + login.Text + @"\");
                foreach (string filename in allfiles)
                {
                    answ.Add(new Global_Test() { Path_toTest = filename, nomer = "Вопрос №" + d.ToString() });
                    d++;
                }
                voprosi.ItemsSource = answ;

                nazv.Text = "";
                tip.SelectedIndex = -1;
                ball.Text = "";
                vope.Content = "Вопрос №" + d.ToString();
                end.IsEnabled = true;

                ClearAll();
            }
        }

        public void AddDataListBox()
        {
            answ.Clear();
            int d = 1;
            voprosi.ItemsSource = "";
            string[] allfiles = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\Temp_Tests\" + grup.SelectedItem.ToString() + @"\" + predmet.SelectedItem.ToString() + @"\" + login.Text + @"\");
            foreach (string filename in allfiles)
            {
                answ.Add(new Global_Test() { Path_toTest = filename, nomer = "Вопрос №" + d.ToString() });
                d++;
            }
            voprosi.ItemsSource = answ;
        }

        private void voprosi_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (voprosi.SelectedItem != null)
            {
                string path = ""; string h = "";
                path = voprosi.SelectedValue.ToString();
                using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
                {
                    while (reader.PeekChar() > -1)
                    {
                        nazv.Text = reader.ReadString();
                        tip.Text = reader.ReadString();
                        if (tip.Text == "Один вариант ответа")
                        {
                            ball.Text = reader.ReadString();
                            h = reader.ReadString();
                            tip_one_1_1.Text = reader.ReadString();
                            tip_one_1_2.Text = reader.ReadString();
                            tip_one_1_3.Text = reader.ReadString();
                            tip_one_1_4.Text = reader.ReadString();
                            if (h == "1")
                                tip_one_1.IsChecked = true;
                            else if (h == "2")
                                tip_one_2.IsChecked = true;
                            else if (h == "3")
                                tip_one_3.IsChecked = true;
                            else if (h == "4")
                                tip_one_4.IsChecked = true;
                        }
                        else if (tip.Text == "Несколько вариантов ответа")
                        {
                            ball.Text = reader.ReadString();
                            tip_two_1_1.Text = reader.ReadString();
                            tip_two_1_2.Text = reader.ReadString();
                            tip_two_1_3.Text = reader.ReadString();
                            tip_two_1_4.Text = reader.ReadString();
                            int k = reader.ReadInt32();
                            for (int i = 0; i < k; i++)
                            {
                                string li = reader.ReadString();
                                if (li == "1")
                                    tip_two_1.IsChecked = true;
                                if (li == "2")
                                    tip_two_2.IsChecked = true;
                                if (li == "3")
                                    tip_two_3.IsChecked = true;
                                if (li == "4")
                                    tip_two_4.IsChecked = true;
                            }
                        }
                        else if (tip.Text == "Текст")
                        {
                            ball.Text = reader.ReadString();
                            tip_tri_1_1.Text = reader.ReadString();
                        }
                        break;
                    }
                }
                end_redac.Visibility = Visibility.Visible;
                Global_Test t = (Global_Test)voprosi.SelectedItem;
                vope.Content = t.nomer;
                zaver.Content = "Изменить вопрос";
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (voprosi.SelectedItem != null)
            {
                string path = "";
                Global_Test t = (Global_Test)voprosi.SelectedItem;
                if (t.nomer == vope.Content.ToString())
                {
                    zaver.Content = "Завершить вопрос";
                    nazv.Text = "";
                    tip.SelectedIndex = -1;
                    ball.Text = "";
                    end.IsEnabled = true;
                }
                File.Delete(voprosi.SelectedValue.ToString());
                path = voprosi.SelectedValue.ToString();
                path = new FileInfo(path).DirectoryName;
                string[] allfiles = Directory.GetFiles(path);
                int k = allfiles.Length; int j = 1;
                foreach (string filename in allfiles)
                {
                    File.Move(filename, path + @"\" + j.ToString() + ".bin");
                    j++;
                }
                vope.Content = "Вопрос №" + j.ToString();
                AddDataListBox();
            }
        }

        private void end_Click(object sender, RoutedEventArgs e)
        {
            if (answ.Count < 1)
            {
                MessageBox.Show("Нет ни одного вопроса в тесте! Добавьте вопросы");
                return;
            }
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\" + grup.SelectedItem.ToString() + @"\" + predmet.SelectedItem.ToString() + @"\" + login.Text);
            DirectoryCopy(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\Temp_Tests\" + grup.SelectedItem.ToString() + @"\" + predmet.SelectedItem.ToString() + @"\" + login.Text,
                          Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\" + grup.SelectedItem.ToString() + @"\" + predmet.SelectedItem.ToString() + @"\" + login.Text, true);
            Directory.Delete(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\Temp_Tests\" + grup.SelectedItem.ToString() + @"\" + predmet.SelectedItem.ToString() + @"\" + login.Text, true);
            MessageBox.Show("Тест успешно создан!");
            zaver.Content = "Завершить вопрос";

            questions.Visibility = Visibility.Hidden;
            predmet.IsEnabled = false;
            end.Visibility = Visibility.Hidden;
            login.Text = ""; login.IsEnabled = true;
            grup.SelectedIndex = -1; grup.IsEnabled = true;
            predmet.IsEnabled = false; create.Visibility = Visibility.Visible;

            ClearAll();
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

        private void end_redac_Click(object sender, RoutedEventArgs e)
        {
            answ.Clear();
            voprosi.ItemsSource = "";
            int d = 1;
            string[] allfiles = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\Temp_Tests" + @"\" + grup.SelectedItem.ToString() + @"\" + predmet.SelectedItem.ToString() + @"\" + login.Text + @"\");
            foreach (string filename in allfiles)
            {
                answ.Add(new Global_Test() { Path_toTest = filename, nomer = "Вопрос №" + d.ToString() });
                d++;
            }
            voprosi.ItemsSource = answ;
            zaver.Content = "Завершить вопрос";
            nazv.Text = "";
            tip.SelectedIndex = -1;
            ball.Text = "";
            vope.Content = "Вопрос №" + d.ToString();
            end.IsEnabled = true;
            ClearAll();
        }

        private void predmet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            login.Items.Clear();
            if (predmet.SelectedIndex != -1)
            {
                string[] allfiles = Directory.GetDirectories(Directory.GetCurrentDirectory() + @"\Пользователи\Преподаватели\" + Globals.Login + @"\Temp_Tests" + @"\" + grup.SelectedItem.ToString()+@"\"+predmet.SelectedItem.ToString());
                foreach (string filename in allfiles)
                {
                    login.Items.Add(new DirectoryInfo(filename).Name);
                }
            }
            login.IsEnabled = true;
            //predmet.IsEnabled = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BeginAnimation(OpacityProperty, _oa);
        }
    }
}
