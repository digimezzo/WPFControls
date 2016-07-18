using System.Windows;

namespace Digimezzo.WPFControls
{
    public class Windows10Window : BorderlessWindow
    {
        #region Construction
        static Windows10Window()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Windows10Window), new FrameworkPropertyMetadata(typeof(Windows10Window)));
        }
        #endregion

        #region Public
        public override void OnApplyTemplate()
        {
             base.OnApplyTemplate();
             this.TitleBarHeight = 29;
        }
        #endregion
    }
}
