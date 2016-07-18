using Digimezzo.WPFControls.Tests.Tests;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Digimezzo.WPFControls.Tests
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Windows8WindowTestButton_Click(object sender, RoutedEventArgs e)
        {
            var win = new Windows8WindowTest();
            win.Show();
        }

        private void Windows10WindowTestButton_Click(object sender, RoutedEventArgs e)
        {
            var win = new Windows10WindowTest();
            win.Show();
        }
    }
}
