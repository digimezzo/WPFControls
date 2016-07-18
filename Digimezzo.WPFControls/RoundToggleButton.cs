using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Digimezzo.WPFControls
{
    public class RoundToggleButton : ToggleButton
    {
        #region Properties
        public Brush Accent
        {
            get { return (Brush)GetValue(AccentProperty); }
            set { SetValue(AccentProperty, value); }
        }

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
        public static readonly DependencyProperty AccentProperty = DependencyProperty.Register("Accent", typeof(Brush), typeof(RoundToggleButton), new PropertyMetadata(null));
        public static readonly DependencyProperty ContentWidthProperty = DependencyProperty.Register("ContentWidth", typeof(double), typeof(RoundToggleButton), new PropertyMetadata(null));
        public static readonly DependencyProperty ContentHeightProperty = DependencyProperty.Register("ContentHeight", typeof(double), typeof(RoundToggleButton), new PropertyMetadata(null));
        public static readonly DependencyProperty ContentMarginProperty = DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(RoundToggleButton), new PropertyMetadata(null));
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(Geometry), typeof(RoundToggleButton), new PropertyMetadata(null));
        public static readonly DependencyProperty ShowCircleProperty = DependencyProperty.Register("ShowCircle", typeof(bool), typeof(RoundToggleButton), new PropertyMetadata(true));
        #endregion

        #region Construction
        static RoundToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RoundToggleButton), new FrameworkPropertyMetadata(typeof(RoundToggleButton)));
        }
        #endregion

        #region Public
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.SizeChanged += RoundToggleButton_SizeChanged;
        }
        #endregion

        #region Private
        private void RoundToggleButton_SizeChanged(object sender, SizeChangedEventArgs e)
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