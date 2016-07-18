using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Digimezzo.WPFControls
{
    public class Windows10ToggleSwitch : CheckBox
    {
        #region Properties
        public Brush Accent
        {
            get { return (Brush)GetValue(AccentProperty); }
            set { SetValue(AccentProperty, value); }
        }

        public string OnLabel
        {
            get { return Convert.ToString(GetValue(OnLabelProperty)); }
            set { SetValue(OnLabelProperty, value); }
        }

        public string OffLabel
        {
            get { return Convert.ToString(GetValue(OffLabelProperty)); }
            set { SetValue(OffLabelProperty, value); }
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty AccentProperty = DependencyProperty.Register("Accent", typeof(Brush), typeof(Windows10ToggleSwitch), new PropertyMetadata(new SolidColorBrush(Colors.Blue)));
        public static readonly DependencyProperty OnLabelProperty = DependencyProperty.Register("OnLabel", typeof(string), typeof(Windows10ToggleSwitch), new PropertyMetadata("On"));
        public static readonly DependencyProperty OffLabelProperty = DependencyProperty.Register("OffLabel", typeof(string), typeof(Windows10ToggleSwitch), new PropertyMetadata("Off"));
        #endregion

        #region Construction
        static Windows10ToggleSwitch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Windows10ToggleSwitch), new FrameworkPropertyMetadata(typeof(Windows10ToggleSwitch)));
        }
        #endregion
    }
}