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
        private static long MillisecondsToTicks(int ms) => ms * 10000;

        public static Duration MouseEnterDuration = new Duration(new TimeSpan(MillisecondsToTicks(250)));
        public static Duration MouseLeaveDuration = MouseEnterDuration;
        public static Duration ToggleSwitchDuration = MouseLeaveDuration;
    }
}
