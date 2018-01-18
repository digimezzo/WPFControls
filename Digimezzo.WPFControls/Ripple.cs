using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Digimezzo.WPFControls
{
    public class Ripple : ContentControl
    {
        Ellipse ellipse;

        public Brush RippleBackground
        {
            get { return (Brush)GetValue(RippleBackgroundProperty); }
            set { SetValue(RippleBackgroundProperty, value); }
        }

        public static readonly DependencyProperty RippleBackgroundProperty =
            DependencyProperty.Register(nameof(RippleBackground), typeof(Brush), typeof(Ripple), new PropertyMetadata(Brushes.White));

        public int DurationMilliseconds
        {
            get { return (int)GetValue(DurationMillisecondsProperty); }
            set { SetValue(DurationMillisecondsProperty, value); }
        }

        public static readonly DependencyProperty DurationMillisecondsProperty =
           DependencyProperty.Register(nameof(DurationMilliseconds), typeof(int), typeof(Ripple), new PropertyMetadata(500));

        public bool StartAtPointer
        {
            get { return (bool)GetValue(StartAtPointerProperty); }
            set { SetValue(StartAtPointerProperty, value); }
        }

        public static readonly DependencyProperty StartAtPointerProperty =
            DependencyProperty.Register(nameof(StartAtPointer), typeof(bool), typeof(Ripple), new PropertyMetadata(true));

        public double Scale
        {
            get { return (double)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }

        public static readonly DependencyProperty ScaleProperty =
           DependencyProperty.Register(nameof(Scale), typeof(double), typeof(Ripple), new PropertyMetadata(2.0));

        public bool DoRippleOnMouseClick
        {
            get { return (bool)GetValue(DoRippleOnMouseClickProperty); }
            set { SetValue(DoRippleOnMouseClickProperty, value); }
        }

        public static readonly DependencyProperty DoRippleOnMouseClickProperty =
            DependencyProperty.Register(nameof(DoRippleOnMouseClick), typeof(bool), typeof(Ripple), new PropertyMetadata(true));

        static Ripple()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Ripple), new FrameworkPropertyMetadata(typeof(Ripple)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ellipse = GetTemplateChild("PART_ellipse") as Ellipse;
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.VerticalAlignment = VerticalAlignment.Stretch;
        }

        public void DoRippple()
        {
            this.MaxWidth = this.ActualWidth; // Make sure the Width cannot expand due to ellipse expand
            this.MaxHeight = this.ActualHeight; // Make sure the Height cannot expand due to ellipse expand

            double targetWidth = Math.Max(this.ActualWidth, this.ActualHeight) * this.Scale;

            Point mousePosition = Mouse.GetPosition(this);

            Thickness startMargin = new Thickness(this.ActualWidth / 2, this.ActualHeight / 2, 0, 0);

            if (this.StartAtPointer)
            {
                startMargin = new Thickness(mousePosition.X, mousePosition.Y, 0, 0);
            }

            int durationOffsetMilliseconds = this.DurationMilliseconds / 4;
            var duration = new TimeSpan(0, 0, 0, 0, this.DurationMilliseconds);
            var afterDuration = new TimeSpan(0, 0, 0, 0, this.DurationMilliseconds + durationOffsetMilliseconds);

            // Set initial ellipse Margin to mouse position
            this.ellipse.Margin = startMargin;

            // Animate ellipse Width
            var widthAnimation = new DoubleAnimation(0, targetWidth, duration);

            // Animate ellipse Margin
            var targetMargin = new Thickness((this.ActualWidth - targetWidth) / 2);

            if (this.StartAtPointer)
            {
                targetMargin = new Thickness(
                    mousePosition.X - targetWidth / 2, 
                    mousePosition.Y - targetWidth / 2,
                    mousePosition.X - targetWidth / 2,
                    mousePosition.Y - targetWidth / 2);
            }

            var marginAnimation = new ThicknessAnimation(startMargin, targetMargin, duration);

            // Animate ellipse Opacity
            var opacityAnimation = new DoubleAnimation(1, 0, afterDuration);

            this.ellipse.BeginAnimation(WidthProperty, widthAnimation);
            this.ellipse.BeginAnimation(MarginProperty, marginAnimation);
            this.ellipse.BeginAnimation(OpacityProperty, opacityAnimation);
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            if (this.DoRippleOnMouseClick)
            {
                this.DoRippple();
            }
        }
    }
}
