using System.Windows;
using System.Windows.Controls;

namespace Digimezzo.WPFControls
{
    public class HorizontalUWPSlider : HorizontalWindows8Slider
    {
        #region Construction
        static HorizontalUWPSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HorizontalUWPSlider), new FrameworkPropertyMetadata(typeof(HorizontalUWPSlider)));
        }
        #endregion

        #region Protected
        protected override void UpdatePosition()
        {
            base.UpdatePosition();
            this.CalculateButtonMargin();
        }

        protected override void CalculatePosition()
        {
            base.CalculatePosition();
            this.CalculateButtonMargin();
        }
        #endregion

        #region Virtual
        protected virtual void CalculateButtonMargin()
        {
            if (this.sliderButton == null || this.sliderCanvas == null) return;

            int margin = 0;

            if (this.sliderCanvas.ActualWidth != 0)
            {
                margin = (int)(this.sliderButton.ActualWidth * (this.Position / this.sliderCanvas.ActualWidth));
            }
            this.sliderButton.Margin = new Thickness(-margin, 0, 0, 0);
        }
        #endregion
    }

    public class HorizontalUWPBottomSlider : HorizontalUWPSlider
    {
        #region Variables
        protected Border sliderButtonBorder;
        #endregion

        #region Construction
        static HorizontalUWPBottomSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HorizontalUWPBottomSlider), new FrameworkPropertyMetadata(typeof(HorizontalUWPBottomSlider)));
        }
        #endregion

        #region Overrides
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.sliderButtonBorder = (Border)GetTemplateChild("PART_BorderHelper");
        }

        protected override void CalculateButtonMargin()
        {
            base.CalculateButtonMargin();

            if (this.sliderButtonBorder == null) return;

            if (this.Position > this.sliderCanvas.ActualWidth / 2)
            {
                this.sliderButtonBorder.CornerRadius = new CornerRadius(8,8,0,8);
            }
            else
            {
                this.sliderButtonBorder.CornerRadius = new CornerRadius(8, 8, 8, 0);
            }
        }
        #endregion
    }

    public class VerticalUWPSlider : VerticalWindows8Slider
    {
        #region Construction
        static VerticalUWPSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VerticalUWPSlider), new FrameworkPropertyMetadata(typeof(VerticalUWPSlider)));
        }
        #endregion

        #region Protected
        protected override void UpdatePosition()
        {
            base.UpdatePosition();
            this.CalculateButtonMargin();
        }

        protected override void CalculatePosition()
        {
            base.CalculatePosition();
            this.CalculateButtonMargin();
        }
        #endregion

        #region Private
        private void CalculateButtonMargin()
        {
            if (this.sliderButton == null || this.sliderCanvas == null) return;

            int margin = 0;

            if (this.sliderCanvas.ActualHeight != 0)
            {
                margin = (int)(this.sliderButton.ActualHeight * (this.Position / this.sliderCanvas.ActualHeight));
            }
            this.sliderButton.Margin = new Thickness(0, 0, 0, -margin);
        }
        #endregion
    }
}
