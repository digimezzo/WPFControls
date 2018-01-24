using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Digimezzo.WPFControls
{
    public class MaterialComboBox : ComboBox
    {
        private TextBlock inputLabel;
        private bool previousIsFloating;
        private Grid panel;
        private Border inputLine;
        private Border inputLineUnfocused;
        private ToggleButton toggleButton;
        private double opacity = 0.55;
        private bool isFocused;
        private Border dropDownBorder;

        public TextBlock InputLabel => this.inputLabel;

        public bool IsFloating
        {
            get { return (bool)GetValue(IsFloatingProperty); }
            set { SetValue(IsFloatingProperty, value); }
        }

        public static readonly DependencyProperty IsFloatingProperty =
            DependencyProperty.Register(nameof(IsFloating), typeof(bool), typeof(MaterialComboBox), new PropertyMetadata(false));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(MaterialComboBox), new PropertyMetadata(null, new PropertyChangedCallback(OnLabelPropertyChanged)));

        private static void OnLabelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MaterialComboBox box = d as MaterialComboBox;

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
            DependencyProperty.Register(nameof(Accent), typeof(Brush), typeof(MaterialComboBox), new PropertyMetadata(Brushes.Red));

        static MaterialComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialComboBox), new FrameworkPropertyMetadata(typeof(MaterialComboBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.inputLabel = (TextBlock)GetTemplateChild("PART_InputLabel");
            this.toggleButton = (ToggleButton)GetTemplateChild("ToggleButton");
            this.inputLine = (Border)GetTemplateChild("PART_InputLine");
            this.inputLineUnfocused = (Border)GetTemplateChild("PART_InputLineUnfocused");
            this.dropDownBorder = (Border)GetTemplateChild("DropDownBorder");
            this.panel = (Grid)GetTemplateChild("PART_Panel");
            this.toggleButton.Opacity = this.opacity;
            this.inputLineUnfocused.Opacity = this.opacity;
            this.inputLabel.Text = this.Label;
            this.inputLabel.Opacity = this.opacity;
            this.inputLabel.MouseDown += InputLabel_MouseDown;
            this.panel.Margin = this.IsFloating ? new Thickness(0, this.GetSmallFontSize() + this.GetMargin(), 0, 0) : new Thickness(0);
            this.dropDownBorder.Background = this.Background == null ? Brushes.White : this.Background;
        }

        private void InputLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Workaround: InputLabel seems to block mouse click which prevents the drop down to open
            this.IsDropDownOpen = !this.IsDropDownOpen;
        }

        private double GetSmallFontSize()
        {
            return this.FontSize > 14 ? this.FontSize * 0.7 : 10;
        }

        private double GetMargin()
        {
            return this.FontSize * 0.3;
        }

        private void SetInputLabelForeground(bool mustFocus)
        {
            if(this.inputLabel == null)
            {
                return;
            }

            this.inputLabel.Foreground = mustFocus ? this.Accent : this.Foreground;
            this.inputLabel.Opacity = mustFocus ? 1.0 : this.opacity;
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            if (this.IsFloating)
            {
                this.AnimateInputLabel(this.Text.Length > 0);
            }
            else
            {
                this.SetInputLabelText(this.Text.Length > 0);
            }
        }

        private void SetInputLabelText(bool mustClear)
        {
            if (this.inputLabel == null)
            {
                return;
            }

            this.inputLabel.Text = mustClear ? string.Empty : this.Label;
        }

        private void AnimateInputLabel(bool mustFloat)
        {
            if (this.inputLabel == null)
            {
                return;
            }

            var duration = new TimeSpan(0, 0, 0, 0, 200);

            this.SetInputLabelForeground(mustFloat);

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

        protected override void OnDropDownOpened(EventArgs e)
        {
            base.OnDropDownOpened(e);
            this.AnimateInputLine(true);
            this.SetInputLabelForeground(true);
            this.AnimateInputLabel(true);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            if (!this.IsDropDownOpen)
            {
                this.AnimateInputLine(false);
                this.SetInputLabelForeground(false);
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
