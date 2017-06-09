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

        public const int UWPSliderBaseUnit = 8;
        public const int UWPSliderLengthOffset = 2 * UWPSliderBaseUnit;
        public static readonly Thickness HorizontalUWPSliderMargin = new Thickness(-UWPSliderBaseUnit, 0,0,0);
        public static readonly Thickness VerticalUWPSliderMargin = new Thickness(0,-UWPSliderBaseUnit,0,0);

    }
}
