using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Digimezzo.WPFControls
{
    public abstract class Windows8Slider : Control
    {
        #region Variables
        protected Canvas sliderCanvas;
        private Rectangle sliderTrack;
        private Rectangle sliderBar;
        private Button sliderButton;
        protected bool isCalculating;
        protected bool isDragging;
        #endregion

        #region Properties
        public bool ChangeValueWhileDragging
        {
            get { return Convert.ToBoolean(GetValue(ChangeValueWhileDraggingProperty)); }
            set { SetValue(ChangeValueWhileDraggingProperty, value); }
        }

        public double Maximum
        {
            get { return Convert.ToDouble(GetValue(MaximumProperty)); }
            set { SetValue(MaximumProperty, value); }
        }

        public double Value
        {
            get { return Convert.ToDouble(GetValue(ValueProperty)); }
            set { SetValue(ValueProperty, value); }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public double Position
        {
            get { return Convert.ToDouble(GetValue(PositionProperty)); }
            set { SetValue(PositionProperty, value); }
        }

        public Brush TrackBackground
        {
            get { return (Brush)GetValue(TrackBackgroundProperty); }
            set { SetValue(TrackBackgroundProperty, value); }
        }

        public Brush BarBackground
        {
            get { return (Brush)GetValue(BarBackgroundProperty); }
            set { SetValue(BarBackgroundProperty, value); }
        }

        public Brush ButtonBackground
        {
            get { return (Brush)GetValue(ButtonBackgroundProperty); }
            set { SetValue(ButtonBackgroundProperty, value); }
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty ChangeValueWhileDraggingProperty = DependencyProperty.Register("ChangeValueWhileDragging", typeof(bool), typeof(Windows8Slider), new PropertyMetadata(false));
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(Windows8Slider), new PropertyMetadata(100.0, IsMaximumChangedCallback));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(Windows8Slider), new PropertyMetadata(0.0, IsValueChangedCallback));
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(double), typeof(Windows8Slider), new PropertyMetadata(0.0));
        public static readonly DependencyProperty TrackBackgroundProperty = DependencyProperty.Register("TrackBackground", typeof(Brush), typeof(Windows8Slider), new PropertyMetadata(null));
        public static readonly DependencyProperty BarBackgroundProperty = DependencyProperty.Register("BarBackground", typeof(Brush), typeof(Windows8Slider), new PropertyMetadata(null));
        public static readonly DependencyProperty ButtonBackgroundProperty = DependencyProperty.Register("ButtonBackground", typeof(Brush), typeof(Windows8Slider), new PropertyMetadata(null));
        #endregion

        #region Events
        public event EventHandler ValueChanged = delegate { };

        protected void OnValueChanged()
        {
            this.ValueChanged(this, null);
        }
        #endregion

        #region Overrides
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.sliderCanvas = (Canvas)GetTemplateChild("PART_Canvas");
            this.sliderTrack = (Rectangle)GetTemplateChild("PART_Track");
            this.sliderBar = (Rectangle)GetTemplateChild("PART_Bar");
            this.sliderButton = (Button)GetTemplateChild("PART_Button");

            this.SizeChanged += SizeChangedHandler;

            if (this.sliderButton != null)
            {
                this.sliderButton.PreviewMouseDown += SliderButton_PreviewMouseDown;
                this.sliderButton.PreviewMouseUp += SliderButton_PreviewMouseUp;
                this.sliderButton.PreviewMouseMove += SliderButton_PreviewMouseMove;
            }

            if (this.sliderCanvas != null)
            {
                this.sliderCanvas.PreviewMouseUp += SliderCanvas_PreviewMouseUp;
            }
        }
        #endregion

        #region Abstract
        protected abstract void UpdatePosition();
        protected abstract void CalculatePosition();
        protected abstract void CalculateValue();
        #endregion

        #region Event Handlers
        private void SizeChangedHandler(object sender, SizeChangedEventArgs e)
        {
            this.CalculatePosition();
        }

        private void SliderButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.isDragging = true;
        }

        private void SliderButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.isDragging = false;
            this.CalculateValue();
        }

        private void SliderButton_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (this.isDragging)
            {
                this.UpdatePosition();

                if (this.ChangeValueWhileDragging)
                {
                    this.CalculateValue();
                }
            }
        }

        private void SliderCanvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!this.isDragging)
            {
                this.UpdatePosition();
                this.CalculateValue();
            }
        }
        #endregion

        #region Callbacks
        private static void IsMaximumChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Windows8Slider slider = sender as Windows8Slider;
            slider.CalculatePosition();
        }

        private static void IsValueChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Windows8Slider slider = sender as Windows8Slider;
            slider.CalculatePosition();
            slider.OnValueChanged();
        }
        #endregion
    }

    public class HorizontalWindows8Slider : Windows8Slider
    {

        #region Construction
        static HorizontalWindows8Slider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HorizontalWindows8Slider), new FrameworkPropertyMetadata(typeof(HorizontalWindows8Slider)));
        }
        #endregion

        #region Overrides
        protected override void UpdatePosition()
        {
            this.Position = Mouse.GetPosition(this.sliderCanvas).X;

            if (this.Position > this.sliderCanvas.ActualWidth)
            {
                this.Position = this.sliderCanvas.ActualWidth;
            }

            if (this.Position < 0.0)
            {
                this.Position = 0.0;
            }

        }

        protected override void CalculatePosition()
        {
            if (!this.isCalculating)
            {
                this.isCalculating = true;

                if (this.Maximum > 0 && this.ActualWidth > 0)
                {
                    this.Position = (this.Value / this.Maximum) * this.ActualWidth;
                }
                else
                {
                    this.Position = 0;
                }

                this.isCalculating = false;
            }
        }

        protected override void CalculateValue()
        {
            if (!this.isCalculating)
            {
                this.isCalculating = true;

                if (this.Maximum > 0 && this.ActualWidth > 0)
                {
                    this.Value = (this.Position * this.Maximum) / this.ActualWidth;
                }
                else
                {
                    this.Value = 0;
                }

                this.isCalculating = false;
            }

        }
        #endregion
    }

    public class VerticalWindows8Slider : Windows8Slider
    {
        #region Construction
        static VerticalWindows8Slider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VerticalWindows8Slider), new FrameworkPropertyMetadata(typeof(VerticalWindows8Slider)));
        }
        #endregion

        #region Overrides
        protected override void UpdatePosition()
        {
            this.Position = this.sliderCanvas.ActualHeight - Mouse.GetPosition(this.sliderCanvas).Y;

            if (this.Position > this.sliderCanvas.ActualHeight)
            {
                this.Position = this.sliderCanvas.ActualHeight;
            }

            if (this.Position < 0.0)
            {
                this.Position = 0.0;
            }

        }

        protected override void CalculatePosition()
        {
            if (!this.isCalculating)
            {
                this.isCalculating = true;

                if (this.Maximum > 0 && this.ActualHeight > 0)
                {
                    this.Position = (this.Value / this.Maximum) * this.ActualHeight;
                }
                else
                {
                    this.Position = 0;
                }

                this.isCalculating = false;
            }
        }

        protected override void CalculateValue()
        {
            if (!this.isCalculating)
            {
                this.isCalculating = true;

                if (this.ActualHeight > 0)
                {
                    this.Value = (this.Position * this.Maximum) / this.ActualHeight;
                }
                else
                {
                    this.Value = 0;
                }

                this.isCalculating = false;
            }
        }
        #endregion
    }
}