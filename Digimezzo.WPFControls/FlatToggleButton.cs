using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Digimezzo.WPFControls
{
    public class FlatToggleButton : ToggleButton
    {
        #region Properties
        public Geometry UnCheckedData
        {
            get { return (Geometry)GetValue(UnCheckedDataProperty); }
            set { SetValue(UnCheckedDataProperty, value); }
        }

        public Geometry CheckedData
        {
            get { return (Geometry)GetValue(CheckedDataProperty); }
            set { SetValue(CheckedDataProperty, value); }
        }

        public Brush UnCheckedForeground
        {
            get { return (Brush)GetValue(UnCheckedForegroundProperty); }
            set { SetValue(UnCheckedForegroundProperty, value); }
        }

        public Brush CheckedForeground
        {
            get { return (Brush)GetValue(CheckedForegroundProperty); }
            set { SetValue(CheckedForegroundProperty, value); }
        }

        public string UnCheckedToolTip
        {
            get { return Convert.ToString(GetValue(UnCheckedToolTipProperty)); }
            set { SetValue(UnCheckedToolTipProperty, value); }
        }

        public string CheckedToolTip
        {
            get { return Convert.ToString(GetValue(CheckedToolTipProperty)); }
            set { SetValue(CheckedToolTipProperty, value); }
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty UnCheckedDataProperty = DependencyProperty.Register("UnCheckedData", typeof(Geometry), typeof(FlatToggleButton), new PropertyMetadata(null));
        public static readonly DependencyProperty CheckedDataProperty = DependencyProperty.Register("CheckedData", typeof(Geometry), typeof(FlatToggleButton), new PropertyMetadata(null));
        public static readonly DependencyProperty UnCheckedForegroundProperty = DependencyProperty.Register("UnCheckedForeground", typeof(Brush), typeof(FlatToggleButton), new PropertyMetadata(null));
        public static readonly DependencyProperty CheckedForegroundProperty = DependencyProperty.Register("CheckedForeground", typeof(Brush), typeof(FlatToggleButton), new PropertyMetadata(null));
        public static readonly DependencyProperty UnCheckedToolTipProperty = DependencyProperty.Register("UnCheckedToolTip", typeof(string), typeof(FlatToggleButton), new PropertyMetadata(null));
        public static readonly DependencyProperty CheckedToolTipProperty = DependencyProperty.Register("CheckedToolTip", typeof(string), typeof(FlatToggleButton), new PropertyMetadata(null));
        #endregion

        #region Construction
        static FlatToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlatToggleButton), new FrameworkPropertyMetadata(typeof(FlatToggleButton)));
        }
        #endregion

        #region Overrides
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
        #endregion
    }
}