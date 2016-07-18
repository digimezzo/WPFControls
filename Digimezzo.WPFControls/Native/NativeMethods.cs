using System;
using System.Runtime.InteropServices;

namespace Digimezzo.WPFControls.Native
{
    public static class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    }
}
