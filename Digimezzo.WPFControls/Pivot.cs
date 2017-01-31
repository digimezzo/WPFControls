using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Digimezzo.WPFControls
{
    public class Pivot : TabControl
    {
        #region Variables
        private Border contentPanel;
        private int previous = -1;
        private int current = -1;
        #endregion

        #region Properties
        public double AnimationWidth
        {
            get { return Convert.ToDouble(GetValue(AnimationWidthProperty)); }
            set { SetValue(AnimationWidthProperty, value); }
        }

        public double AnimationDuration
        {
            get { return Convert.ToDouble(GetValue(AnimationDurationProperty)); }
            set { SetValue(AnimationDurationProperty, value); }
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty AnimationWidthProperty = DependencyProperty.Register("AnimationWidth", typeof(double), typeof(Pivot), new PropertyMetadata(20.0));
        public static readonly DependencyProperty AnimationDurationProperty = DependencyProperty.Register("AnimationDuration", typeof(double), typeof(Pivot), new PropertyMetadata(0.3));
        #endregion

        #region Construction
        static Pivot()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Pivot), new FrameworkPropertyMetadata(typeof(Pivot)));
        }
        #endregion

        #region Overrides
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.contentPanel = (Border)GetTemplateChild("contentPanel");

            if (this.contentPanel != null)
            {
                this.SelectionChanged += Pivot_SelectionChanged;
            }
        }
        #endregion

        #region Event handlers
        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            current = (sender as TabControl).SelectedIndex;
            if (previous != current)
            {
                DoubleAnimation xTranslate = null;

                if (previous > current)
                {
                    xTranslate = new DoubleAnimation() { From = -this.AnimationWidth, To = 0.0, Duration = TimeSpan.FromSeconds(this.AnimationDuration) };
                }
                else
                {
                    xTranslate = new DoubleAnimation() { From = this.AnimationWidth, To = 0.0, Duration = TimeSpan.FromSeconds(this.AnimationDuration) };
                }

                TranslateTransform translateTransform1 = new TranslateTransform();
                translateTransform1.BeginAnimation(TranslateTransform.XProperty, xTranslate);
                contentPanel.RenderTransform = translateTransform1;
                previous = current;
            }
        }
        #endregion
    }

    public class PivotItem : TabItem
    {
        #region Properties
        public Brush SelectedForeground
        {
            get { return (Brush)GetValue(SelectedForegroundProperty); }
            set { SetValue(SelectedForegroundProperty, value); }
        }

        public FontWeight SelectedFontWeight
        {
            get { return (FontWeight)GetValue(SelectedFontWeightProperty); }
            set { SetValue(SelectedFontWeightProperty, value); }
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty SelectedForegroundProperty = DependencyProperty.Register("SelectedForeground", typeof(Brush), typeof(PivotItem), new PropertyMetadata(Brushes.Green));
        public static readonly DependencyProperty SelectedFontWeightProperty = DependencyProperty.Register("SelectedFontWeight", typeof(FontWeight), typeof(PivotItem), new PropertyMetadata(FontWeights.Normal));
        #endregion

        #region Construction
        static PivotItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PivotItem), new FrameworkPropertyMetadata(typeof(PivotItem)));
        }
        #endregion
    }
}
