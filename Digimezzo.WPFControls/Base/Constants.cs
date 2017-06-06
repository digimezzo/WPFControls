using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Digimezzo.WPFControls.Base
{
    internal static class Constants
    {
        public static readonly Duration MouseEnterDuration = new Duration(TimeSpan.FromMilliseconds(250));
        public static readonly Duration MouseLeaveDuration = MouseEnterDuration;
        public static readonly Duration ToggleSwitchDuration = MouseEnterDuration;
        public static readonly Duration SmoothScrollingDuration = MouseEnterDuration;
    }
}
