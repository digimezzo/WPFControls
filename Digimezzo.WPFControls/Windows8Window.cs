using System.Windows;

namespace Digimezzo.WPFControls
{
    public class Windows8Window : BorderlessWindow
    {
        #region Construction
        static Windows8Window()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Windows8Window), new FrameworkPropertyMetadata(typeof(Windows8Window)));
        }
        #endregion
    }
}
