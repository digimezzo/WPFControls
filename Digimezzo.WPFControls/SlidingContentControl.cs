using Digimezzo.WPFControls.Effects;
using Digimezzo.WPFControls.Enums;
using Digimezzo.WPFControls.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Digimezzo.WPFControls
{
    public class SlidingContentControl : ContentControl
    {
        private ContentPresenter mainContent;
        private Shape paintArea;
        private FeatheringEffect effect;

        public SlideDirection SlideDirection
        {
            get { return (SlideDirection)GetValue(SlideDirectionProperty); }
            set { SetValue(SlideDirectionProperty, value); }
        }

        public static readonly DependencyProperty SlideDirectionProperty =
            DependencyProperty.Register(nameof(SlideDirection), typeof(SlideDirection), typeof(SlidingContentControl), new PropertyMetadata(Enums.SlideDirection.RightToLeft));

        public double EasingAmplitude
        {
            get { return Convert.ToDouble(GetValue(EasingAmplitudeProperty)); }
            set { SetValue(EasingAmplitudeProperty, value); }
        }

        public static readonly DependencyProperty EasingAmplitudeProperty =
            DependencyProperty.Register(nameof(EasingAmplitude), typeof(double), typeof(SlidingContentControl), new PropertyMetadata(0.0));

        public double Duration
        {
            get { return Convert.ToDouble(GetValue(DurationProperty)); }
            set { SetValue(DurationProperty, value); }
        }

        public static readonly DependencyProperty DurationProperty =
           DependencyProperty.Register(nameof(Duration), typeof(double), typeof(SlidingContentControl), new PropertyMetadata(0.5));

        public double FeatheringRadius
        {
            get => (double)GetValue(FeatheringRadiusProperty);
            set => SetValue(FeatheringRadiusProperty, value);
        }

        public static DependencyProperty FeatheringRadiusProperty =
            DependencyProperty.Register(nameof(FeatheringRadius), typeof(double), typeof(SlidingContentControl), new PropertyMetadata(0.0));

        public bool SlideFadeIn
        {
            get { return (bool)GetValue(SlideFadeInProperty); }
            set { SetValue(SlideFadeInProperty, value); }
        }

        public static readonly DependencyProperty SlideFadeInProperty =
            DependencyProperty.Register(nameof(SlideFadeIn), typeof(bool), typeof(SlidingContentControl), new PropertyMetadata(false));

        public double SlideFadeInDuration
        {
            get { return Convert.ToDouble(GetValue(SlideFadeInDurationProperty)); }
            set { SetValue(SlideFadeInDurationProperty, value); }
        }

        public static readonly DependencyProperty SlideFadeInDurationProperty =
          DependencyProperty.Register(nameof(SlideFadeInDuration), typeof(double), typeof(SlidingContentControl), new PropertyMetadata(0.5));

        static SlidingContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SlidingContentControl), new FrameworkPropertyMetadata(typeof(SlidingContentControl)));
        }

        public override void OnApplyTemplate()
        {
            this.mainContent = (ContentPresenter)GetTemplateChild("PART_MainContent");
            this.paintArea = (Shape)GetTemplateChild("PART_PaintArea");
            this.effect = new FeatheringEffect() { FeatheringRadius = this.FeatheringRadius };

            base.OnApplyTemplate();
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            this.DoSlideAnimation();
            base.OnContentChanged(oldContent, newContent);
        }

        private void ApplyEffect()
        {
            if (this.FeatheringRadius > 0.0)
            {
                // Only apply the effect when FeatheringRadius is specified.
                this.effect.TexWidth = ActualWidth;
                this.Effect = effect;
            }
        }

        private void ClearEffect()
        {
            this.Effect = null;
        }

        private void DoSlideAnimation()
        {
            if (this.paintArea != null && this.mainContent != null && this.ActualWidth > 0 &&  this.ActualHeight > 0)
            {
                this.paintArea.Fill = AnimationUtils.CreateBrushFromVisual(this.mainContent, this.ActualWidth, this.ActualHeight);

                var newContentTransform = new TranslateTransform();
                var oldContentTransform = new TranslateTransform();
                this.paintArea.RenderTransform = oldContentTransform;
                this.mainContent.RenderTransform = newContentTransform;
                this.paintArea.Visibility = Visibility.Visible;

                this.ApplyEffect();

                switch (this.SlideDirection)
                {
                    case SlideDirection.LeftToRight:
                        newContentTransform.BeginAnimation(TranslateTransform.XProperty, AnimationUtils.CreateSlideAnimation(-this.ActualWidth, 0, this.EasingAmplitude, this.Duration));
                        oldContentTransform.BeginAnimation(TranslateTransform.XProperty, AnimationUtils.CreateSlideAnimation(0, this.ActualWidth, this.EasingAmplitude, this.Duration,
                            (s, e) =>
                            {
                                this.paintArea.Visibility = Visibility.Hidden;
                                this.ClearEffect();
                            }));
                        break;
                    case SlideDirection.RightToLeft:
                        newContentTransform.BeginAnimation(TranslateTransform.XProperty, AnimationUtils.CreateSlideAnimation(this.ActualWidth, 0, this.EasingAmplitude, this.Duration));
                        oldContentTransform.BeginAnimation(TranslateTransform.XProperty, AnimationUtils.CreateSlideAnimation(0, -this.ActualWidth, this.EasingAmplitude, this.Duration,
                           (s, e) =>
                           {
                               this.paintArea.Visibility = Visibility.Hidden;
                               this.ClearEffect();
                           }));
                        break;
                    case SlideDirection.UpToDown:
                        newContentTransform.BeginAnimation(TranslateTransform.YProperty, AnimationUtils.CreateSlideAnimation(-this.ActualHeight, 0, this.EasingAmplitude, this.Duration));
                        oldContentTransform.BeginAnimation(TranslateTransform.YProperty, AnimationUtils.CreateSlideAnimation(0, this.ActualHeight, this.EasingAmplitude, this.Duration,
                            (s, e) =>
                            {
                                this.paintArea.Visibility = Visibility.Hidden;
                                this.ClearEffect();
                            }));
                        break;
                    case SlideDirection.DownToUp:
                        newContentTransform.BeginAnimation(TranslateTransform.YProperty, AnimationUtils.CreateSlideAnimation(this.ActualHeight, 0, this.EasingAmplitude, this.Duration));
                        oldContentTransform.BeginAnimation(TranslateTransform.YProperty, AnimationUtils.CreateSlideAnimation(0, -this.ActualHeight, this.EasingAmplitude, this.Duration,
                            (s, e) =>
                            {
                                this.paintArea.Visibility = Visibility.Hidden;
                                this.ClearEffect();
                            }));
                        break;
                }

                if (this.SlideFadeIn)
                {
                    this.mainContent.BeginAnimation(OpacityProperty, AnimationUtils.CreateFadeAnimation(0, 1, this.SlideFadeInDuration));
                    this.paintArea.BeginAnimation(OpacityProperty, AnimationUtils.CreateFadeAnimation(1, 0, this.SlideFadeInDuration));
                }
            }
        }
    }
}