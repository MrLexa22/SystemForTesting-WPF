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
    /// Логика взаимодействия для Groups.xaml
    /// </summary>
    public partial class Groups : UserControl
    {
        private readonly DoubleAnimation _oa;
        List<Gups> ludi = new List<Gups>();
        public Groups()
        {
            InitializeComponent();
            this.Opacity = 0.1;
            _oa = new DoubleAnimation();
            _oa.From = Opacity;
            _oa.To = 1;
            _oa.Duration = new Duration(TimeSpan.FromMilliseconds(1000d));
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
                string[] allfiles1 = Directory.GetDirectories(path + @"\Студенты\"+ new DirectoryInfo(filename).Name);
                ludi.Add(new Gups() { Nazvanie = new DirectoryInfo(filename).Name, Kolvo=allfiles1.Length });
            }
            Polzovateli.ItemsSource = ludi;
        }

        private void Polzovateli_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            if (grid != null && grid.SelectedItems != null
                && grid.SelectedItems.Count == 1)
            {
                DataGridRow dgr = grid.ItemContainerGenerator.
                ContainerFromItem(grid.SelectedItem) as DataGridRow;
                Gups dr = (Gups)dgr.Item;
                NewGrup n = new NewGrup(dr.Nazvanie);
                n.ShowDialog();
                Polzovateli.UnselectAllCells();
                Polzovateli.UnselectAll();
                LoadUsers();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewGrup n = new NewGrup("Добавление");
            n.ShowDialog();
            LoadUsers();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BeginAnimation(OpacityProperty, _oa);
        }
    }
    class Gups
    {
        public string Nazvanie { get; set; }
        public int Kolvo { get; set; }
    }
}
