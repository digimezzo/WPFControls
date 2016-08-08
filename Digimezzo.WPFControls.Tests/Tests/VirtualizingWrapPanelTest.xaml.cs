using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Digimezzo.WPFControls.Tests.Tests
{
    /// <summary>
    /// Interaction logic for VirtualizingWrapPanelTest.xaml
    /// </summary>
    public partial class VirtualizingWrapPanelTest : Window
    {
        public VirtualizingWrapPanelTest()
        {
            InitializeComponent();

            var images = new ObservableCollection<Image>();

            for (int i = 0; i < 50000; i++)
            {
                var im = new Image();
                im.Source = new BitmapImage(new Uri("CS.bmp", UriKind.Relative));
                im.Width = 90;
                im.Height = 110;
                images.Add(im);
            }

            Box.ItemsSource = images;
        }
    }
}
