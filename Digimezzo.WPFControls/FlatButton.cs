using System.Windows;
using System.Windows.Controls;

namespace Digimezzo.WPFControls
{
    public class FlatButton : Button
    {
        #region Construction
        static FlatButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlatButton), new FrameworkPropertyMetadata(typeof(FlatButton)));
        }
        #endregion
    }
}
