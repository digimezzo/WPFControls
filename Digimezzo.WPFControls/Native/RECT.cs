using System.Runtime.InteropServices;

namespace Digimezzo.WPFControls.Native
{
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
}