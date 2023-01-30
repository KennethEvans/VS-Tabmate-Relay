using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using WindowsInput;
using WindowsInput.Native;

namespace TabmateRelay {
    public static class WinUtils {
        /// <summary>
        /// The saved value of the foreground window to be restored later.
        /// </summary>
        public static IntPtr HForegroundWindow {
            get {
                return NativeMethods.GetForegroundWindow();
            }
        }

        /// <summary>
        /// Get the title for the current foreground window.
        /// </summary>
        /// <returns></returns>
        public static string GetForegroundWindowTitle() {
            IntPtr hWnd = NativeMethods.GetForegroundWindow();
            if (hWnd.Equals(IntPtr.Zero)) {
                return "<NotFound>";
            }
            return GetWindowTitle(hWnd);
        }

        /// <summary>
        /// Gets the title for the current saved window handle.
        /// </summary>
        /// <returns></returns>
        public static string GetSavedForegroundWindowTitle() {
            IntPtr hWnd = HForegroundWindow;
            if (hWnd.Equals(IntPtr.Zero)) {
                return "NotFound>";
            }
            return GetWindowTitle(hWnd);
        }

        /// <summary>
        /// Gets the title for the given window handle.
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static string GetWindowTitle(IntPtr hWnd) {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            if (NativeMethods.GetWindowText(hWnd, Buff, nChars) > 0) {
                return Buff.ToString();
            }
            return "<empty>";
        }

        /// <summary>
        /// Gets if window is topmost for the given window handle.
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static bool GetWindowIsTopmost(IntPtr hWnd) {
            IntPtr exStyle = NativeMethods.GetWindowLongPtr(hWnd,
                NativeMethods.GWL_EXSTYLE);
            return ((long)exStyle & NativeMethods.WS_EX_TOPMOST) ==
                NativeMethods.WS_EX_TOPMOST;
        }

        /// <summary>
        /// Gets a VirtualKeyCode for the given KeyConfig.
        /// </summary>
        /// <param name="keyDef">The key definition to use.</param>
        /// <returns></returns>
        public static VirtualKeyCode GetKeyCode(KeyDef keyDef) {
            VirtualKeyCode keyCode;
            if (keyDef.KeyString.Equals("^")) { // Ctrl
                keyCode = VirtualKeyCode.CONTROL;
            } else if (keyDef.KeyString.Equals("%")) { // Alt
                keyCode = VirtualKeyCode.MENU;
            } else if (keyDef.KeyString.Equals("+")) { // Shift
                keyCode = VirtualKeyCode.SHIFT;
            } else {
                throw new ArgumentException(keyDef.KeyString
                    + " is not supported");
            }
            return keyCode;
        }

        /// <summary>
        /// Sends up events for any pressed keys in the given keyConfig list.
        /// </summary>
        /// <param name="keyDefs">List of key definitions to use.</param>
        public static void SendUpEventsForPressedKeys(List<KeyDef> keyDefs) {
            if (keyDefs == null) {
                return;
            }
            foreach (KeyDef keyDef in keyDefs) {
                if (keyDef.Type == KeyDef.KeyType.HOLD) {
                    VirtualKeyCode keyCode;
                    try {
                        keyCode = WinUtils.GetKeyCode(keyDef);
                    } catch (System.ArgumentException) {
                        continue;
                    }
                    keyDef.Pressed = false;
                    var sim = new InputSimulator();
                    sim.Keyboard.KeyUp(keyCode);
                }
            }
        }

        #region DEBUG
#if DEBUG
        /// <summary>
        /// Generic debugging printout for examining foreground windows.
        /// </summary>
        /// <param name="method"></param>
        public static void DebugForegroundWindows(string method) {
            Debug.Print(method + ": Foreground: "
                + GetForegroundWindowTitle()
                + ", Saved: " + GetSavedForegroundWindowTitle());
        }

        /// <summary>
        /// Prints PortableExecutableKinds and PortableExecutableKinds
        /// information for the Manifest Module or for each module in the
        /// assembly.
        /// </summary>
        public static void PrintModuleInfo() {
            PortableExecutableKinds peKinds;
            ImageFileMachine imageFileMachine;

            var assembly = Assembly.GetExecutingAssembly();
            assembly.ManifestModule.GetPEKind(out peKinds, out imageFileMachine);
            Debug.Print("*********************************************");
            Debug.Print(assembly.ManifestModule.Name + ": " + peKinds
                + " " + imageFileMachine);
            Debug.Print("*********************************************");

#if false
            // Do this for all the modules
            var modules = assembly.GetModules();
            var kinds = new List<PortableExecutableKinds>();
            var images = new List<ImageFileMachine>();
            Debug.Print("*********************************************");
            foreach (var module in modules) {
                module.GetPEKind(out peKinds, out imageFileMachine);
                Debug.Print(module.Name + ": " + peKinds
                    + " " + imageFileMachine);

                kinds.Add(peKinds);
                images.Add(imageFileMachine);
            }
            Debug.Print("*********************************************");
            var distinctKinds = kinds.Distinct().ToList();
            var distinctImages = images.Distinct().ToList();
#endif // false
        }
#endif // Debug
        #endregion DEBUG

        /// <summary>
        /// Class for native methods.
        /// </summary>
        internal static class NativeMethods {
            internal const int WS_EX_NOACTIVATE = 0x08000000;
            internal const int GWL_EXSTYLE = -20;
            internal const long WS_EX_TOPMOST = 0x00000008;

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern int SetWindowLong(IntPtr hwnd, int index,
                int newStyle);

            [DllImport("user32.dll")]
            internal static extern IntPtr GetForegroundWindow();

            [DllImport("user32.dll")]
            internal static extern bool SetForegroundWindow(IntPtr WindowHandle);

            [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern int GetWindowText(IntPtr hWnd,
                StringBuilder lpString, int nMaxCount);

            internal static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
            internal static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
            internal static readonly IntPtr HWND_TOP = new IntPtr(0);
            internal static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

            internal const UInt32 SWP_NOSIZE = 0x0001;
            internal const UInt32 SWP_NOMOVE = 0x0002;
            internal const UInt32 SWP_SHOWWINDOW = 0x0040;

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool SetWindowPos(IntPtr hWnd,
                IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

            [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
            internal static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
            internal static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

            // This static method is required because Win32 does not support
            // GetWindowLongPtr directly
            internal static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex) {
                if (IntPtr.Size == 8)
                    return GetWindowLongPtr64(hWnd, nIndex);
                else
                    return GetWindowLongPtr32(hWnd, nIndex);
            }
        }
    }
}
