using Digimezzo.WPFControls.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Digimezzo.WPFControls
{
    public enum PivotAnimationType
    {
        Fade = 1,
        Slide = 2
    }

    public class Pivot : TabControl
    {
        private Grid contentPanel;
        private ContentPresenter mainContent;
        private Shape paintArea;
        private int previous = -1;
        private int current = -1;

        public PivotAnimationType AnimationType
        {
            get { return (PivotAnimationType)GetValue(animationTypeProperty); }
            set { SetValue(animationTypeProperty, value); }
        }

        public static readonly DependencyProperty animationTypeProperty =
            DependencyProperty.Register(nameof(AnimationType), typeof(PivotAnimationType), typeof(Pivot), new PropertyMetadata(PivotAnimationType.Fade));

        public double Elevation
        {
            get { return Convert.ToDouble(GetValue(ElevationProperty)); }
            set { SetValue(ElevationProperty, value); }
        }

        public static readonly DependencyProperty ElevationProperty =
          DependencyProperty.Register(nameof(Elevation), typeof(double), typeof(Pivot), new PropertyMetadata(0.0));

        public Brush ElevationBackground
        {
            get { return (Brush)GetValue(ElevationBackgroundProperty); }
            set { SetValue(ElevationBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ElevationBackgroundProperty =
           DependencyProperty.Register(nameof(ElevationBackground), typeof(Brush), typeof(Pivot), new PropertyMetadata(Brushes.Transparent));

        public Brush HeaderBackground
        {
            get { return (Brush)GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }

        public static readonly DependencyProperty HeaderBackgroundProperty =
         DependencyProperty.Register(nameof(HeaderBackground), typeof(Brush), typeof(Pivot), new PropertyMetadata(Brushes.Transparent));

        public double SlideDistance
        {
            get { return Convert.ToDouble(GetValue(SlideDistanceProperty)); }
            set { SetValue(SlideDistanceProperty, value); }
        }

        public static readonly DependencyProperty SlideDistanceProperty =
         DependencyProperty.Register(nameof(SlideDistance), typeof(double), typeof(Pivot), new PropertyMetadata(20.0));

        public double SlideDuration
        {
            get { return Convert.ToDouble(GetValue(SlideDurationProperty)); }
            set { SetValue(SlideDurationProperty, value); }
        }

        public static readonly DependencyProperty SlideDurationProperty =
           DependencyProperty.Register(nameof(SlideDuration), typeof(double), typeof(Pivot), new PropertyMetadata(0.5));

        public double EasingAmplitude
        {
            get { return Convert.ToDouble(GetValue(EasingAmplitudeProperty)); }
            set { SetValue(EasingAmplitudeProperty, value); }
        }

        public static readonly DependencyProperty EasingAmplitudeProperty = 
            DependencyProperty.Register(nameof(EasingAmplitude), typeof(double), typeof(Pivot), new PropertyMetadata(0.0));

        public double FadeDuration
        {
            get { return Convert.ToDouble(GetValue(FadeDurationProperty)); }
            set { SetValue(FadeDurationProperty, value); }
        }

        public static readonly DependencyProperty FadeDurationProperty =
           DependencyProperty.Register(nameof(FadeDuration), typeof(double), typeof(Pivot), new PropertyMetadata(0.5));

        public double IndicatorHeight
        {
            get { return (double)GetValue(IndicatorHeightProperty); }
            set { SetValue(IndicatorHeightProperty, value); }
        }

        public static readonly DependencyProperty IndicatorHeightProperty =
          DependencyProperty.Register(nameof(IndicatorHeight), typeof(double), typeof(Pivot), new PropertyMetadata(0.0));

        public Brush IndicatorBackground
        {
            get { return (Brush)GetValue(IndicatorBackgroundProperty); }
            set { SetValue(IndicatorBackgroundProperty, value); }
        }

        public static readonly DependencyProperty IndicatorBackgroundProperty =
          DependencyProperty.Register(nameof(IndicatorBackground), typeof(Brush), typeof(Pivot), new PropertyMetadata(Brushes.Transparent));

        public bool DisableTabKey
        {
            get { return (bool)GetValue(DisableTabKeyProperty); }
            set { SetValue(DisableTabKeyProperty, value); }
        }

        public static readonly DependencyProperty DisableTabKeyProperty =
          DependencyProperty.Register(nameof(DisableTabKey), typeof(bool), typeof(Pivot), new PropertyMetadata(false));

        static Pivot()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Pivot), new FrameworkPropertyMetadata(typeof(Pivot)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.contentPanel = (Grid)GetTemplateChild("contentPanel");
            this.mainContent = (ContentPresenter)GetTemplateChild("PART_SelectedContentHost");
            this.paintArea = (Shape)GetTemplateChild("PART_PaintArea");

            if (this.contentPanel != null)
            {
                this.SelectionChanged += Pivot_SelectionChanged;
            }

            if (this.DisableTabKey)
            {
                this.PreviewKeyDown += Pivot_PreviewKeyDown;
            }
        }

        private void Pivot_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                e.Handled = true;
            }
        }

        private void DoFadeAnimation()
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

        private void DoSlideAnimation()
        {
            try
            {
                if (this.paintArea != null && this.mainContent != null)
                {
                    this.paintArea.Fill = AnimationUtils.CreateBrushFromVisual(this.mainContent, this.ActualWidth, this.ActualHeight);

                    var newContentTransform = new TranslateTransform();
                    var oldContentTransform = new TranslateTransform();
                    this.paintArea.RenderTransform = oldContentTransform;
                    this.mainContent.RenderTransform = newContentTransform;
                    this.paintArea.Visibility = Visibility.Visible;

                    if (previous > current)
                    {
                        newContentTransform.BeginAnimation(TranslateTransform.XProperty, AnimationUtils.CreateSlideAnimation(-this.ActualWidth, 0, this.EasingAmplitude, this.SlideDuration));
                        oldContentTransform.BeginAnimation(TranslateTransform.XProperty, AnimationUtils.CreateSlideAnimation(0, this.ActualWidth, this.EasingAmplitude, this.SlideDuration, (s, e) => this.paintArea.Visibility = Visibility.Hidden));

                    }
                    else
                    {
                        newContentTransform.BeginAnimation(TranslateTransform.XProperty, AnimationUtils.CreateSlideAnimation(this.ActualWidth, 0, this.EasingAmplitude, this.SlideDuration));
                        oldContentTransform.BeginAnimation(TranslateTransform.XProperty, AnimationUtils.CreateSlideAnimation(0, -this.ActualWidth, this.EasingAmplitude, this.SlideDuration, (s, e) => this.paintArea.Visibility = Visibility.Hidden));
                    }

                    previous = current;
                }
            }
            catch (Exception)
            {
            }
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            current = (sender as TabControl).SelectedIndex;

            if (previous != current)
            {

                if (this.AnimationType == PivotAnimationType.Fade)
                {
                    this.DoFadeAnimation();
                }
                else if (this.AnimationType == PivotAnimationType.Slide)
                {
                    this.DoSlideAnimation();
                }
            }
        }
    }

    public class PivotItem : TabItem
    {
        public double HeaderFontSize
        {
            get { return (double)GetValue(HeaderFontSizeProperty); }
            set { SetValue(HeaderFontSizeProperty, value); }
        }

        public static readonly DependencyProperty HeaderFontSizeProperty =
           DependencyProperty.Register(nameof(HeaderFontSize), typeof(double), typeof(PivotItem), new PropertyMetadata(13.0));

        public Brush SelectedForeground
        {
            get { return (Brush)GetValue(SelectedForegroundProperty); }
            set { SetValue(SelectedForegroundProperty, value); }
        }

        public static readonly DependencyProperty SelectedForegroundProperty =
           DependencyProperty.Register(nameof(SelectedForeground), typeof(Brush), typeof(PivotItem), new PropertyMetadata(Brushes.Black));

        public FontWeight SelectedFontWeight
        {
            get { return (FontWeight)GetValue(SelectedFontWeightProperty); }
            set { SetValue(SelectedFontWeightProperty, value); }
        }

        public static readonly DependencyProperty SelectedFontWeightProperty =
             DependencyProperty.Register(nameof(SelectedFontWeight), typeof(FontWeight), typeof(PivotItem), new PropertyMetadata(FontWeights.Normal));

        static PivotItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PivotItem), new FrameworkPropertyMetadata(typeof(PivotItem)));
        }
    }
}
