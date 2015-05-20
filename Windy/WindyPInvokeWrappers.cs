using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Windy
{
    internal static class WindyPInvokeWrappers
    {
        [DllImport("user32")]
        private static extern bool ShowWindowAsync(IntPtr HWnd, int nCmdShow);

        [DllImport("user32")]
        private static extern bool SetForegroundWindow(IntPtr HWnd);

        public const int SW_RESTORE = 9;

        public static void FocusWindow(Window window)
        {
            ShowWindowAsync(window.HWnd, SW_RESTORE);
            SetForegroundWindow(window.HWnd);
        }
    }
}
