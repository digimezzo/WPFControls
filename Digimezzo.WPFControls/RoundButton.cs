using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Digimezzo.WPFControls
{
    public class RoundButton : Button
    {
        #region Properties
        public double ContentWidth
        {
            get { return Convert.ToDouble(GetValue(ContentWidthProperty)); }
            set { SetValue(ContentWidthProperty, value); }
        }

        public double ContentHeight
        {
            get { return Convert.ToDouble(GetValue(ContentHeightProperty)); }
            set { SetValue(ContentHeightProperty, value); }
        }

        public Thickness ContentMargin
        {
            get { return (Thickness)GetValue(ContentMarginProperty); }
            set { SetValue(ContentMarginProperty, value); }
        }

        public Geometry Data
        {
            get { return (Geometry)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public bool ShowCircle
        {
            get { return Convert.ToBoolean(GetValue(ShowCircleProperty)); }
            set { SetValue(ShowCircleProperty, value); }
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty ContentWidthProperty = DependencyProperty.Register("ContentWidth", typeof(double), typeof(RoundButton), new PropertyMetadata(null));
        public static readonly DependencyProperty ContentHeightProperty = DependencyProperty.Register("ContentHeight", typeof(double), typeof(RoundButton), new PropertyMetadata(null));
        public static readonly DependencyProperty ContentMarginProperty = DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(RoundButton), new PropertyMetadata(null));
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(Geometry), typeof(RoundButton), new PropertyMetadata(null));
        public static readonly DependencyProperty ShowCircleProperty = DependencyProperty.Register("ShowCircle", typeof(bool), typeof(RoundButton), new PropertyMetadata(true));
        #endregion

        #region Construction
        static RoundButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RoundButton), new FrameworkPropertyMetadata(typeof(RoundButton)));
        }
        #endregion

        #region Overrides
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.SizeChanged += RoundButton_SizeChanged;
        }
        #endregion

        #region Event Handlers
        private void RoundButton_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualWidth > 0.0)
            {
                if (this.ShowCircle)
                {
                    this.ContentWidth = this.ActualWidth * 0.35;
                    this.ContentHeight = this.ActualWidth * 0.35;
                }
                else
                {
                    this.ContentWidth = this.ActualWidth;
                    this.ContentHeight = this.ActualWidth;
                }
            }
        }
        #endregion
    }
}