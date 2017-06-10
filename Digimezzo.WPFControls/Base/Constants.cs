using System;
using System.Windows;

namespace Digimezzo.WPFControls.Base
{
    internal static class Constants
    {
        //Common
        public static readonly Duration MouseEnterDuration = new Duration(TimeSpan.FromMilliseconds(250));
        public static readonly Duration MouseLeaveDuration = MouseEnterDuration;

        //ToggleSwitch
        public static readonly Duration ToggleSwitchDuration = MouseEnterDuration;

        //VirtualizingWrapPanel
        public static readonly Duration SmoothScrollingDuration = MouseEnterDuration;

        // UWPSlider
        public const int UWPSliderBaseUnit = 8;
        public const int UWPSliderCanvasLengthOffset = -2 * UWPSliderBaseUnit;
        public const double UWPSliderButtonSize = 2 * UWPSliderBaseUnit;
        public static readonly Thickness HorizontalUWPSliderMargin = new Thickness(-UWPSliderBaseUnit, 0, 0, 0);
        public static readonly Thickness VerticalUWPSliderMargin = new Thickness(0, 0, 0, -UWPSliderBaseUnit);

    }
}
