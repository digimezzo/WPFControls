using System;
using System.Runtime.InteropServices;

namespace Digimezzo.WPFControls.Native
{
    public static partial class NativeMethods
    {
        [DllImport("user32.dll", EntryPoint = "GetMonitorInfoW", ExactSpelling = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, [Out] MONITORINFO lpmi);

        [DllImport("user32.dll")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, uint flags);
    }
}