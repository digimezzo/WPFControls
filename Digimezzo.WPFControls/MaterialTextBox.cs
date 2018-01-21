using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Digimezzo.WPFControls
{
    public enum ValidationMode{
        None = 0,
        Number = 1,
        Text = 2
    }

    public class MaterialTextBox : TextBox
    {
        private TextBlock inputLabel;
        private TextBlock errorLabel;
        private bool previousIsFloating;
        private Border inputLine;
        private Border inputLineUnfocused;
        private StackPanel panel;
        private double opacity = 0.55;
        private bool isFocused;

        public ValidationMode ValidationMode
        {
            get { return (ValidationMode)GetValue(ValidationModeProperty); }
            set { SetValue(ValidationModeProperty, value); }
        }

        public static readonly DependencyProperty ValidationModeProperty =
            DependencyProperty.Register(nameof(ValidationMode), typeof(ValidationMode), typeof(MaterialTextBox), new PropertyMetadata(ValidationMode.None));

        public bool IsFloating
        {
            get { return (bool)GetValue(IsFloatingProperty); }
            set { SetValue(IsFloatingProperty, value); }
        }

        public static readonly DependencyProperty IsFloatingProperty =
            DependencyProperty.Register(nameof(IsFloating), typeof(bool), typeof(MaterialTextBox), new PropertyMetadata(false));

        public bool UseLightCursor
        {
            get { return (bool)GetValue(UseLightCursorProperty); }
            set { SetValue(UseLightCursorProperty, value); }
        }

        public static readonly DependencyProperty UseLightCursorProperty =
            DependencyProperty.Register(nameof(UseLightCursor), typeof(bool), typeof(MaterialTextBox), new PropertyMetadata(false));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(MaterialTextBox), new PropertyMetadata(null, new PropertyChangedCallback(OnLabelPropertyChanged)));

        private static void OnLabelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MaterialTextBox box = d as MaterialTextBox;

            if (box != null && box.inputLabel != null)
            {
                box.inputLabel.Text = box.Label;
            }
        }

        public Brush Accent
        {
            get { return (Brush)GetValue(AccentProperty); }
            set { SetValue(AccentProperty, value); }
        }

        public static readonly DependencyProperty AccentProperty =
            DependencyProperty.Register(nameof(Accent), typeof(Brush), typeof(MaterialTextBox), new PropertyMetadata(Brushes.Blue));

        public Brush ErrorForeground
        {
            get { return (Brush)GetValue(ErrorForegroundProperty); }
            set { SetValue(ErrorForegroundProperty, value); }
        }

        public static readonly DependencyProperty ErrorForegroundProperty =
            DependencyProperty.Register(nameof(ErrorForeground), typeof(Brush), typeof(MaterialTextBox), new PropertyMetadata(Brushes.Red));

        public string ErrorText
        {
            get { return (string)GetValue(ErrorTextProperty); }
            set { SetValue(ErrorTextProperty, value); }
        }

        public static readonly DependencyProperty ErrorTextProperty =
            DependencyProperty.Register(nameof(ErrorText), typeof(string), typeof(MaterialTextBox), new PropertyMetadata("Invalid"));

        static MaterialTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialTextBox), new FrameworkPropertyMetadata(typeof(MaterialTextBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.inputLabel = (TextBlock)GetTemplateChild("PART_InputLabel");
            this.errorLabel = (TextBlock)GetTemplateChild("PART_ErrorLabel");
            this.inputLine = (Border)GetTemplateChild("PART_InputLine");
            this.inputLineUnfocused = (Border)GetTemplateChild("PART_InputLineUnfocused");
            this.panel = (StackPanel)GetTemplateChild("PART_Panel");
            this.inputLabel.Text = this.Label;
            this.inputLabel.Opacity = this.opacity;
            this.inputLineUnfocused.Opacity = this.opacity;

            this.errorLabel.FontSize = this.GetSmallFontSize();
            this.errorLabel.Margin = this.ValidationMode.Equals(ValidationMode.None) ? new Thickness(0) : new Thickness(0, this.GetMargin(), 0, 0);

            this.panel.Margin = this.IsFloating ? new Thickness(0, this.GetSmallFontSize() + this.GetMargin(), 0, 0) : new Thickness(0);

            this.SelectionBrush = this.Accent; // Binding SelectionBrush in the xaml template doesn't work, so we set it here.

            // This workaround changes the color of the cursor
            if (this.UseLightCursor)
            {
                this.Background = new SolidColorBrush(Colors.Black); 
            }
            else {
                this.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void Validate()
        {
            switch (this.ValidationMode)
            {
                case ValidationMode.Number:
                    this.ValidateNumber();
                    break;
                case ValidationMode.Text:
                    this.ValidateText();
                    break;
                case ValidationMode.None:
                default:
                    break;
            }
        }

        private void ValidateText()
        {
            if (string.IsNullOrWhiteSpace(this.Text))
            {
                this.errorLabel.Text = this.ErrorText;
            }
            else
            {
                this.errorLabel.Text = String.Empty;
            }
        }

        private void ValidateNumber()
        {
            bool isNumberValid = false;

            if (string.IsNullOrEmpty(this.Text))
            {
                isNumberValid = false;
            }
            else
            {
                double number = 0;
                isNumberValid = double.TryParse(this.Text, out number);
            }

            if (isNumberValid)
            {
                this.errorLabel.Text = String.Empty;
            }
            else
            {
                this.errorLabel.Text = this.ErrorText;
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            if (this.IsFloating)
            {
                this.AnimateInputLabel(isFocused | this.Text.Length > 0);
            }
            else
            {
                this.SetInputLabelText(this.Text.Length > 0);
            }

            this.Validate();
        }

        protected override void OnIsKeyboardFocusedChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsKeyboardFocusedChanged(e);
            bool isFocused = (bool)e.NewValue;
            this.AnimateInputLine(isFocused);
            this.SetInputLabelForeground(isFocused);
            this.AnimateInputLabel(isFocused | !isFocused & this.Text.Length > 0);
        }

        private void SetInputLabelText(bool mustClear)
        {
            if (this.inputLabel == null)
            {
                return;
            }

            this.inputLabel.Text = mustClear ? string.Empty : this.Label;
        }

        private void SetInputLabelForeground(bool mustFocus)
        {
            if (this.inputLabel == null)
            {
                return;
            }

            this.inputLabel.Foreground = mustFocus ? this.Accent : this.Foreground;
            this.inputLabel.Opacity = mustFocus ? 1.0 : this.opacity;
        }

        private double GetSmallFontSize()
        {
            return this.FontSize > 14 ? this.FontSize * 0.7 : 10;
        }

        private double GetMargin()
        {
            return this.FontSize * 0.3;
        }

        private void AnimateInputLabel(bool mustFloat)
        {
            if(this.inputLabel == null)
            {
                return;
            }

            var duration = new TimeSpan(0, 0, 0, 0, 200);

            double smallFontSize = 0;
            double margin = 2;

            if (this.FontSize != double.NaN)
            {
                smallFontSize = this.GetSmallFontSize();
                margin = this.GetMargin();
            }

            double offset = smallFontSize + margin;

            var enlarge = new DoubleAnimation(smallFontSize, this.FontSize, duration);
            var reduce = new DoubleAnimation(this.FontSize, smallFontSize, duration);

            var moveUp = new ThicknessAnimation(new Thickness(2, 0, 2, 0), new Thickness(2, -offset, 2, offset), duration);
            var moveDown = new ThicknessAnimation(new Thickness(2, -offset, 2, offset), new Thickness(2, 0, 2, 0), duration);

            if (!previousIsFloating.Equals(mustFloat))
            {
                previousIsFloating = mustFloat;
                this.inputLabel.BeginAnimation(FontSizeProperty, mustFloat ? reduce : enlarge);
                this.inputLabel.BeginAnimation(MarginProperty, mustFloat ? moveUp : moveDown);
            }
        }

        private void AnimateInputLine(bool mustFocus)
        {
            if (this.inputLine == null)
            {
                return;
            }

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
