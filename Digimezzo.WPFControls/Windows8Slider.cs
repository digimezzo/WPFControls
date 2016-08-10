using Digimezzo.WPFControls.Base;
using System.Windows;
using System.Windows.Input;

namespace Digimezzo.WPFControls
{
    public class HorizontalWindows8Slider : SliderBase
    {

        #region Construction
        static HorizontalWindows8Slider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HorizontalWindows8Slider), new FrameworkPropertyMetadata(typeof(HorizontalWindows8Slider)));
        }
        #endregion

        #region Overrides
        protected override void UpdatePosition()
        {
            this.Position = Mouse.GetPosition(this.sliderCanvas).X;

            if (this.Position > this.sliderCanvas.ActualWidth)
            {
                this.Position = this.sliderCanvas.ActualWidth;
            }

            if (this.Position < 0.0)
            {
                this.Position = 0.0;
            }

        }

        protected override void CalculatePosition()
        {
            if (!this.isCalculating)
            {
                this.isCalculating = true;

                if ((this.Maximum - this.Minimum) > 0 && this.ActualWidth > 0)
                {
                    this.Position = ((this.Value -this.Minimum) / (this.Maximum - this.Minimum)) * this.ActualWidth;
                }
                else
                {
                    this.Position = 0;
                }

                this.isCalculating = false;
            }
        }

        protected override void CalculateValue()
        {
            if (!this.isCalculating)
            {
                this.isCalculating = true;

                if (this.ActualWidth > 0)
                {
                    this.Value = ((this.Position * (this.Maximum - this.Minimum)) / this.ActualWidth) + this.Minimum;
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

    public class VerticalWindows8Slider : SliderBase
    {
        #region Construction
        static VerticalWindows8Slider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VerticalWindows8Slider), new FrameworkPropertyMetadata(typeof(VerticalWindows8Slider)));
        }
        #endregion

        #region Overrides
        protected override void UpdatePosition()
        {
            this.Position = this.sliderCanvas.ActualHeight - Mouse.GetPosition(this.sliderCanvas).Y;

            if (this.Position > this.sliderCanvas.ActualHeight)
            {
                this.Position = this.sliderCanvas.ActualHeight;
            }

            if (this.Position < 0.0)
            {
                this.Position = 0.0;
            }

        }

        protected override void CalculatePosition()
        {
            if (!this.isCalculating)
            {
                this.isCalculating = true;

                if ((this.Maximum - this.Minimum) > 0 && this.ActualHeight > 0)
                {
                    this.Position = ((this.Value - this.Minimum) / (this.Maximum - this.Minimum)) * this.ActualHeight;
                }
                else
                {
                    this.Position = 0;
                }

                this.isCalculating = false;
            }
        }

        protected override void CalculateValue()
        {
            if (!this.isCalculating)
            {
                this.isCalculating = true;

                if (this.ActualHeight > 0)
                {
                    this.Value = ((this.Position * (this.Maximum - this.Minimum)) / this.ActualHeight) + this.Minimum;
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
}