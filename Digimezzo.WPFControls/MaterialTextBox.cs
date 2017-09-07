using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Digimezzo.WPFControls
{
    public class MaterialTextBox : TextBox
    {
        private TextBlock inputLabel;
        private Border inputLine;
        private StackPanel panel;
        private bool previousIsFloating;

        public bool IsFloating
        {
            get { return (bool)GetValue(IsFloatingProperty); }
            set { SetValue(IsFloatingProperty, value); }
        }

        public static readonly DependencyProperty IsFloatingProperty =
            DependencyProperty.Register(nameof(IsFloating), typeof(bool), typeof(MaterialTextBox), new PropertyMetadata(false));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(MaterialTextBox), new PropertyMetadata(string.Empty));

        public Brush Accent
        {
            get { return (Brush)GetValue(AccentProperty); }
            set { SetValue(AccentProperty, value); }
        }

        public static readonly DependencyProperty AccentProperty =
            DependencyProperty.Register(nameof(Accent), typeof(Brush), typeof(MaterialTextBox), new PropertyMetadata(Brushes.Red));

        static MaterialTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialTextBox), new FrameworkPropertyMetadata(typeof(MaterialTextBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.inputLabel = (TextBlock)GetTemplateChild("PART_InputLabel");
            this.inputLine = (Border)GetTemplateChild("PART_InputLine");
            this.panel = (StackPanel)GetTemplateChild("PART_Panel");
            this.inputLabel.Text = this.Label;

            this.panel.Margin = this.IsFloating ? new Thickness(0, 16, 0, 0) : new Thickness(0);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            if (this.IsFloating)
            {
                this.AnimateInputLabel(this.Text.Length > 0);
            }
            else
            {
                this.ClearInputLabel(this.Text.Length > 0);
            }
        }

        protected override void OnIsKeyboardFocusedChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsKeyboardFocusedChanged(e);
            this.AnimateInputLine((bool)e.NewValue);
        }

        private void ClearInputLabel(bool hasText)
        {
            this.inputLabel.Text = hasText ? string.Empty : this.Label;
        }

        private void AnimateInputLabel(bool hasText)
        {
            var duration = new TimeSpan(0, 0, 0, 0, 200);
            this.inputLabel.Foreground = hasText ? this.Accent : Brushes.Black;
            this.inputLabel.Opacity = hasText ? 1.0 : 0.54;

            var enlarge = new DoubleAnimation(10, this.FontSize, duration);
            var reduce = new DoubleAnimation(this.FontSize, 10, duration);

            var moveUp = new ThicknessAnimation(new Thickness(2, 0, 2, 0), new Thickness(2, -16, 2, 16), duration);
            var moveDown = new ThicknessAnimation(new Thickness(2, -16, 2, 16), new Thickness(2, 0, 2, 0), duration);

            if (!previousIsFloating.Equals(hasText))
            {
                previousIsFloating = hasText;
                this.inputLabel.BeginAnimation(FontSizeProperty, hasText ? reduce : enlarge);
                this.inputLabel.BeginAnimation(MarginProperty, hasText ? moveUp : moveDown);
            }
        }

        private void AnimateInputLine(bool show)
        {
            var duration = new TimeSpan(0, 0, 0, 0, 200);
            var enlarge = new DoubleAnimation(0, this.ActualWidth, duration);
            var reduce = new DoubleAnimation(this.ActualWidth, 0, duration);

            this.inputLine.BeginAnimation(WidthProperty, show ? enlarge : reduce);
        }
    }
}
