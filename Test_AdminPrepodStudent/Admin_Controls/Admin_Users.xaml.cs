using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
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
using Test_AdminPrepodStudent.Admin_Controls;

namespace Test_AdminPrepodStudent.Admin
{
    /// <summary>
    /// Логика взаимодействия для Admin_Users.xaml
    /// </summary>
    public partial class Admin_Users : UserControl
    {
        List<User> ludi = new List<User>();
        private readonly DoubleAnimation _oa;
        public Admin_Users()
        {
            InitializeComponent();
            this.Opacity = 0.1;
            _oa = new DoubleAnimation();
            _oa.From = Opacity;
            _oa.To = 1;
            _oa.Duration = new Duration(TimeSpan.FromMilliseconds(1000d));
            rol.Items.Add("Студент");
            rol.Items.Add("Преподаватель");
            rol.Items.Add("Все");
            rol.SelectedIndex = 2;
            Polzovateli.ItemsSource = ludi;
            LoadUsers();
        }

        public void LoadUsers()
        {

            Polzovateli.ItemsSource = "";
            ludi.Clear();
            string path = Directory.GetCurrentDirectory() + @"\Пользователи";

            string[] allfiles = Directory.GetDirectories(path + @"\Студенты");
            foreach (string filename in allfiles)
            {
                string[] all2 = Directory.GetDirectories(filename);
                foreach (string userna in all2)
                {
                    using (BinaryReader reader = new BinaryReader(File.Open(userna + @"\info.bin", FileMode.Open)))
                    {
                        while (reader.PeekChar() > -1)
                        {
                            string pas = reader.ReadString();
                            string fam = reader.ReadString();
                            string ima = reader.ReadString();
                            string otch1 = reader.ReadString();
                            string gr = reader.ReadString();
                            ludi.Add(new User() { Fam = fam, Ima = ima, Othc = otch1, Login = new DirectoryInfo(userna).Name, Role = "Студент", Grupa = gr });
                            break;
                        }
                    }
                }
            }

            allfiles = Directory.GetDirectories(path + @"\Преподаватели");
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
                        ludi.Add(new User() { Fam = fam, Ima = ima, Othc = otch1, Login = new DirectoryInfo(filename).Name, Role = "Преподаватель", Grupa = "" });
                        break;
                    }
                }
            }
            Polzovateli.ItemsSource = ludi;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewUser n = new NewUser("Добавление", "","");
            n.ShowDialog();
            LoadUsers();
        }

        private void Polzovateli_MouseLeave(object sender, MouseEventArgs e)
        {
            Polzovateli.SelectedItem = -1;
        }

        private void Polzovateli_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            if (grid != null && grid.SelectedItems != null
                && grid.SelectedItems.Count == 1)
            {
                DataGridRow dgr = grid.ItemContainerGenerator.
                ContainerFromItem(grid.SelectedItem) as DataGridRow;
                User dr = (User)dgr.Item;

                NewUser ad = new NewUser(dr.Login, dr.Role, dr.Grupa);
                ad.ShowDialog();
                //MessageBox.Show(dr.Login.ToString());
                Polzovateli.UnselectAllCells();
                Polzovateli.UnselectAll();
                LoadUsers();
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rol.SelectedIndex > -1)
            {
                if (rol.SelectedItem.ToString() != "Все")
                {
                    var sorted = ludi.FindAll(x => x.Role == rol.SelectedItem.ToString()).ToList();
                    Polzovateli.ItemsSource = sorted;
                    
                    if (rol.SelectedItem.ToString() == "Студент")
                        grup.Visibility = Visibility.Visible;
                    else
                        grup.Visibility = Visibility.Hidden;
                }
                else if (rol.SelectedItem.ToString() == "Все")
                    Polzovateli.ItemsSource = ludi;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            DataGrid grid = Polzovateli;
            if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
            {
                DataGridRow dgr = grid.ItemContainerGenerator.
                ContainerFromItem(grid.SelectedItem) as DataGridRow;
                User dr = (User)dgr.Item;

                MessageBox.Show(dr.Fam);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DataGrid grid = Polzovateli;

            if (e.ChangedButton == MouseButton.Right && grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
            {
                ContextMenu menu = new ContextMenu();
                MenuItem mi_add = new MenuItem();
                mi_add.Header = "Удалить строку";
                mi_add.Click += MenuItem_Click;
                menu.Items.Add(mi_add);
                menu.IsOpen = true;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BeginAnimation(OpacityProperty, _oa);
        }
    }
    class User
    {
        public string Fam { get; set; }
        public string Ima { get; set; }
        public string Othc { get; set; }
        public string Login { get; set; }
        public string Role { get; set; }
        public string Grupa { get; set; }
    }
}
