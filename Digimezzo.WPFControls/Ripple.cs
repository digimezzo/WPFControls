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
        #region Variables
        Ellipse ellipse;
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty RippleBackgroundProperty =
            DependencyProperty.Register("RippleBackground", typeof(Brush), typeof(Ripple), new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty DoRippleProperty =
            DependencyProperty.Register("DoRipple", typeof(bool), typeof(Ripple), new PropertyMetadata(DoRippleChangedHandler));

        public static readonly DependencyProperty DurationMillisecondsProperty =
           DependencyProperty.Register("DurationMilliseconds", typeof(int), typeof(Ripple), new PropertyMetadata(500));
        #endregion

        #region Properties
        public Brush RippleBackground
        {
            get { return (Brush)GetValue(RippleBackgroundProperty); }
            set { SetValue(RippleBackgroundProperty, value); }
        }

        public bool DoRipple
        {
            get { return (bool)GetValue(DoRippleProperty); }
            set { SetValue(DoRippleProperty, value); }
        }

        public int DurationMilliseconds
        {
            get { return (int)GetValue(DurationMillisecondsProperty); }
            set { SetValue(DurationMillisecondsProperty, value); }
        }
        #endregion

        #region Construction
        static Ripple()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Ripple), new FrameworkPropertyMetadata(typeof(Ripple)));
        }
        #endregion

        #region Overrides
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ellipse = GetTemplateChild("PART_ellipse") as Ellipse;
        }
        #endregion

        #region Private
        private static void DoRippleChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                var self = (Ripple)d;

                if (!self.DoRipple) return; // Only ripple if true

                self.MaxWidth = self.ActualWidth; // Make sure the Width cannot expand due to ellipse expand
                self.MaxHeight = self.ActualHeight; // Make sure the Height cannot expand due to ellipse expand

                double targetWidth = Math.Max(self.ActualWidth, self.ActualHeight) * 2;
                Point mousePosition = Mouse.GetPosition(self);
                var startMargin = new Thickness(mousePosition.X, mousePosition.Y, 0, 0);
                int durationOffsetMilliseconds = self.DurationMilliseconds/4;
                var duration = new TimeSpan(0, 0, 0, 0, self.DurationMilliseconds);
                var afterDuration = new TimeSpan(0, 0, 0, 0, self.DurationMilliseconds + durationOffsetMilliseconds);

                // Set initial ellipse Margin to mouse position
                self.ellipse.Margin = startMargin;

                // Animate ellipse Width
                var widthAnimation = new DoubleAnimation(0, targetWidth, duration);

                // Animate ellipse Margin
                var marginAnimation = new ThicknessAnimation(startMargin, new Thickness(mousePosition.X - targetWidth / 2, mousePosition.Y - targetWidth / 2, 0, 0), duration);

                // Animate ellipse Opacity
                var opacityAnimation = new DoubleAnimation(1, 0, afterDuration);

                self.ellipse.BeginAnimation(WidthProperty, widthAnimation);
                self.ellipse.BeginAnimation(MarginProperty, marginAnimation);
                self.ellipse.BeginAnimation(OpacityProperty, opacityAnimation);
            }
            catch (Exception)
            {
            }
        }
        #endregion
    }
}
