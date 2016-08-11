using System.Windows;

namespace Digimezzo.WPFControls.Tests.Tests
{
    public partial class UWPSliderTest : Window
    {
        public UWPSliderTest()
        {
            InitializeComponent();

            this.HorizontalUWPSliderTrue.Value = 10;
            this.HorizontalUWPBottomSliderTrue.Value = 15;
        }
    }
}
