using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Animation;

namespace Digimezzo.WPFControls.CustomAnimations
{
    // Quote from http://www.cnblogs.com/zhouyinhui/archive/2007/05/16/748952.html
    [DebuggerStepThrough]
    public class AccelerationDoubleAnimation : DoubleAnimationBase
    {
        public static readonly DependencyProperty FromProperty = DependencyProperty.Register(
            "From", typeof(double), typeof(AccelerationDoubleAnimation), new PropertyMetadata(null));

        public double From
        {
            get { return (double) GetValue(FromProperty); }
            set { SetValue(FromProperty, value); }
        }

        public static readonly DependencyProperty ToProperty = DependencyProperty.Register(
            "To", typeof(double), typeof(AccelerationDoubleAnimation), new PropertyMetadata(null));

        public double To
        {
            get { return (double) GetValue(ToProperty); }
            set { SetValue(ToProperty, value); }
        }

        public static readonly DependencyProperty PowerProperty = DependencyProperty.Register(
            "Power", typeof(double), typeof(AccelerationDoubleAnimation), new PropertyMetadata((double)2));

        public double Power
        {
            get { return (double) GetValue(PowerProperty); }
            set { SetValue(PowerProperty, value); }
        }

        public static readonly DependencyProperty MovementStyleProperty = DependencyProperty.Register(
            "MovementStyle", typeof(MovementStyleEnum), typeof(AccelerationDoubleAnimation),
            new PropertyMetadata(default(MovementStyleEnum)));

        public MovementStyleEnum MovementStyle
        {
            get { return (MovementStyleEnum) GetValue(MovementStyleProperty); }
            set { SetValue(MovementStyleProperty, value); }
        }

        public enum MovementStyleEnum
        {
            AccelerateAndSlowdown,
            Accelerate,
        }

        protected override Freezable CreateInstanceCore()
        {
            return new AccelerationDoubleAnimation();
        }

        protected override double GetCurrentValueCore(double defaultOriginValue, double defaultDestinationValue,
            AnimationClock animationClock)
        {
            var delta = To - From;

            switch (MovementStyle)
            {
                default:
                case MovementStyleEnum.Accelerate:
                    return delta * Math.Pow(animationClock.CurrentProgress.Value, Power) + From;
                case MovementStyleEnum.AccelerateAndSlowdown:
                    if (animationClock.CurrentProgress.Value < 0.5)
                    {
                        return delta / 2 * Math.Pow(animationClock.CurrentProgress.Value * 2, Power) + From;
                    }
                    return delta / 2 * Math.Pow((animationClock.CurrentProgress.Value - 0.5) * 2, 1 / Power) + delta / 2 +
                           From;
            }
        }
    }
}