using System.Runtime.InteropServices;

using KeyEventArgs = InputRecorder.Models.Keyboard.KeyEventArgs;

namespace InputRecorder.Hooks
{
    /// <summary>
    /// Handles keyboard hooks and raises events for key presses and releases.
    /// </summary>
    internal class KeyboardHook
    {
        /// <summary>
        /// Event triggered when a key is pressed or released.
        /// </summary>
        public event EventHandler<KeyEventArgs>? KeyPressed;

        private const int WH_KEYBOARD_LL = 13; // Low-level keyboard hook constant
        private const int WM_KEYDOWN = 0x0100; // WM_KEYDOWN message constant
        private const int WM_KEYUP = 0x0101;   // WM_KEYUP message constant
        private LowLevelKeyboardProc _proc;    // Delegate for the hook callback
        private IntPtr _hookID = IntPtr.Zero;  // Hook ID for unhooking

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardHook"/> class and sets the hook.
        /// </summary>
        public KeyboardHook()
        {
            _proc = HookCallback;
            _hookID = SetHook(_proc);
        }

        /// <summary>
        /// Finalizer to unhook the keyboard hook when the object is destroyed.
        /// </summary>
        ~KeyboardHook()
        {
            UnhookWindowsHookEx(_hookID);
        }

        /// <summary>
        /// Sets the low-level keyboard hook.
        /// </summary>
        /// <param name="proc">The callback method for the hook.</param>
        /// <returns>The handle to the hook.</returns>
        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (var curProcess = System.Diagnostics.Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                if (curModule == null)
                {
                    throw new InvalidOperationException("Unable to get the current module.");
                }

                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        /// <summary>
        /// Delegate for the low-level keyboard hook procedure.
        /// </summary>
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Callback method for the hook procedure.
        /// </summary>
        /// <param name="nCode">The hook code.</param>
        /// <param name="wParam">The message identifier.</param>
        /// <param name="lParam">Additional message information.</param>
        /// <returns>The result of the next hook procedure.</returns>
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_KEYUP))
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;
                bool isKeyDown = wParam == (IntPtr)WM_KEYDOWN;
                KeyPressed?.Invoke(this, new KeyEventArgs(key, isKeyDown));
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn,
            IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}