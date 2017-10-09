using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Digimezzo.WPFControls.Utils
{
    public sealed class AnimationUtils
    {
        public static AnimationTimeline CreateSlideAnimation(double from, double to, double easingAmplitude, double slideDuration, EventHandler whenDone = null)
        {
            IEasingFunction ease = new BackEase
            {
                Amplitude = easingAmplitude,
                EasingMode = EasingMode.EaseOut
            };

            var duration = new Duration(TimeSpan.FromSeconds(slideDuration));
            var anim = new DoubleAnimation(from, to, duration) { EasingFunction = ease };

            if (whenDone != null) anim.Completed += whenDone;

            anim.Freeze();

            return anim;
        }

        public static AnimationTimeline CreateFadeAnimation(double from, double to, double durationSeconds, EventHandler whenDone = null)
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

        public static Brush CreateBrushFromVisual(Visual visual, double desiredWidth, double desiredHeight)
        {
            if (visual == null)
            {
                throw new ArgumentNullException("visual");
            }

            var target = new RenderTargetBitmap(Convert.ToInt32(desiredWidth), Convert.ToInt32(desiredHeight), 96, 96, PixelFormats.Pbgra32);
            target.Render(visual);
            var brush = new ImageBrush(target);
            brush.Freeze();
            return brush;
        }
    }
}
