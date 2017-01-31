using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Digimezzo.WPFControls
{
    public class Pivot : TabControl
    {
        #region Variables
        private Border contentPanel;
        private int previous = -1;
        private int current = -1;
        #endregion

        #region Properties
        public double SlideDistance
        {
            get { return Convert.ToDouble(GetValue(SlideDistanceProperty)); }
            set { SetValue(SlideDistanceProperty, value); }
        }

        public double SlideDuration
        {
            get { return Convert.ToDouble(GetValue(SlideDurationProperty)); }
            set { SetValue(SlideDurationProperty, value); }
        }

        public double FadeDuration
        {
            get { return Convert.ToDouble(GetValue(FadeDurationProperty)); }
            set { SetValue(FadeDurationProperty, value); }
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty SlideDistanceProperty = DependencyProperty.Register("SlideDistance", typeof(double), typeof(Pivot), new PropertyMetadata(20.0));
        public static readonly DependencyProperty SlideDurationProperty = DependencyProperty.Register("SlideDuration", typeof(double), typeof(Pivot), new PropertyMetadata(0.25));
        public static readonly DependencyProperty FadeDurationProperty = DependencyProperty.Register("FadeDuration", typeof(double), typeof(Pivot), new PropertyMetadata(0.5));
        #endregion

        #region Construction
        static Pivot()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Pivot), new FrameworkPropertyMetadata(typeof(Pivot)));
        }
        #endregion

        #region Overrides
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.contentPanel = (Border)GetTemplateChild("contentPanel");

            if (this.contentPanel != null)
            {
                this.SelectionChanged += Pivot_SelectionChanged;
            }
        }
        #endregion

        #region Event handlers
        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            current = (sender as TabControl).SelectedIndex;
            if (previous != current)
            {
                DoubleAnimation slideAnimation = null;
                DoubleAnimation opacityAnimation = new DoubleAnimation() { From = 0.0, To = 1.0, Duration = TimeSpan.FromSeconds(this.FadeDuration) };

                if (previous > current)
                {
                    slideAnimation = new DoubleAnimation() { From = -this.SlideDistance, To = 0.0, Duration = TimeSpan.FromSeconds(this.SlideDuration) };

                }
                else
                {
                    slideAnimation = new DoubleAnimation() { From = this.SlideDistance, To = 0.0, Duration = TimeSpan.FromSeconds(this.SlideDuration) };
                }

                TranslateTransform translateTransform1 = new TranslateTransform();
                var fadeStoryboard = new Storyboard();
                fadeStoryboard.Children.Add(opacityAnimation);
                Storyboard.SetTargetName(contentPanel, contentPanel.Name);
                Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UserControl.OpacityProperty));
                fadeStoryboard.Begin(contentPanel);
                translateTransform1.BeginAnimation(TranslateTransform.XProperty, slideAnimation);
                contentPanel.RenderTransform = translateTransform1;
                previous = current;
            }
        }
        #endregion
    }

    public class PivotItem : TabItem
    {
        #region Properties
        public Brush SelectedForeground
        {
            get { return (Brush)GetValue(SelectedForegroundProperty); }
            set { SetValue(SelectedForegroundProperty, value); }
        }

        public FontWeight SelectedFontWeight
        {
            get { return (FontWeight)GetValue(SelectedFontWeightProperty); }
            set { SetValue(SelectedFontWeightProperty, value); }
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty SelectedForegroundProperty = DependencyProperty.Register("SelectedForeground", typeof(Brush), typeof(PivotItem), new PropertyMetadata(Brushes.Black));
        public static readonly DependencyProperty SelectedFontWeightProperty = DependencyProperty.Register("SelectedFontWeight", typeof(FontWeight), typeof(PivotItem), new PropertyMetadata(FontWeights.Normal));
        #endregion

        #region Construction
        static PivotItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PivotItem), new FrameworkPropertyMetadata(typeof(PivotItem)));
        }
        #endregion
    }
}
