using System.Runtime.InteropServices;

namespace NoMouseOnlyKeyboard.WindowsAPI
{
    [StructLayout(LayoutKind.Sequential)]
    public struct INPUT
    {
        public int type;
        public MOUSEINPUT mi;
    }
}
