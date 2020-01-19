using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.ComponentModel;
using System.Timers;
using System.Windows;

namespace WinLib
{
    public class MouseInput : INotifyPropertyChanged
    {
        public System.Drawing.Point mousePoint;
        public System.Drawing.Point previousMousePoint;
        public ObservableCollection<string> mouseHistory { get; set; }

        public int tick;
        private Timer timer;

        public IntPtr targetHWND { get; set; }
        public bool supressNext { get; set; }

        public string mouseString
        {
            get { return _mouseString; }
            set
            {
                _mouseString = value;
                OnPropertyChanged("mouseString");
            }
        }
        private string _mouseString;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string _mouseString)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_mouseString));
        }

        public void SimulateLeftClick()
        {
            GetMousePoint();
            SimulateLeftClick(mousePoint.X, mousePoint.Y);
        }
        public void SimulateLeftClick(int X, int Y)
        {
            mouse_event((int)MouseEventFlags.LeftDown, X, Y, 0, 0);
        }

        public void SimulateLeftUnclick()
        {
            GetMousePoint();
            SimulateLeftUnclick(mousePoint.X, mousePoint.Y);
        }
        public void SimulateLeftUnclick(int X, int Y)
        {
            mouse_event((int)MouseEventFlags.LeftUp, X, Y, 0, 0);
        }

        public void SimulateRightClick()
        {
            GetMousePoint();
            SimulateRightClick(mousePoint.X, mousePoint.Y);
        }
        public void SimulateRightClick(int X, int Y)
        {
            mouse_event((int)MouseEventFlags.RightDown, X, Y, 0, 0);
        }

        public void GetMousePoint()
        {
            previousMousePoint = mousePoint;
            mousePoint = GetCursorPosition();
            if (mousePoint != previousMousePoint)
            {
                mouseString = "Mouse is at: " + mousePoint.ToString();
                //mouseHistory.Add(mouseString);
                //mouseHistory.Insert(0, mouseString);
            }
        }
        public System.Drawing.Point GetCursorPosition()
        {
            System.Drawing.Point lpPoint = new System.Drawing.Point();
            GetCursorPos(out lpPoint);

            return lpPoint;
        }

        /// <summary>
        /// Internal callback processing function
        /// </summary>
        private delegate IntPtr MouseHookHandler(int nCode, IntPtr wParam, IntPtr lParam);
        private MouseHookHandler hookHandler;

        /// <summary>
        /// Function to be called when defined event occurs
        /// </summary>
        /// <param name="mouseStruct">MSLLHOOKSTRUCT mouse structure</param>
        public delegate void MouseHookCallback(MSLLHOOKSTRUCT mouseStruct);

        #region Events
        public event MouseHookCallback LeftButtonDown;
        public event MouseHookCallback LeftButtonUp;
        public event MouseHookCallback RightButtonDown;
        public event MouseHookCallback RightButtonUp;
        public event MouseHookCallback MouseMove;
        public event MouseHookCallback MouseWheel;
        public event MouseHookCallback DoubleClick;
        public event MouseHookCallback MiddleButtonDown;
        public event MouseHookCallback MiddleButtonUp;
        #endregion

        /// <summary>
        /// Low level mouse hook's ID
        /// </summary>
        private IntPtr hookID = IntPtr.Zero;

        /// <summary>
        /// Install low level mouse hook
        /// </summary>
        /// <param name="mouseHookCallbackFunc">Callback function</param>
        public void Install()
        {
            hookHandler = HookFunc;
            hookID = SetHook(hookHandler);

            if (mouseHistory == null)
            {
                mouseHistory = new ObservableCollection<string>();
            }

            supressNext = false;
        }

        /// <summary>
        /// Remove low level mouse hook
        /// </summary>
        public void Uninstall()
        {
            if (hookID == IntPtr.Zero)
                return;

            UnhookWindowsHookEx(hookID);
            hookID = IntPtr.Zero;
        }

        /// <summary>
        /// Destructor. Unhook current hook
        /// </summary>
        ~MouseInput()
        {
            Uninstall();
        }

        /// <summary>
        /// Sets hook and assigns its ID for tracking
        /// </summary>
        /// <param name="proc">Internal callback function</param>
        /// <returns>Hook ID</returns>
        private IntPtr SetHook(MouseHookHandler proc)
        {
            using (ProcessModule module = Process.GetCurrentProcess().MainModule)
                return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(module.ModuleName), 0);
        }

        /// <summary>
        /// Callback function
        /// </summary>
        private IntPtr HookFunc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            // parse system messages
            if (nCode >= 0)
            {
                if (MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)
                    if (supressNext == true)
                    {
                        WindowHelper.SetForegroundWindow(targetHWND);
                        supressNext = false;
                        return new IntPtr(1);
                    }
                    else
                    {
                        if (LeftButtonDown != null)
                            LeftButtonDown((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
                        if (MouseMessages.WM_LBUTTONUP == (MouseMessages)wParam)
                            if (LeftButtonUp != null)
                                LeftButtonUp((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
                        if (MouseMessages.WM_RBUTTONDOWN == (MouseMessages)wParam)
                            if (RightButtonDown != null)
                                RightButtonDown((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
                        if (MouseMessages.WM_RBUTTONUP == (MouseMessages)wParam)
                            if (RightButtonUp != null)
                                RightButtonUp((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
                        if (MouseMessages.WM_MOUSEMOVE == (MouseMessages)wParam)
                            if (MouseMove != null)
                                MouseMove((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
                        if (MouseMessages.WM_MOUSEWHEEL == (MouseMessages)wParam)
                            if (MouseWheel != null)
                                MouseWheel((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
                        if (MouseMessages.WM_LBUTTONDBLCLK == (MouseMessages)wParam)
                            if (DoubleClick != null)
                                DoubleClick((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
                        if (MouseMessages.WM_MBUTTONDOWN == (MouseMessages)wParam)
                            if (MiddleButtonDown != null)
                                MiddleButtonDown((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
                        if (MouseMessages.WM_MBUTTONUP == (MouseMessages)wParam)
                            if (MiddleButtonUp != null)
                                MiddleButtonUp((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
                    }
                
            }
            return CallNextHookEx(hookID, nCode, wParam, lParam);    
        }

        #region WinAPI
        private const int WH_MOUSE_LL = 14;

        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205,
            WM_LBUTTONDBLCLK = 0x0203,
            WM_MBUTTONDOWN = 0x0207,
            WM_MBUTTONUP = 0x0208
        }

        private enum MouseEventFlags
        {
            LeftDown = 0x00000002,
            LeftUp = 0x00000004,
            MiddleDown = 0x00000020,
            MiddleUp = 0x00000040,
            Move = 0x00000001,
            Absolute = 0x00008000,
            RightDown = 0x00000008,
            RightUp = 0x00000010
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            MouseHookHandler lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out System.Drawing.Point lpPoint);

        #endregion
    }
}
