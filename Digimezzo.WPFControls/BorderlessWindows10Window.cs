using Digimezzo.WPFControls.Base;
using System.Windows;
using System.Windows.Media;

namespace Digimezzo.WPFControls
{
    public abstract class BorderlessWindows10Window : BorderlessWindowBase
    {
        public bool ApplyDefaultButtonColors
        {
            get { return (bool)GetValue(ApplyDefaultButtonColorsProperty); }
            set { SetValue(ApplyDefaultButtonColorsProperty, value); }
        }

        public static readonly DependencyProperty ApplyDefaultButtonColorsProperty =
            DependencyProperty.Register(nameof(ApplyDefaultButtonColors), typeof(bool), typeof(BorderlessWindowBase), new PropertyMetadata(true));

        static BorderlessWindows10Window()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BorderlessWindows10Window), new FrameworkPropertyMetadata(typeof(BorderlessWindows10Window)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.ApplyDefaultButtonColors)
            {
                this.TitleBarHeight = 29;
                this.MinMaxBackground = Brushes.Transparent;
                this.MinMaxHoveredBackground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#E5E5E5"));
                this.MinMaxPressedBackground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#CACACB"));
                this.MinMaxForeground = Brushes.Black;
                this.MinMaxHoveredForeground = Brushes.Black;
                this.MinMaxPressedForeground = Brushes.Black;
                this.CloseBackground = Brushes.Transparent;
                this.CloseHoveredBackground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#E81123"));
                this.ClosePressedBackground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#F1707A"));
                this.CloseForeground = Brushes.Black;
                this.CloseHoveredForeground = Brushes.White;
                this.ClosePressedForeground = Brushes.White;
            }
        }
    }
}
