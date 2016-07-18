using System.Windows;
using System.Windows.Controls;

namespace Digimezzo.WPFControls
{
    public class LabelToUpper : Label
    {
        #region Construction
        static LabelToUpper()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LabelToUpper), new FrameworkPropertyMetadata(typeof(LabelToUpper)));
        }
        #endregion
    }
}
