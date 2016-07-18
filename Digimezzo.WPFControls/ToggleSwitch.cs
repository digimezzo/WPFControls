using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Digimezzo.WPFControls
{
    public class ToggleSwitch : CheckBox
    {
        #region Variables
        private Label onLabel;
        private Label offLabel;
        #endregion

        #region Properties
        public Brush SwitchBackground
        {
            get { return (Brush)GetValue(SwitchBackgroundProperty); }
            set { SetValue(SwitchBackgroundProperty, value); }
        }

        public Brush ThumbBackground
        {
            get { return (Brush)GetValue(ThumbBackgroundProperty); }
            set { SetValue(ThumbBackgroundProperty, value); }
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
        public static readonly DependencyProperty SwitchBackgroundProperty = DependencyProperty.Register("SwitchBackground", typeof(Brush), typeof(ToggleSwitch), new PropertyMetadata(new SolidColorBrush(Colors.Blue)));
        public static readonly DependencyProperty ThumbBackgroundProperty = DependencyProperty.Register("ThumbBackground", typeof(Brush), typeof(ToggleSwitch), new PropertyMetadata(new SolidColorBrush(Colors.Black)));
        public static readonly DependencyProperty OnLabelProperty = DependencyProperty.Register("OnLabel", typeof(string), typeof(ToggleSwitch), new PropertyMetadata("On"));
        public static readonly DependencyProperty OffLabelProperty = DependencyProperty.Register("OffLabel", typeof(string), typeof(ToggleSwitch), new PropertyMetadata("Off"));
        #endregion

        #region Construction
        static ToggleSwitch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleSwitch), new FrameworkPropertyMetadata(typeof(ToggleSwitch)));
        }
        #endregion

        #region Overrides
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.onLabel = (Label)GetTemplateChild("PART_OnLabel");
            this.offLabel = (Label)GetTemplateChild("PART_OffLabel");

            if (this.onLabel != null)
            {
                this.onLabel.MouseDown += ToggleSwitch_MouseDown;
            }

            if (this.offLabel != null)
            {
                this.offLabel.MouseDown += ToggleSwitch_MouseDown;
            }
        }
        #endregion

        #region Event Handlers
        private void ToggleSwitch_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        #endregion
    }
}