using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Digimezzo.WPFControls
{
    public class ToggleSwitch : CheckBox
    {
        private Label onLabel;
        private Label offLabel;
     
        public Brush SwitchBackground
        {
            get { return (Brush)GetValue(SwitchBackgroundProperty); }
            set { SetValue(SwitchBackgroundProperty, value); }
        }

        public static readonly DependencyProperty SwitchBackgroundProperty =
          DependencyProperty.Register(nameof(SwitchBackground), typeof(Brush), typeof(ToggleSwitch), new PropertyMetadata(new SolidColorBrush(Colors.Blue)));
   
        public Brush ThumbBackground
        {
            get { return (Brush)GetValue(ThumbBackgroundProperty); }
            set { SetValue(ThumbBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ThumbBackgroundProperty =
           DependencyProperty.Register(nameof(ThumbBackground), typeof(Brush), typeof(ToggleSwitch), new PropertyMetadata(new SolidColorBrush(Colors.Black)));
       
        public string OnLabel
        {
            get { return Convert.ToString(GetValue(OnLabelProperty)); }
            set { SetValue(OnLabelProperty, value); }
        }

        public static readonly DependencyProperty OnLabelProperty =
           DependencyProperty.Register(nameof(OnLabel), typeof(string), typeof(ToggleSwitch), new PropertyMetadata("On"));
        
        public string OffLabel
        {
            get { return Convert.ToString(GetValue(OffLabelProperty)); }
            set { SetValue(OffLabelProperty, value); }
        }

        public static readonly DependencyProperty OffLabelProperty =
            DependencyProperty.Register(nameof(OffLabel), typeof(string), typeof(ToggleSwitch), new PropertyMetadata("Off"));

        static ToggleSwitch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleSwitch), new FrameworkPropertyMetadata(typeof(ToggleSwitch)));
        }
  
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
   
        private void ToggleSwitch_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}