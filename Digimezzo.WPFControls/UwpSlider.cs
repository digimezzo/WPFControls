using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Digimezzo.WPFControls
{
    public class HorizontalUWPSlider : HorizontalWindows8Slider
    {
        #region Properties
        [EditorBrowsable(EditorBrowsableState.Never)]
        public double VisibleBarLength
        {
            get { return Convert.ToDouble(GetValueVisibleBarLengthProperty); }
            set { SetValue(GetValueVisibleBarLengthProperty, value); }
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty GetValueVisibleBarLengthProperty = DependencyProperty.Register("VisibleBarLength", typeof(double), typeof(HorizontalUWPSlider), new PropertyMetadata(0.0));
        #endregion

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
            this.CalculateVisibleBarLength();
        }

        protected override void CalculatePosition()
        {
            base.CalculatePosition();
            this.CalculateVisibleBarLength();
        }
        #endregion

        #region Virtual
        protected virtual void CalculateVisibleBarLength()
        {
            if (this.sliderCanvas != null && this.sliderCanvas.ActualWidth != 0 && this.sliderButton != null)
            {
                this.VisibleBarLength = this.Position + (int)(this.sliderButton.ActualWidth * (this.Position / this.sliderCanvas.ActualWidth));
            }
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

        protected override void CalculateVisibleBarLength()
        {
            base.CalculateVisibleBarLength();

            if (this.sliderButtonBorder == null) return;

            if (this.Position > this.sliderCanvas.ActualWidth / 2)
            {
                this.sliderButtonBorder.CornerRadius = new CornerRadius(8, 8, 0, 8);
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
        #region Properties
        [EditorBrowsable(EditorBrowsableState.Never)]
        public double VisibleBarLength
        {
            get { return Convert.ToDouble(GetValueVisibleBarLengthProperty); }
            set { SetValue(GetValueVisibleBarLengthProperty, value); }
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty GetValueVisibleBarLengthProperty = DependencyProperty.Register("VisibleBarLength", typeof(double), typeof(VerticalUWPSlider), new PropertyMetadata(0.0));
        #endregion

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
            this.CalculateVisibleBarLength();
        }

        protected override void CalculatePosition()
        {
            base.CalculatePosition();
            this.CalculateVisibleBarLength();
        }
        #endregion

        #region Private
        private void CalculateVisibleBarLength()
        {
            if (this.sliderCanvas != null && this.sliderCanvas.ActualWidth != 0 && this.sliderButton != null)
            {
                this.VisibleBarLength = this.Position + (int)(this.sliderButton.ActualHeight * (this.Position / this.sliderCanvas.ActualHeight));
            }
        }
        #endregion
    }
}
