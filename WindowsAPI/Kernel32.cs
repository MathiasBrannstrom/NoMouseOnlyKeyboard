using System.Runtime.InteropServices;

namespace NoMouseOnlyKeyboard.WindowsAPI
{
    public static class Kernel32
    {
        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();
    }
}
