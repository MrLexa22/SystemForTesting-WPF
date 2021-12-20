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

namespace Test_AdminPrepodStudent.Admin_Controls
{
    /// <summary>
    /// Логика взаимодействия для Disciplini.xaml
    /// </summary>
    public partial class Disciplini : UserControl
    {
        private readonly DoubleAnimation _oa;
        List<Disc> Discipl = new List<Disc>();
        public Disciplini()
        {
            InitializeComponent();
            this.Opacity = 0.1;
            _oa = new DoubleAnimation();
            _oa.From = Opacity;
            _oa.To = 1;
            _oa.Duration = new Duration(TimeSpan.FromMilliseconds(1000d));
            LoadData();
        }

        public void LoadData()
        {
            Discipl.Clear();
            Polzovateli.ItemsSource = "";
            string[] allfiles = Directory.GetDirectories(Directory.GetCurrentDirectory()+@"\Пользователи\Преподаватели");
            foreach (string filename in allfiles)
            {
                string pas = ""; string fam = ""; string ima = ""; string otch = ""; string gtup = ""; string discip="";

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

                string[] allfiles1 = Directory.GetDirectories(filename);
                foreach (string filename1 in allfiles1)
                {
                    gtup = new DirectoryInfo(filename1).Name;
                    string[] allfiles2 = Directory.GetDirectories(filename1);
                    foreach (string filename2 in allfiles2)
                    {
                        if (gtup != "Temp_Tests")
                        {
                            discip = new DirectoryInfo(filename2).Name;
                            Discipl.Add(new Disc { Login_Prepod = new DirectoryInfo(filename).Name, Group = gtup, Nazvanie = discip, Fam = fam, Ima = ima });
                        }
                    }
                }
            }
            Polzovateli.ItemsSource = Discipl;
        }
        

        private void Polzovateli_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            if (grid != null && grid.SelectedItems != null
                && grid.SelectedItems.Count == 1)
            {
                DataGridRow dgr = grid.ItemContainerGenerator.
                ContainerFromItem(grid.SelectedItem) as DataGridRow;
                Disc dr = (Disc)dgr.Item;

                NewDisc ad = new NewDisc(dr.Login_Prepod, dr.Nazvanie, dr.Group);
                ad.ShowDialog();

                Polzovateli.UnselectAllCells();
                Polzovateli.UnselectAll();
                LoadData();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewDisc n = new NewDisc("Добавление","","");
            n.ShowDialog();
            LoadData();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BeginAnimation(OpacityProperty, _oa);
        }
    }
    class Disc
    {
        public string Nazvanie { get; set; }
        public string Group { get; set; }
        public string Login_Prepod { get; set; }
        public string Fam { get; set; }
        public string Ima { get; set; }
    }
}
