using Digimezzo.WPFControls.Base;
using System.Windows;

namespace Digimezzo.WPFControls
{
    public abstract class BorderlessWindows10Window : BorderlessWindowBase
    {
        static BorderlessWindows10Window()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BorderlessWindows10Window), new FrameworkPropertyMetadata(typeof(BorderlessWindows10Window)));
        }

        public override void OnApplyTemplate()
        {
             base.OnApplyTemplate();
             this.TitleBarHeight = 29;
        }
    }
}
