using System.Windows;

namespace Digimezzo.WPFControls.Tests.Tests
{
    public partial class SplitViewTest : Window
    {
        public SplitViewTest()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.MySplitView.IsPaneOpen = !this.MySplitView.IsPaneOpen;
        }
    }
}
