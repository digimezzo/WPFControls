using System.Windows;
using System.Windows.Media;

namespace Digimezzo.WPFControls
{
    public class HorizontalUWPSlider : HorizontalWindows8Slider
    {
        #region Properties
        public Brush ButtonInnerBackground
        {
            get { return (Brush)GetValue(ButtonInnerBackgroundProperty); }
            set { SetValue(ButtonInnerBackgroundProperty, value); }
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty ButtonInnerBackgroundProperty = DependencyProperty.Register("ButtonInnerBackground", typeof(Brush), typeof(HorizontalUWPSlider), new PropertyMetadata(null));
        #endregion

        #region Construction
        static HorizontalUWPSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HorizontalUWPSlider), new FrameworkPropertyMetadata(typeof(HorizontalUWPSlider)));
        }
        #endregion
    }

    public class HorizontalUWPBottomSlider : HorizontalWindows8Slider
    {
        #region Properties
        public Brush ButtonInnerBackground
        {
            get { return (Brush)GetValue(ButtonInnerBackgroundProperty); }
            set { SetValue(ButtonInnerBackgroundProperty, value); }
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty ButtonInnerBackgroundProperty = DependencyProperty.Register("ButtonInnerBackground", typeof(Brush), typeof(HorizontalUWPBottomSlider), new PropertyMetadata(null));
        #endregion

        #region Construction
        static HorizontalUWPBottomSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HorizontalUWPBottomSlider), new FrameworkPropertyMetadata(typeof(HorizontalUWPBottomSlider)));
        }
        #endregion
    }

    public class VerticalUWPSlider : VerticalWindows8Slider
    {
        #region Properties
        public Brush ButtonInnerBackground
        {
            get { return (Brush)GetValue(ButtonInnerBackgroundProperty); }
            set { SetValue(ButtonInnerBackgroundProperty, value); }
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty ButtonInnerBackgroundProperty = DependencyProperty.Register("ButtonInnerBackground", typeof(Brush), typeof(VerticalUWPSlider), new PropertyMetadata(null));
        #endregion

        #region Construction
        static VerticalUWPSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VerticalUWPSlider), new FrameworkPropertyMetadata(typeof(VerticalUWPSlider)));
        }
        #endregion
    }
}
