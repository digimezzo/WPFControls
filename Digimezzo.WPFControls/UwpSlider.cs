using System.Windows;
using System.Windows.Media;

namespace Digimezzo.WPFControls
{
    public class HorizontalUwpSlider : HorizontalWindows8Slider
    {
        #region Properties
        public Brush ButtonInnerBackground
        {
            get { return (Brush)GetValue(ButtonInnerBackgroundProperty); }
            set { SetValue(ButtonInnerBackgroundProperty, value); }
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty ButtonInnerBackgroundProperty = DependencyProperty.Register("ButtonInnerBackground", typeof(Brush), typeof(HorizontalUwpSlider), new PropertyMetadata(null));
        #endregion

        #region Construction
        static HorizontalUwpSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HorizontalUwpSlider), new FrameworkPropertyMetadata(typeof(HorizontalUwpSlider)));
        }
        #endregion
    }

    public class HorizontalUwpBottomSlider : HorizontalWindows8Slider
    {
        #region Properties
        public Brush ButtonInnerBackground
        {
            get { return (Brush)GetValue(ButtonInnerBackgroundProperty); }
            set { SetValue(ButtonInnerBackgroundProperty, value); }
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty ButtonInnerBackgroundProperty = DependencyProperty.Register("ButtonInnerBackground", typeof(Brush), typeof(HorizontalUwpBottomSlider), new PropertyMetadata(null));
        #endregion

        #region Construction
        static HorizontalUwpBottomSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HorizontalUwpBottomSlider), new FrameworkPropertyMetadata(typeof(HorizontalUwpBottomSlider)));
        }
        #endregion
    }

    public class VerticalUwpSlider : VerticalWindows8Slider
    {
        #region Properties
        public Brush ButtonInnerBackground
        {
            get { return (Brush)GetValue(ButtonInnerBackgroundProperty); }
            set { SetValue(ButtonInnerBackgroundProperty, value); }
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty ButtonInnerBackgroundProperty = DependencyProperty.Register("ButtonInnerBackground", typeof(Brush), typeof(VerticalUwpSlider), new PropertyMetadata(null));
        #endregion

        #region Construction
        static VerticalUwpSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VerticalUwpSlider), new FrameworkPropertyMetadata(typeof(VerticalUwpSlider)));
        }
        #endregion
    }
}
