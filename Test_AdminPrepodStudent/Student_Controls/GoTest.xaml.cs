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

namespace Test_AdminPrepodStudent.Student_Controls
{
    /// <summary>
    /// Логика взаимодействия для GoTest.xaml
    /// </summary>
    public partial class GoTest : UserControl
    {
        private readonly DoubleAnimation _oa;
        List<Testis> tests = new List<Testis>();
        List<Global_Test> answ = new List<Global_Test>();
        public GoTest()
        {
            InitializeComponent();
            this.Opacity = 0.1;
            _oa = new DoubleAnimation();
            _oa.From = Opacity;
            _oa.To = 1;
            _oa.Duration = new Duration(TimeSpan.FromMilliseconds(1000d));
            LoadTests();          
        }

        public int LoadTests()
        {
            zaver.Visibility = Visibility.Hidden;
            if (Directory.Exists(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + Globals.Grupa + @"\" + Globals.Login + @"\Test"))
                Directory.Delete(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + Globals.Grupa + @"\" + Globals.Login + @"\Test", true);

            tests.Clear();
            Testi.ItemsSource = "";
            string fam = ""; string ima = ""; //Фамилия Имя преподавателя
            string disp = ""; //Название дисциплины
            string nametest = ""; //Название теста
            string pj = ""; //Путь до теста

            string path = Directory.GetCurrentDirectory() + @"\Пользователи";
            string[] allfiles = Directory.GetDirectories(path + @"\Преподаватели");
            foreach (string logins in allfiles)
            {
                string[] all2 = Directory.GetDirectories(logins);
                foreach (string grups in all2)
                {
                    if (new DirectoryInfo(grups).Name == Globals.Grupa)
                    {
                        using (BinaryReader reader = new BinaryReader(File.Open(logins + @"\info.bin", FileMode.Open)))
                        {
                            while (reader.PeekChar() > -1)
                            {
                                string pas = reader.ReadString();
                                fam = reader.ReadString();
                                ima = reader.ReadString();
                                string otch1 = reader.ReadString();
                                string gr = reader.ReadString();
                                break;
                            }
                        }

                        string[] all3 = Directory.GetDirectories(grups);
                        foreach (string disciplins in all3)
                        {
                            disp = new DirectoryInfo(disciplins).Name;  //Наименование дисциплины
                            string[] all4 = Directory.GetDirectories(disciplins);
                            foreach (string name_test in all4)
                            {
                                nametest = new DirectoryInfo(name_test).Name;
                                pj = name_test; string sta = "";
                                if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\"+Globals.Grupa+@"\" + Globals.Login + @"\Тесты\"+disp+@"\" + nametest))
                                {
                                    sta = "Не пройден";
                                }
                                else
                                    sta = "Пройден";

                                if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + Globals.Grupa + @"\" + Globals.Login + @"\Test\" + disp + @"\" + nametest))
                                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + Globals.Grupa + @"\" + Globals.Login + @"\Test\" + disp + @"\" + nametest);

                                tests.Add(new Testis() { NazvTesta = nametest, path_to_test = pj, Predmet = disp, Prepod = fam + " " + ima, Status = sta });
                            }
                        }
                    }
                }
            }
            Testi.ItemsSource = tests;
            return 1;
        }

        private void Testi_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            if (grid != null && grid.SelectedItems != null
                && grid.SelectedItems.Count == 1)
            {
                DataGridRow dgr = grid.ItemContainerGenerator.
                ContainerFromItem(grid.SelectedItem) as DataGridRow;
                Testis dr = (Testis)dgr.Item;
                if (dr.Status == "Пройден")
                {
                    MessageBox.Show("Вы уже прошли этот тест!");
                    LoadTests();
                    return;
                }
                txt.Text = "Прохождение теста";
                Testi.Visibility = Visibility.Hidden;

                te.Visibility = Visibility.Visible;
                grup.Text = Globals.Grupa;
                predmet.Text = dr.Predmet;
                nazt.Text = dr.NazvTesta;

                answ.Clear();
                voprosi.ItemsSource = "";
                int d = 1;
                string[] allfiles = Directory.GetFiles(dr.path_to_test);
                foreach (string filename in allfiles)
                {
                    answ.Add(new Global_Test() { Path_toTest = filename, nomer = "Вопрос №" + d.ToString() });
                    d++;
                }
                voprosi.ItemsSource = answ;
                vope.Content = "Вопрос №";
                txt.Text = "Выберите тест";
            }
        }

        public void ClearAll()
        {
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

            zaver.Visibility = Visibility.Hidden;
            vope.Content = "Вопрос №";
            nazv.Text = "";
        }

        Global_Test tet = new Global_Test();

        private void voprosi_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (voprosi.SelectedItem != null)
            {
                ClearAll();
                string path = ""; string h = "";
                Global_Test t = (Global_Test)voprosi.SelectedItem;        
                zaver.Visibility = Visibility.Visible;
                path = voprosi.SelectedValue.ToString();
                tet = (Global_Test)voprosi.SelectedItem;
                using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
                {
                    while (reader.PeekChar() > -1)
                    {
                        nazv.Text = reader.ReadString();
                        string tip = reader.ReadString();
                        if (tip == "Один вариант ответа")
                        {
                            tip_one.Visibility = Visibility.Visible;
                            ball.Text = reader.ReadString();
                            h = reader.ReadString();
                            tip_one_1_1.Text = reader.ReadString();
                            tip_one_1_2.Text = reader.ReadString();
                            tip_one_1_3.Text = reader.ReadString();
                            tip_one_1_4.Text = reader.ReadString();
                        }
                        else if (tip == "Несколько вариантов ответа")
                        {
                            tip_two.Visibility = Visibility.Visible;
                            ball.Text = reader.ReadString();
                            tip_two_1_1.Text = reader.ReadString();
                            tip_two_1_2.Text = reader.ReadString();
                            tip_two_1_3.Text = reader.ReadString();
                            tip_two_1_4.Text = reader.ReadString();
                            int k = reader.ReadInt32();
                        }
                        else if (tip == "Текст")
                        {
                            tip_tri.Visibility = Visibility.Visible;
                            ball.Text = reader.ReadString();
                        }
                        break;
                    }
                }
                vope.Content = t.nomer;
            }
        }

        private void zaver_Click(object sender, RoutedEventArgs e)
        {
            string u = tet.nomer;
            u = u.Remove(0, 8);
            if(tip_one.Visibility == Visibility.Visible)
            {
                if(tip_one_1.IsChecked == false && tip_one_2.IsChecked == false && tip_one_3.IsChecked == false && tip_one_4.IsChecked == false )
                {
                    MessageBox.Show("Выберите ответ!");
                    return;
                }    
            }
            else if(tip_two.Visibility == Visibility.Visible)
            {
                if (tip_two_1.IsChecked == false && tip_two_2.IsChecked == false && tip_two_3.IsChecked == false && tip_two_4.IsChecked == false)
                {
                    MessageBox.Show("Выберите ответ!");
                    return;
                }
            }
            else if(tip_tri.Visibility == Visibility.Visible)
            {
                if(tip_tri_1_1.Text == "" || tip_tri_1_1.Text.Trim() == "")
                {
                    MessageBox.Show("Введите ответ!");
                    return;
                }
            }
            answ = answ.Where(x => x.nomer != tet.nomer).ToList();
            voprosi.ItemsSource = ""; voprosi.ItemsSource = answ;
            if (tip_one.Visibility == Visibility.Visible)
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + Globals.Grupa + @"\" + Globals.Login + @"\Test\" + predmet.Text+@"\" + nazt.Text +@"\"+ u.ToString() + ".bin", FileMode.OpenOrCreate)))
                {
                    writer.Write(nazv.Text); //Вопрос
                    writer.Write("Один вариант ответа"); //Тип
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
            else if (tip_two.Visibility == Visibility.Visible)
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + Globals.Grupa + @"\" + Globals.Login + @"\Test\" + predmet.Text + @"\" + nazt.Text + @"\" + u.ToString() + ".bin", FileMode.OpenOrCreate)))
                {
                    writer.Write(nazv.Text); //Вопрос
                    writer.Write("Несколько вариантов ответа"); //Тип
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
            else if (tip_tri.Visibility == Visibility.Visible)
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + Globals.Grupa + @"\" + Globals.Login + @"\Test\" + predmet.Text + @"\" + nazt.Text + @"\" + u.ToString() + ".bin", FileMode.OpenOrCreate)))
                {
                    writer.Write(nazv.Text); //Вопрос
                    writer.Write("Текст"); //Тип
                    writer.Write(ball.Text); //Балл
                    writer.Write(tip_tri_1_1.Text);
                }
            }
            MessageBox.Show("Ответ записан");

            if(voprosi.Items.Count == 0)
            {
                MessageBox.Show("Тест успешно завершён");
                if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + Globals.Grupa + @"\" + Globals.Login + @"\Тесты\" + predmet.Text + @"\" + nazt.Text))
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + Globals.Grupa + @"\" + Globals.Login + @"\Тесты\" + predmet.Text + @"\" + nazt.Text);
                DirectoryCopy(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + Globals.Grupa + @"\" + Globals.Login + @"\Test\" + predmet.Text + @"\" + nazt.Text, Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + Globals.Grupa + @"\" + Globals.Login + @"\Тесты\" + predmet.Text + @"\" + nazt.Text, true);
                Directory.Delete(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + Globals.Grupa + @"\" + Globals.Login + @"\Test\" + predmet.Text + @"\" + nazt.Text,true);

                
                string[] all4 = Directory.GetFiles(new FileInfo(tet.Path_toTest).DirectoryName);
                //MessageBox.Show(new FileInfo(tet.Path_toTest).DirectoryName);
                int balli = 0; int max_ball = 0;
                foreach (string name_test in all4)
                {
                    //MessageBox.Show(name_test);
                    string tip = ""; 
                    string answk = ""; string t1 = ""; string balls = ""; int ik = 0; string[] answk1 = new string[20]; 
                    using (BinaryReader reader = new BinaryReader(File.Open(name_test, FileMode.Open)))
                    {
                        while (reader.PeekChar() > -1)
                        {
                            t1 = reader.ReadString();
                            tip = reader.ReadString();
                            //MessageBox.Show(tip);
                            if (tip == "Один вариант ответа")
                            {
                                balls = reader.ReadString();
                                max_ball += Convert.ToInt32(balls);
                                answk = reader.ReadString();
                                t1 = reader.ReadString();
                                t1 = reader.ReadString();
                                t1 = reader.ReadString();
                                t1 = reader.ReadString();
                            }
                            else if (tip == "Несколько вариантов ответа")
                            {                            
                                balls = reader.ReadString();
                                max_ball += Convert.ToInt32(balls);
                                t1 = reader.ReadString();
                                t1 = reader.ReadString();
                                t1 = reader.ReadString();
                                t1 = reader.ReadString();
                                ik = reader.ReadInt32();
                                for (int i = 0; i < ik; i++)
                                {
                                    string li = reader.ReadString();
                                    if (li == "1")
                                        answk1[1] = "true";
                                    if (li == "2")
                                        answk1[2] = "true";
                                    if (li == "3")
                                        answk1[3] = "true";
                                    if (li == "4")
                                        answk1[4] = "true";
                                }
                            }
                            else if (tip == "Текст")
                            {
                                balls = reader.ReadString();
                                max_ball += Convert.ToInt32(balls);
                                answk = reader.ReadString();
                            }
                            break;
                        }
                    }

                    string answk_st = ""; string[] answk1_st = new string[20];
                    using (BinaryReader reader = new BinaryReader(File.Open(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + Globals.Grupa + @"\" + Globals.Login + @"\Тесты\" + predmet.Text + @"\" + nazt.Text+@"\"+new DirectoryInfo(name_test).Name, FileMode.Open)))
                    {
                        while (reader.PeekChar() > -1)
                        {
                            t1 = reader.ReadString();
                            tip = reader.ReadString();
                            if (tip == "Один вариант ответа")
                            {
                                t1 = reader.ReadString();
                                answk_st = reader.ReadString();
                                t1 = reader.ReadString();
                                t1 = reader.ReadString();
                                t1 = reader.ReadString();
                                t1 = reader.ReadString();
                            }
                            else if (tip == "Несколько вариантов ответа")
                            {

                                t1 = reader.ReadString();
                                t1 = reader.ReadString();
                                t1 = reader.ReadString();
                                t1 = reader.ReadString();
                                t1 = reader.ReadString();
                                ik = reader.ReadInt32();
                                for (int i = 0; i < ik; i++)
                                {
                                    string li = reader.ReadString();
                                    if (li == "1")
                                        answk1_st[1] = "true";
                                    if (li == "2")
                                        answk1_st[2] = "true";
                                    if (li == "3")
                                        answk1_st[3] = "true";
                                    if (li == "4")
                                        answk1_st[4] = "true";
                                }
                            }
                            else if (tip == "Текст")
                            {
                                t1 = reader.ReadString();
                                answk_st = reader.ReadString();
                            }
                            break;
                        }
                    }
                    //MessageBox.Show(max_ball.ToString());
                    if (tip == "Один вариант ответа")
                    {
                        if (answk == answk_st)
                        {
                            balli += Convert.ToInt32(balls);
                        }
                    }
                    else if (tip == "Несколько вариантов ответа")
                    {
                        if (answk1[1] == answk1_st[1] && answk1[2] == answk1_st[2] && answk1[3] == answk1_st[3] && answk1[4] == answk1_st[4])
                        {
                            balli += Convert.ToInt32(balls);
                        }
                    }
                    else if (tip == "Текст")
                    {
                        if(answk == answk_st)
                        {
                            balli += Convert.ToInt32(balls);
                        }    
                    }
                }
                using (BinaryWriter writer = new BinaryWriter(File.Open(Directory.GetCurrentDirectory() + @"\Пользователи\Студенты\" + Globals.Grupa + @"\" + Globals.Login + @"\Тесты\" + predmet.Text + @"\" + nazt.Text + @"\Результат" + ".bin", FileMode.OpenOrCreate)))
                {
                    writer.Write(balli.ToString());
                    writer.Write(max_ball.ToString());
                }

                Testi.Visibility = Visibility.Visible;
                te.Visibility = Visibility.Hidden;
                LoadTests();
                LoadTests();
                
                ClearAll();
            }
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

        private void create_Click(object sender, RoutedEventArgs e)
        {
            if(voprosi.Items.Count > 0)
            {
                MessageBox.Show("Вы не ответили на все вопросы! Тест будет завершён автоматически, после ответа на все вопросы");
                return;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BeginAnimation(OpacityProperty, _oa);
        }
    }
    class Testis
    {
        public string NazvTesta { get; set; }
        //public string log_prepod { get; set; }
        public string Predmet { get; set; }
        public string Prepod { get; set; }
        public string Status { get; set; }
        public string path_to_test { get; set; }
    }
}
