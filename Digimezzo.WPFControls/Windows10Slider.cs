using System.Windows;

namespace Digimezzo.WPFControls
{
    public class HorizontalWindows10Slider : HorizontalWindows8Slider
    {
        #region Construction
        static HorizontalWindows10Slider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HorizontalWindows10Slider), new FrameworkPropertyMetadata(typeof(HorizontalWindows10Slider)));
        }
        #endregion
    }

    public class VerticalWindows10Slider : VerticalWindows8Slider
    {
        #region Construction
        static VerticalWindows10Slider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VerticalWindows10Slider), new FrameworkPropertyMetadata(typeof(VerticalWindows10Slider)));
        }
        #endregion
    }
}
