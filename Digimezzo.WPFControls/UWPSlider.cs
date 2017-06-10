using System;
using System.Windows;
using System.Windows.Media;
using Digimezzo.WPFControls.Base;

namespace Digimezzo.WPFControls
{
    public class HorizontalUWPSlider : HorizontalWindows8Slider
    {
        #region Properties
        public double BarFillPosition
        {
            get { return Convert.ToDouble(BarFillPositionProperty); }
            set { SetValue(BarFillPositionProperty, value); }
        }

        public Brush ButtonInnerBackground
        {
            get { return (Brush)GetValue(ButtonInnerBackgroundProperty); }
            set { SetValue(ButtonInnerBackgroundProperty, value); }
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty BarFillPositionProperty = DependencyProperty.Register("BarFillPosition", typeof(double), typeof(HorizontalUWPSlider), new PropertyMetadata(0.0));
        public static readonly DependencyProperty ButtonInnerBackgroundProperty = DependencyProperty.Register("ButtonInnerBackground", typeof(Brush), typeof(HorizontalUWPSlider), new PropertyMetadata(null));
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
            this.CalculateVisibleLengths();
        }

        protected override void CalculatePosition()
        {
            base.CalculatePosition();
            this.CalculateVisibleLengths();
        }
        #endregion

        #region Virtual
        protected virtual void CalculateVisibleLengths()
        {
            if (this.sliderCanvas != null && this.sliderCanvas.ActualWidth != 0 && this.sliderButton != null)
            {
                this.BarFillPosition = this.Position * (this.sliderCanvas.ActualWidth - Constants.UWPSliderCanvasLengthOffset) /
                                       this.sliderCanvas.ActualWidth;
            }
        }
        #endregion
    }

    public class HorizontalUWPBottomSlider : HorizontalUWPSlider
    {
        #region Variables
        private static readonly CornerRadius rightCornerRadius = new CornerRadius(Constants.UWPSliderBaseUnit, Constants.UWPSliderBaseUnit, 0, Constants.UWPSliderBaseUnit);
        private static readonly CornerRadius leftCornerRadius = new CornerRadius(Constants.UWPSliderBaseUnit, Constants.UWPSliderBaseUnit, Constants.UWPSliderBaseUnit, 0);
        private static readonly Thickness rightButtonMargin = new Thickness(-24, 0, 0, 0);
        private static readonly Thickness leftButtonMargin = new Thickness(-8, 0, 0, 0);
        private static readonly double leftButtonBorderLeft = 20;
        private static readonly double rightButtonBorderLeft = 4;
        #endregion

        #region Properties
        private CornerRadius SliderButtonCornerRadius
        {
            get { return (CornerRadius)GetValue(SliderButtonCornerRadiusProperty); }
            set { SetValue(SliderButtonCornerRadiusProperty, value); }
        }

        private Thickness SliderButtonMargin
        {
            get { return (Thickness)GetValue(SliderButtonMarginProperty); }
            set { SetValue(SliderButtonMarginProperty, value); }
        }

        private double SliderButtonBorderLeft
        {
            get { return (double)GetValue(SliderButtonBorderLeftProperty); }
            set { SetValue(SliderButtonBorderLeftProperty, value); }
        }
        #endregion

        #region Dependency Properties
        private static readonly DependencyProperty SliderButtonCornerRadiusProperty = DependencyProperty.Register("SliderButtonCornerRadius", typeof(CornerRadius), typeof(HorizontalUWPBottomSlider), new PropertyMetadata(null));
        private static readonly DependencyProperty SliderButtonMarginProperty = DependencyProperty.Register("SliderButtonMargin", typeof(Thickness), typeof(HorizontalUWPBottomSlider), new PropertyMetadata(null));
        private static readonly DependencyProperty SliderButtonBorderLeftProperty = DependencyProperty.Register("SliderButtonBorderLeft", typeof(double), typeof(HorizontalUWPBottomSlider), new PropertyMetadata(null));
   
        #endregion

        #region Construction
        static HorizontalUWPBottomSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HorizontalUWPBottomSlider), new FrameworkPropertyMetadata(typeof(HorizontalUWPBottomSlider)));
        }
        #endregion

        #region Overrides

        protected override void CalculateVisibleLengths()
        {
            if (this.sliderCanvas == null) return;

            if (this.Position > this.sliderCanvas.ActualWidth / 2)
            {
                this.SliderButtonCornerRadius = rightCornerRadius;
                this.SliderButtonMargin = rightButtonMargin;
                this.SliderButtonBorderLeft= rightButtonBorderLeft;
                this.BarFillPosition = this.Position ;
            }
            else
            {
                this.SliderButtonCornerRadius = leftCornerRadius;
                this.SliderButtonMargin = leftButtonMargin;
                this.SliderButtonBorderLeft = leftButtonBorderLeft;
                this.BarFillPosition = this.Position;
            }
        }
        #endregion
    }

    public class VerticalUWPSlider : VerticalWindows8Slider
    {
        #region Properties
        public double BarFillPosition
        {
            get { return Convert.ToDouble(BarFillPositionProperty); }
            set { SetValue(BarFillPositionProperty, value); }
        }
        public Brush ButtonInnerBackground
        {
            get { return (Brush)GetValue(ButtonInnerBackgroundProperty); }
            set { SetValue(ButtonInnerBackgroundProperty, value); }
        }
        #endregion

        #region DependencyProperty
        public static readonly DependencyProperty BarFillPositionProperty = DependencyProperty.Register("BarFillPosition", typeof(double), typeof(VerticalUWPSlider), new PropertyMetadata(0.0));
        public static readonly DependencyProperty ButtonInnerBackgroundProperty = DependencyProperty.Register("ButtonInnerBackground", typeof(Brush), typeof(VerticalUWPSlider), new PropertyMetadata(null));
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
            this.CalculateVisibleLengths();
        }

        protected override void CalculatePosition()
        {
            base.CalculatePosition();
            this.CalculateVisibleLengths();
        }
        #endregion

        #region Private
        private void CalculateVisibleLengths()
        {
            if (this.sliderCanvas != null && this.sliderCanvas.ActualWidth != 0 && this.sliderButton != null)
            {
                this.BarFillPosition = this.Position * (this.sliderCanvas.ActualHeight - Constants.UWPSliderCanvasLengthOffset) /
                                       this.sliderCanvas.ActualHeight;
            }
        }
        #endregion
    }
}
