using System.Windows;
using System.Windows.Controls;

namespace Digimezzo.WPFControls
{
    public class LabelToLower : Label
    {
        static LabelToLower()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LabelToLower), new FrameworkPropertyMetadata(typeof(LabelToLower)));
        }
    }
}
