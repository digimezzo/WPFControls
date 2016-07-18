using System.Windows;
using System.Windows.Controls;

namespace Digimezzo.WPFControls
{
    public class LabelToLower : Label
    {
        #region Construction
        static LabelToLower()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LabelToLower), new FrameworkPropertyMetadata(typeof(LabelToLower)));
        }
        #endregion
    }
}
