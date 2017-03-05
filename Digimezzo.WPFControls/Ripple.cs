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
        Grid grid;
        Storyboard animation;
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty RippleBackgroundProperty =
            DependencyProperty.Register("RippleBackground", typeof(Brush), typeof(Ripple), new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty DoRippleProperty =
            DependencyProperty.Register("DoRipple", typeof(bool), typeof(Ripple), new PropertyMetadata(DoRippleChangedHandler));
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
            grid = GetTemplateChild("PART_grid") as Grid;
            animation = grid.FindResource("PART_animation") as Storyboard;
        }
        #endregion

        #region Private
        private static void DoRippleChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                var self = (Ripple)d;

                if (!self.DoRipple) return; // Only ripple if true

                var targetWidth = Math.Max(self.ActualWidth, self.ActualHeight) * 2;
                var mousePosition = Mouse.GetPosition(self);
                var startMargin = new Thickness(mousePosition.X, mousePosition.Y, 0, 0);

                // Set initial margin to mouse position
                self.ellipse.Margin = startMargin;

                // Set the "To" value of the animation that animates the width to the target width
                (self.animation.Children[0] as DoubleAnimation).To = targetWidth;

                // Set the "To" and "From" values of the animation that animates the distance relative to the container (grid)
                (self.animation.Children[1] as ThicknessAnimation).From = startMargin;
                (self.animation.Children[1] as ThicknessAnimation).To = new Thickness(mousePosition.X - targetWidth / 2, mousePosition.Y - targetWidth / 2, 0, 0);
                self.ellipse.BeginStoryboard(self.animation);
            }
            catch (Exception)
            {
            }
        }
        #endregion
    }
}
