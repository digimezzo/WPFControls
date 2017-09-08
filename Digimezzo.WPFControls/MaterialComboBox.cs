using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Digimezzo.WPFControls
{
    public class MaterialComboBox : ComboBox
    {
        private Border inputLine;
        private Border inputLineUnfocused;
        private ToggleButton toggleButton;
        private double opacity = 0.55;
        private bool isFocused;

        public Brush Accent
        {
            get { return (Brush)GetValue(AccentProperty); }
            set { SetValue(AccentProperty, value); }
        }

        public static readonly DependencyProperty AccentProperty =
            DependencyProperty.Register(nameof(Accent), typeof(Brush), typeof(MaterialComboBox), new PropertyMetadata(Brushes.Red));

        static MaterialComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialComboBox), new FrameworkPropertyMetadata(typeof(MaterialComboBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.toggleButton = (ToggleButton)GetTemplateChild("ToggleButton");
            this.inputLine = (Border)GetTemplateChild("PART_InputLine");
            this.inputLineUnfocused = (Border)GetTemplateChild("PART_InputLineUnfocused");
            this.toggleButton.Opacity = this.opacity;
            this.inputLineUnfocused.Opacity = this.opacity;
        }

        protected override void OnIsKeyboardFocusedChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsKeyboardFocusedChanged(e);
            bool isFocused = (bool)e.NewValue;

            if (!this.isFocused)
            {
                this.isFocused = true;
                this.AnimateInputLine(isFocused);
            }
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            if (!this.IsDropDownOpen)
            {
                this.AnimateInputLine(false);
            }
        }

        private void AnimateInputLine(bool mustFocus)
        {
            this.isFocused = mustFocus;

            var duration = new TimeSpan(0, 0, 0, 0, 200);
            var enlarge = new DoubleAnimation(0, this.ActualWidth, duration);
            var reduce = new DoubleAnimation(this.ActualWidth, 0, duration);

            this.inputLine.BeginAnimation(WidthProperty, mustFocus ? enlarge : reduce);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            if (this.isFocused)
            {
                this.inputLine.BeginAnimation(WidthProperty, null);
                this.inputLine.Width = this.ActualWidth;
            }
        }
    }
}
