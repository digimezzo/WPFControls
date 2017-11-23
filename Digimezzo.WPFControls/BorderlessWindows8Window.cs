using Digimezzo.WPFControls.Base;
using System.Windows;

namespace Digimezzo.WPFControls
{
    public abstract class BorderlessWindows8Window : BorderlessWindowBase
    {
        static BorderlessWindows8Window()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BorderlessWindows8Window), new FrameworkPropertyMetadata(typeof(BorderlessWindows8Window)));
        }
    }
}
