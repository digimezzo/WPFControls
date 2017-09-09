using Digimezzo.WPFControls.Enums;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Digimezzo.WPFControls
{
    public class SlidingContentControl : ContentControl
    {
        #region Variables
        protected ContentPresenter mainContent;
        protected Shape paintArea;
        #endregion

        #region Properties
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
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty SlideDirectionProperty = DependencyProperty.Register("SlideDirection", typeof(SlideDirection), typeof(SlidingContentControl), new PropertyMetadata(Enums.SlideDirection.RightToLeft));
        public static readonly DependencyProperty EasingAmplitudeProperty = DependencyProperty.Register("EasingAmplitude", typeof(double), typeof(SlidingContentControl), new PropertyMetadata(0.5));
        public static readonly DependencyProperty SlideDurationProperty = DependencyProperty.Register("SlideDuration", typeof(double), typeof(SlidingContentControl), new PropertyMetadata(0.5));
        public static readonly DependencyProperty FadeOutDurationProperty = DependencyProperty.Register("FadeOutDuration", typeof(double), typeof(SlidingContentControl), new PropertyMetadata(0.3));
        public static readonly DependencyProperty FadeInDurationProperty = DependencyProperty.Register("FadeInDuration", typeof(double), typeof(SlidingContentControl), new PropertyMetadata(0.7));
        public static readonly DependencyProperty FadeOnSlideProperty = DependencyProperty.Register("FadeOnSlide", typeof(bool), typeof(SlidingContentControl), new PropertyMetadata(false));
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
                    this.paintArea.Fill = this.CreateBrushFromVisual(this.mainContent);
                    this.BeginAnimateContentReplacement();
                }
                base.OnContentChanged(oldContent, newContent);

            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Starts the animation for the new content
        /// </summary>
        protected virtual void BeginAnimateContentReplacement()
        {
            var newContentTransform = new TranslateTransform();
            var oldContentTransform = new TranslateTransform();
            this.paintArea.RenderTransform = oldContentTransform;
            this.mainContent.RenderTransform = newContentTransform;
            this.paintArea.Visibility = Visibility.Visible;

            switch (this.SlideDirection)
            {
                case SlideDirection.LeftToRight:
                    newContentTransform.BeginAnimation(TranslateTransform.XProperty, this.CreateSlideAnimation(-this.ActualWidth, 0));
                    oldContentTransform.BeginAnimation(TranslateTransform.XProperty, this.CreateSlideAnimation(0, this.ActualWidth, (s, e) => this.paintArea.Visibility = Visibility.Hidden));
                    break;
                case SlideDirection.RightToLeft:
                    newContentTransform.BeginAnimation(TranslateTransform.XProperty, this.CreateSlideAnimation(this.ActualWidth, 0));
                    oldContentTransform.BeginAnimation(TranslateTransform.XProperty, this.CreateSlideAnimation(0, -this.ActualWidth, (s, e) => this.paintArea.Visibility = Visibility.Hidden));
                    break;
                case SlideDirection.UpToDown:
                    newContentTransform.BeginAnimation(TranslateTransform.YProperty, this.CreateSlideAnimation(-this.ActualHeight, 0));
                    oldContentTransform.BeginAnimation(TranslateTransform.YProperty, this.CreateSlideAnimation(0, this.ActualHeight, (s, e) => this.paintArea.Visibility = Visibility.Hidden));
                    break;
                case SlideDirection.DownToUp:
                    newContentTransform.BeginAnimation(TranslateTransform.YProperty, this.CreateSlideAnimation(this.ActualHeight, 0));
                    oldContentTransform.BeginAnimation(TranslateTransform.YProperty, this.CreateSlideAnimation(0, -this.ActualHeight, (s, e) => this.paintArea.Visibility = Visibility.Hidden));
                    break;
            }

            if (this.FadeOnSlide)
            {
                this.mainContent.BeginAnimation(OpacityProperty, this.CreateFadeAnimation(0, 1, this.FadeInDuration));
                this.paintArea.BeginAnimation(OpacityProperty, this.CreateFadeAnimation(1, 0, this.FadeOutDuration));
            }
        }

        /// <summary>
        /// Creates a brush based on the current appearance of a visual element. 
        /// The brush is an ImageBrush and once created, won't update its look
        /// </summary>
        /// <param name="visual">The visual element to take a snapshot of</param>
        protected Brush CreateBrushFromVisual(Visual visual)
        {
            if (visual == null)
            {
                throw new ArgumentNullException("visual");
            }

            var target = new RenderTargetBitmap(Convert.ToInt32(this.ActualWidth), Convert.ToInt32(this.ActualHeight), 96, 96, PixelFormats.Pbgra32);
            target.Render(visual);
            var brush = new ImageBrush(target);
            brush.Freeze();
            return brush;
        }

        /// <summary>
        /// Creates the animation that moves content in or out of view.
        /// </summary>
        /// <param name="from">The starting value of the animation.</param>
        /// <param name="to">The end value of the animation.</param>
        /// <param name="whenDone">(optional)
        ///   A callback that will be called when the animation has completed.</param>
        protected AnimationTimeline CreateSlideAnimation(double from, double to, EventHandler whenDone = null)
        {

            IEasingFunction ease = new BackEase
            {
                Amplitude = this.EasingAmplitude,
                EasingMode = EasingMode.EaseOut
            };

            var duration = new Duration(TimeSpan.FromSeconds(this.SlideDuration));
            var anim = new DoubleAnimation(from, to, duration) { EasingFunction = ease };

            if (whenDone != null) anim.Completed += whenDone;

            anim.Freeze();

            return anim;
        }

        protected AnimationTimeline CreateFadeAnimation(double from, double to, double durationSeconds, EventHandler whenDone = null)
        {
            var duration = new Duration(TimeSpan.FromSeconds(durationSeconds));
            var anim = new DoubleAnimation(from, to, duration);

            if (whenDone != null)
            {
                anim.Completed += whenDone;
            }

            anim.Freeze();

            return anim;
        }
        #endregion
    }
}