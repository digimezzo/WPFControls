using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Digimezzo.WPFControls.Base;

namespace Digimezzo.WPFControls
{
    public class HorizontalUWPSlider : HorizontalWindows8Slider
    {
        #region Properties
        public double ButtonPosition
        {
            get { return Convert.ToDouble(ButtonPositionProperty); }
            set { SetValue(ButtonPositionProperty, value); }
        }

        public Brush ButtonInnerBackground
        {
            get { return (Brush) GetValue(ButtonInnerBackgroundProperty); }
            set { SetValue(ButtonInnerBackgroundProperty, value);}
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty ButtonPositionProperty = DependencyProperty.Register("ButtonPosition", typeof(double), typeof(HorizontalUWPSlider), new PropertyMetadata(0.0));
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
                this.ButtonPosition = this.Position * (this.sliderCanvas.ActualWidth - 16) /
                                      this.sliderCanvas.ActualWidth;
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

        protected override void CalculateVisibleLengths()
        {
            base.CalculateVisibleLengths();

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
        public double ButtonPosition
        {
            get { return Convert.ToDouble(ButtonPositionProperty); }
            set { SetValue(ButtonPositionProperty, value); }
        }
        public Brush ButtonInnerBackground
        {
            get { return (Brush)GetValue(ButtonInnerBackgroundProperty); }
            set { SetValue(ButtonInnerBackgroundProperty, value); }
        }
        #endregion

        #region DependencyProperty
        public static readonly DependencyProperty ButtonPositionProperty = DependencyProperty.Register("ButtonPosition", typeof(double), typeof(VerticalUWPSlider), new PropertyMetadata(0.0));
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
                this.ButtonPosition = this.Position * (this.sliderCanvas.ActualHeight - 16) /
                                      this.sliderCanvas.ActualHeight;
            }
        }
        #endregion
    }
}
