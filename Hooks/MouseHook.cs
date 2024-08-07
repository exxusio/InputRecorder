using System.Runtime.InteropServices;
using InputRecorder.Models.Mouse;

using MouseEventArgs = InputRecorder.Models.Mouse.MouseEventArgs;

namespace InputRecorder.Hooks
{
    /// <summary>
    /// Handles mouse hooks and raises events for mouse actions such as clicks and movements.
    /// </summary>
    internal class MouseHook
    {
        /// <summary>
        /// Event triggered when a mouse action occurs (click or movement).
        /// </summary>
        public event EventHandler<MouseEventArgs>? MouseActionOccurred;

        private const int WH_MOUSE_LL = 14; // Low-level mouse hook constant
        private const int WM_LBUTTONDOWN = 0x0201; // WM_LBUTTONDOWN message constant
        private const int WM_LBUTTONUP = 0x0202;   // WM_LBUTTONUP message constant
        private const int WM_RBUTTONDOWN = 0x0204; // WM_RBUTTONDOWN message constant
        private const int WM_RBUTTONUP = 0x0205;   // WM_RBUTTONUP message constant
        private const int WM_MOUSEMOVE = 0x0200;   // WM_MOUSEMOVE message constant
        private LowLevelMouseProc _proc;           // Delegate for the hook callback
        private IntPtr _hookID = IntPtr.Zero;     // Hook ID for unhooking

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseHook"/> class and sets the hook.
        /// </summary>
        public MouseHook()
        {
            _proc = HookCallback;
            _hookID = SetHook(_proc);
        }

        /// <summary>
        /// Finalizer to unhook the mouse hook when the object is destroyed.
        /// </summary>
        ~MouseHook()
        {
            UnhookWindowsHookEx(_hookID);
        }

        /// <summary>
        /// Sets the low-level mouse hook.
        /// </summary>
        /// <param name="proc">The callback method for the hook.</param>
        /// <returns>The handle to the hook.</returns>
        private IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (var curProcess = System.Diagnostics.Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                if (curModule == null)
                {
                    throw new InvalidOperationException("Unable to get the current module.");
                }

                return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        /// <summary>
        /// Delegate for the low-level mouse hook procedure.
        /// </summary>
        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Callback method for the hook procedure.
        /// </summary>
        /// <param name="nCode">The hook code.</param>
        /// <param name="wParam">The message identifier.</param>
        /// <param name="lParam">Additional message information.</param>
        /// <returns>The result of the next hook procedure.</returns>
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var mouseStruct = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
                MouseEventType eventType;

                switch (wParam.ToInt32())
                {
                    case WM_LBUTTONDOWN:
                    case WM_LBUTTONUP:
                        eventType = MouseEventType.ClickLeft;
                        break;
                    case WM_RBUTTONDOWN:
                    case WM_RBUTTONUP:
                        eventType = MouseEventType.ClickRight;
                        break;
                    case WM_MOUSEMOVE:
                        eventType = MouseEventType.Move;
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine($"Unknown mouse event type: {wParam.ToInt32()}");
                        return CallNextHookEx(_hookID, nCode, wParam, lParam);
                }

                bool isDown = wParam == (IntPtr)WM_LBUTTONDOWN || wParam == (IntPtr)WM_RBUTTONDOWN;
                MouseActionOccurred?.Invoke(this, new MouseEventArgs(eventType, new Point(mouseStruct.pt.X, mouseStruct.pt.Y), isDown));
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public Point pt; // Mouse cursor position
            public int mouseData; // Mouse data
            public int flags; // Hook flags
            public int time; // Time of the event
            public IntPtr dwExtraInfo; // Extra information
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}