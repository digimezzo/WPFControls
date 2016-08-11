using Digimezzo.WPFControls.Base;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Digimezzo.WPFControls
{
    public abstract class UWPSliderBase : SliderBase
    {
        #region Variables
        protected Canvas sliderCanvasHelper;
        #endregion

        #region Properties
        public double BarLength
        {
            get { return (double)GetValue(BarLengthProperty); }
            set { SetValue(BarLengthProperty, value); }
        }

        public double TrackLength
        {
            get { return (double)GetValue(TrackLengthProperty); }
            set { SetValue(TrackLengthProperty, value); }
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty BarLengthProperty = DependencyProperty.Register("BarLength", typeof(double), typeof(UWPSliderBase), new PropertyMetadata(0.0));
        public static readonly DependencyProperty TrackLengthProperty = DependencyProperty.Register("TrackLength", typeof(double), typeof(UWPSliderBase), new PropertyMetadata(0.0));
        #endregion

        #region Overrides
        public override void OnApplyTemplate()
        {
            this.sliderCanvasHelper = (Canvas)GetTemplateChild("PART_CanvasHelper");
            base.OnApplyTemplate();
        }
        #endregion

        #region Protected
        protected abstract void SetLengths();
        #endregion
    }
    public class HorizontalUWPSlider : UWPSliderBase
    {
        #region Construction
        static HorizontalUWPSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HorizontalUWPSlider), new FrameworkPropertyMetadata(typeof(HorizontalUWPSlider)));
        }
        #endregion

        #region Overrides
        protected override void SetLengths()
        {
            this.BarLength = this.Position;
            this.TrackLength = this.sliderCanvasHelper.ActualWidth - this.Position;
        }

        protected override void UpdatePosition()
        {
            this.Position = Mouse.GetPosition(this.sliderCanvas).X - this.sliderButton.ActualWidth / 2;

            if (this.Position > this.sliderCanvasHelper.ActualWidth)
            {
                this.Position = this.sliderCanvasHelper.ActualWidth;
            }

            if (this.Position < 0.0)
            {
                this.Position = 0.0;
            }

            this.SetLengths();
        }

        protected override void CalculatePosition()
        {
            if (this.sliderCanvasHelper == null) return;

            if (!this.isCalculating)
            {
                this.isCalculating = true;

                if ((this.Maximum - this.Minimum) > 0)
                {
                    this.Position = ((this.Value - this.Minimum) / (this.Maximum - this.Minimum)) * this.sliderCanvasHelper.ActualWidth;
                }
                else
                {
                    this.Position = 0;
                }

                this.SetLengths();

                this.isCalculating = false;
            }
        }

        protected override void CalculateValue()
        {
            if (this.sliderCanvasHelper == null) return;

            if (!this.isCalculating)
            {
                this.isCalculating = true;

                if (this.ActualWidth > 0)
                {
                    this.Value = ((this.Position * (this.Maximum - this.Minimum)) / this.sliderCanvasHelper.ActualWidth) + this.Minimum;
                }
                else
                {
                    this.Value = 0;
                }

                this.isCalculating = false;
            }
        }
        #endregion
    }

    public class VerticalUWPSlider : UWPSliderBase
    {
        #region Construction
        static VerticalUWPSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VerticalUWPSlider), new FrameworkPropertyMetadata(typeof(VerticalUWPSlider)));
        }
        #endregion

        #region Overrides
        protected override void SetLengths()
        {
            this.BarLength = this.Position;
            this.TrackLength = this.sliderCanvasHelper.ActualHeight - this.Position;
        }

        protected override void UpdatePosition()
        {
            this.Position = this.sliderCanvas.ActualHeight - Mouse.GetPosition(this.sliderCanvas).Y - this.sliderButton.ActualWidth / 2;

            if (this.Position > this.sliderCanvasHelper.ActualHeight)
            {
                this.Position = this.sliderCanvasHelper.ActualHeight;
            }

            if (this.Position < 0.0)
            {
                this.Position = 0.0;
            }

            this.SetLengths();
        }

        protected override void CalculatePosition()
        {
            if (this.sliderCanvasHelper == null) return;

            if (!this.isCalculating)
            {
                this.isCalculating = true;

                if ((this.Maximum - this.Minimum) > 0)
                {
                    this.Position = ((this.Value - this.Minimum) / (this.Maximum - this.Minimum)) * this.sliderCanvasHelper.ActualHeight;
                }
                else
                {
                    this.Position = 0;
                }

                this.SetLengths();

                this.isCalculating = false;
            }
        }

        protected override void CalculateValue()
        {
            if (this.sliderCanvasHelper == null) return;

            if (!this.isCalculating)
            {
                this.isCalculating = true;

                if (this.sliderCanvasHelper.ActualHeight > 0)
                {
                    this.Value = ((this.Position * (this.Maximum - this.Minimum)) / this.sliderCanvasHelper.ActualHeight) + this.Minimum;
                }
                else
                {
                    this.Value = 0;
                }

                this.isCalculating = false;
            }
        }
        #endregion
    }

    public class HorizontalUWPBottomSlider : HorizontalUWPSlider
    {
        #region Variables
        protected Border sliderBorderHelper;
        #endregion

        #region Construction
        static HorizontalUWPBottomSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HorizontalUWPBottomSlider), new FrameworkPropertyMetadata(typeof(HorizontalUWPBottomSlider)));
        }
        #endregion

        #region Overrides
        protected override void SetLengths()
        {
            this.BarLength = this.Position + this.sliderButton.ActualWidth / 2;
            this.TrackLength = this.sliderCanvas.ActualWidth - this.Position - this.sliderButton.ActualWidth / 2;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.sliderBorderHelper = (Border)GetTemplateChild("PART_BorderHelper");
        }

        protected override void UpdatePosition()
        {
            this.Position = Mouse.GetPosition(this.sliderCanvas).X - this.sliderButton.ActualWidth / 2;

            if (this.Position > this.sliderCanvasHelper.ActualWidth)
            {
                this.Position = this.sliderCanvasHelper.ActualWidth;
            }

            if (this.Position < 0.0)
            {
                this.Position = 0.0;
            }

            this.SetLengths();

            if(this.Position < this.sliderCanvas.ActualWidth / 2)
            {
                this.sliderBorderHelper.CornerRadius = new CornerRadius(9, 9, 9, 0);
            }else
            {
                this.sliderBorderHelper.CornerRadius = new CornerRadius(9, 9, 0, 9);
            }
        }
        #endregion
    }
}
