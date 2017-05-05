using System.Runtime.InteropServices;

namespace Digimezzo.WPFControls.Native
{
    public static partial class NativeMethods
    {
        [DllImport("shell32.dll")]
        internal static extern uint SHAppBarMessage(int dwMessage, ref APPBARDATA pData);
    }
}