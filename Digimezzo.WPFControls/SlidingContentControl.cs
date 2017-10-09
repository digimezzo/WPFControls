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
        #region Variables
        private ContentPresenter mainContent;
        private Shape paintArea;
        private FeatheringEffect effect;
        private bool isFirstShow = true;
        #endregion

        #region Properties
        public ContentPresenter MainContent
        {
            get { return this.mainContent; }
        }

        public Shape PaintArea
        {
            get { return this.paintArea; }
        }

        public SlideDirection SlideDirection
        {
            get { return (SlideDirection)GetValue(SlideDirectionProperty); }
            set { SetValue(SlideDirectionProperty, value); }
        }

        public double EasingAmplitude
        {
            get { return Convert.ToDouble(GetValue(EasingAmplitudeProperty)); }
            set { SetValue(EasingAmplitudeProperty, value); }
        }

        public double SlideDuration
        {
            get { return Convert.ToDouble(GetValue(SlideDurationProperty)); }
            set { SetValue(SlideDurationProperty, value); }
        }

        public double FadeOutDuration
        {
            get { return Convert.ToDouble(GetValue(FadeOutDurationProperty)); }
            set { SetValue(FadeOutDurationProperty, value); }
        }

        public double FadeInDuration
        {
            get { return Convert.ToDouble(GetValue(FadeInDurationProperty)); }
            set { SetValue(FadeInDurationProperty, value); }
        }

        public bool FadeOnSlide
        {
            get { return (bool)GetValue(FadeOnSlideProperty); }
            set { SetValue(FadeOnSlideProperty, value); }
        }

        public double FeatheringRadius
        {
            get => (double)GetValue(FeatheringRadiusProperty);
            set => SetValue(FeatheringRadiusProperty, value);
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty SlideDirectionProperty = DependencyProperty.Register("SlideDirection", 
            typeof(SlideDirection), typeof(SlidingContentControl), new PropertyMetadata(Enums.SlideDirection.RightToLeft));
        public static readonly DependencyProperty EasingAmplitudeProperty = DependencyProperty.Register("EasingAmplitude", 
            typeof(double), typeof(SlidingContentControl), new PropertyMetadata(0.0));
        public static readonly DependencyProperty SlideDurationProperty = DependencyProperty.Register("SlideDuration", 
            typeof(double), typeof(SlidingContentControl), new PropertyMetadata(0.5));
        public static readonly DependencyProperty FadeOutDurationProperty = DependencyProperty.Register("FadeOutDuration", 
            typeof(double), typeof(SlidingContentControl), new PropertyMetadata(0.3));
        public static readonly DependencyProperty FadeInDurationProperty = DependencyProperty.Register("FadeInDuration", 
            typeof(double), typeof(SlidingContentControl), new PropertyMetadata(0.7));
        public static readonly DependencyProperty FadeOnSlideProperty = DependencyProperty.Register("FadeOnSlide", 
            typeof(bool), typeof(SlidingContentControl), new PropertyMetadata(false));
        public static DependencyProperty FeatheringRadiusProperty = DependencyProperty.Register("FeatheringRadius",
           typeof(double), typeof(SlidingContentControl), new PropertyMetadata(0.0));
        #endregion

        #region Construction
        static SlidingContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SlidingContentControl), new FrameworkPropertyMetadata(typeof(SlidingContentControl)));
        }
        #endregion

        #region Public
        public override void OnApplyTemplate()
        {
            this.mainContent = (ContentPresenter)GetTemplateChild("PART_MainContent");
            this.paintArea = (Shape)GetTemplateChild("PART_PaintArea");
            this.effect = new FeatheringEffect() { FeatheringRadius = this.FeatheringRadius };

            base.OnApplyTemplate();
        }
        #endregion

        #region Protected
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            try
            {
                if (this.paintArea != null && this.mainContent != null)
                {
                    this.paintArea.Fill = AnimationUtils.CreateBrushFromVisual(this.mainContent, this.ActualWidth, this.ActualHeight);
                    this.BeginAnimateContentReplacement();
                }
                base.OnContentChanged(oldContent, newContent);

            }
            catch (Exception)
            {
            }
        }

        protected virtual void BeginAnimateContentReplacement()
        {
            // When the first time we show the content, do not apply the effect to prevent side-effects.
            if (this.isFirstShow)
            {
                this.isFirstShow = false;
            }
            else if(this.FeatheringRadius > 0.0)
            {
                this.effect.TexWidth = ActualWidth;
                this.Effect = effect;
            }

            var newContentTransform = new TranslateTransform();
            var oldContentTransform = new TranslateTransform();
            this.paintArea.RenderTransform = oldContentTransform;
            this.mainContent.RenderTransform = newContentTransform;
            this.paintArea.Visibility = Visibility.Visible;

            switch (this.SlideDirection)
            {
                case SlideDirection.LeftToRight:
                    newContentTransform.BeginAnimation(TranslateTransform.XProperty, AnimationUtils.CreateSlideAnimation(-this.ActualWidth, 0, this.EasingAmplitude, this.SlideDuration));
                    oldContentTransform.BeginAnimation(TranslateTransform.XProperty, AnimationUtils.CreateSlideAnimation(0, this.ActualWidth, this.EasingAmplitude, this.SlideDuration,
                        (s, e) =>
                        {
                            this.PaintArea.Visibility = Visibility.Hidden;
                            this.Effect = null;
                        }));
                    break;
                case SlideDirection.RightToLeft:
                    newContentTransform.BeginAnimation(TranslateTransform.XProperty, AnimationUtils.CreateSlideAnimation(this.ActualWidth, 0, this.EasingAmplitude, this.SlideDuration));
                    oldContentTransform.BeginAnimation(TranslateTransform.XProperty, AnimationUtils.CreateSlideAnimation(0, -this.ActualWidth, this.EasingAmplitude, this.SlideDuration,
                       (s, e) =>
                       {
                           this.PaintArea.Visibility = Visibility.Hidden;
                           this.Effect = null;
                       }));
                    break;
                case SlideDirection.UpToDown:
                    newContentTransform.BeginAnimation(TranslateTransform.YProperty, AnimationUtils.CreateSlideAnimation(-this.ActualHeight, 0, this.EasingAmplitude, this.SlideDuration));
                    oldContentTransform.BeginAnimation(TranslateTransform.YProperty, AnimationUtils.CreateSlideAnimation(0, this.ActualHeight, this.EasingAmplitude, this.SlideDuration,
                        (s, e) =>
                        {
                            this.PaintArea.Visibility = Visibility.Hidden;
                            this.Effect = null;
                        }));
                    break;
                case SlideDirection.DownToUp:
                    newContentTransform.BeginAnimation(TranslateTransform.YProperty, AnimationUtils.CreateSlideAnimation(this.ActualHeight, 0, this.EasingAmplitude, this.SlideDuration));
                    oldContentTransform.BeginAnimation(TranslateTransform.YProperty, AnimationUtils.CreateSlideAnimation(0, -this.ActualHeight, this.EasingAmplitude, this.SlideDuration,
                        (s, e) =>
                        {
                            this.PaintArea.Visibility = Visibility.Hidden;
                            this.Effect = null;
                        }));
                    break;
            }

            if (this.FadeOnSlide)
            {
                this.mainContent.BeginAnimation(OpacityProperty, AnimationUtils.CreateFadeAnimation(0, 1, this.FadeInDuration));
                this.paintArea.BeginAnimation(OpacityProperty, AnimationUtils.CreateFadeAnimation(1, 0, this.FadeOutDuration));
            }
        }
        #endregion
    }
}