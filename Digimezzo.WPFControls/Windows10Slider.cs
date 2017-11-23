using System.Windows;

namespace Digimezzo.WPFControls
{
    public class HorizontalWindows10Slider : HorizontalWindows8Slider
    {
        static HorizontalWindows10Slider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HorizontalWindows10Slider), new FrameworkPropertyMetadata(typeof(HorizontalWindows10Slider)));
        }
    }

    public class VerticalWindows10Slider : VerticalWindows8Slider
    {
        static VerticalWindows10Slider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VerticalWindows10Slider), new FrameworkPropertyMetadata(typeof(VerticalWindows10Slider)));
        }
    }
}
