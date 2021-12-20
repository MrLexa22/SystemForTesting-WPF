using System;
using System.Collections.Generic;
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
using Test_AdminPrepodStudent.Admin;
using Test_AdminPrepodStudent.Admin_Controls;

namespace Test_AdminPrepodStudent
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin1 : UserControl
    {
        private readonly DoubleAnimation _oa;
        public Admin1()
        {
            InitializeComponent();
            this.Opacity = 0.1;
            _oa = new DoubleAnimation();
            _oa.From = Opacity;
            _oa.To = 1;
            //_oa.RepeatBehavior = RepeatBehavior.Forever;
            //_oa.AutoReverse = true;
            _oa.Duration = new Duration(TimeSpan.FromMilliseconds(1000d));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            control.Content = new Groups();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            BeginAnimation(OpacityProperty, _oa);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            control.Content = new Admin_Users();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            control.Content = new Disciplini();
        }
    }
}
