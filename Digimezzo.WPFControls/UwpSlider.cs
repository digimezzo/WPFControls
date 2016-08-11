using System.Windows;

namespace Digimezzo.WPFControls
{
    public class HorizontalUWPSlider : HorizontalWindows8Slider
    {
        #region Construction
        static HorizontalUWPSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HorizontalUWPSlider), new FrameworkPropertyMetadata(typeof(HorizontalUWPSlider)));
        }
        #endregion
    }

    public class HorizontalUWPBottomSlider : HorizontalWindows8Slider
    {
        #region Construction
        static HorizontalUWPBottomSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HorizontalUWPBottomSlider), new FrameworkPropertyMetadata(typeof(HorizontalUWPBottomSlider)));
        }
        #endregion
    }

    public class VerticalUWPSlider : VerticalWindows8Slider
    {
        #region Construction
        static VerticalUWPSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VerticalUWPSlider), new FrameworkPropertyMetadata(typeof(VerticalUWPSlider)));
        }
        #endregion
    }
}
