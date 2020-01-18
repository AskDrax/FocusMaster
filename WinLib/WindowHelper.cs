using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Diagnostics;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Automation;
using System.Windows.Threading;

namespace WinLib
{
    public static class WindowHelper
    { 
        public static ObservableCollection<AWindow> windowList { get; set; }

        public static IntPtr DesktopWindowHandle;
        public static Window mainWindow = Application.Current.MainWindow;

        public delegate bool EnumDelegate(IntPtr hWnd, int lParam);

        public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        public static void GetAllWindows()
        {
            windowList.Clear();
            DesktopWindowHandle = GetDesktopWindow();

            Process[] processList = Process.GetProcesses();
            foreach (Process process in processList)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    windowList.Add(GetAWINDOW(process.MainWindowHandle));
                }
            }
        }

        public static void EnumAllWindows()
        {
            windowList.Clear();
            DesktopWindowHandle = GetDesktopWindow();

            EnumDelegate filter = delegate (IntPtr hWnd, int lParam)
            {
                AWindow awin = GetAWINDOW(hWnd);
                if (IsWindowVisible(hWnd) && string.IsNullOrEmpty(awin.title) == false)
                {
                    AddAWindow(awin);
                }

                return true;
            };

            if (EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero))
            {
                //success
            }
        }
        public static void AddAWindow(AWindow awin)
        {
            if (string.IsNullOrEmpty(awin.title) == false)
            {
                if (awin.title == "SET_WINDOW") return;
                if (awin.title == "Program Manager") return;
                windowList.Add(awin);

                //infoString = "Added Window: " + awin.title + " (" + awin.hwnd.ToString() + ")";
            }
        }

        public static void RemoveAWindow(IntPtr hwnd)
        {
            foreach (AWindow awin in windowList)
            {
                if (awin.hwnd == hwnd)
                {
                    windowList.Remove(awin);
                    //infoString = "Removed Window: " + awin.title + " (" + hwnd.ToString() + ")";
                    break;
                }
            }
        }

        public static AWindow GetAWINDOW(IntPtr hWnd)
        {
            /*
            AWINDOW awin = windowList.FirstOrDefault(wind => wind.hwnd == hWnd);

            Guid guid;

            if (awin != null && awin.GUID != null)
            {
                guid = awin.GUID;
            }
            else
            {
                guid = Guid.NewGuid();
            }
            */

            Guid guid = Guid.NewGuid();

            AWindow theWINDOW = new AWindow
            {
                GUID = guid,
                hwnd = hWnd,
                title = GetTitleOfWindow(hWnd),
                className = GetClassNameOfWindow(hWnd),
                baseClassName = GetBaseClassNameOfWindow(hWnd),
                savedParent = GetParent(hWnd),
                savedStyle = SaveWindowStyle(hWnd),
                automationElement = GetAutomationElementOfWindow(hWnd),
                savedRECT = SaveWindowRect(hWnd),
                niceRECT = SaveNiceWindowRect(hWnd),
                borderRECT = new RECT(),
                minRECT = new RECT(),
                icon = GetAppIcon(hWnd)
            };

            theWINDOW.borderRECT = GetWindowBorder(hWnd);

            WINDOWPLACEMENT place = new WINDOWPLACEMENT();
            GetWindowPlacement(hWnd, ref place);
            theWINDOW.placement = place;

            theWINDOW.iconImage = ToImageSource(theWINDOW.icon);

            WINDOWINFO info = new WINDOWINFO();
            info.cbSize = (uint)Marshal.SizeOf(info);
            bool gotInfo = GetWindowInfo(hWnd, ref info);
            theWINDOW.info = info;

            uint threadId;
            GetWindowThreadProcessId(hWnd, out threadId);
            theWINDOW.threadProcessId = threadId;

            GUITHREADINFO guiInfo = GetWindowGUIThreadInfo(hWnd);
            theWINDOW.guiThreadInfo = guiInfo;

            return theWINDOW;
        }

        public static AutomationElement GetAutomationElementOfWindow(IntPtr hwnd)
        {
            AutomationElement element = AutomationElement.FromHandle(hwnd);
            return element;
        }

        public static void ListenForWindowOpen()
        {
            var desktopElement = AutomationElement.FromHandle(DesktopWindowHandle);
            Automation.AddAutomationEventHandler(WindowPattern.WindowOpenedEvent, desktopElement,
                TreeScope.Subtree, (s1, e1) =>
                {
                    var element = s1 as AutomationElement;

                    if (element.Current.Name != null)
                    {
                        IntPtr handle = (IntPtr)element.Current.NativeWindowHandle;
                        //string name = element.Current.Name;

                        mainWindow.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            AddAWindow(GetAWINDOW(handle));
                        }));
                    }
                });
        }

        public static void ListenForWindowClose()
        {
            var desktopElement = AutomationElement.FromHandle(DesktopWindowHandle);
            Automation.AddAutomationEventHandler(WindowPattern.WindowClosedEvent, desktopElement,
                TreeScope.Subtree, (s1, e1) =>
                {
                    WindowClosedEventArgs windowEventArgs = (WindowClosedEventArgs)e1;
                    int[] runtimeIdentifiers = windowEventArgs.GetRuntimeId();
                    IntPtr handle = WindowListed(runtimeIdentifiers);

                    if (handle != IntPtr.Zero)
                    {
                        mainWindow.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            RemoveAWindow(handle);
                        }));

                    }
                });
        }

        private static IntPtr WindowListed(int[] runtimeId)
        {
            for (int i = 0; i < windowList.Count; i++)
            {
                int[] listedId = windowList[i].automationElement.GetRuntimeId();
                if (Automation.Compare(listedId, runtimeId))
                {
                    return windowList[i].hwnd;
                }
            }
            return IntPtr.Zero;
        }

        public static void CloseAutomationEvents()
        {
            Automation.RemoveAllEventHandlers();
        }

        public static void FixWindowStyle(IntPtr hwnd)
        {
            IntPtr myStyle = new IntPtr(WS.WS_CAPTION | WS.WS_MINIMIZEBOX | WS.WS_MAXIMIZEBOX | WS.WS_SYSMENU | WS.WS_SIZEBOX);
            SetWindowLongPtr(new HandleRef(null, hwnd), GWL.GWL_STYLE, myStyle);
        }

        public static void MakeTransparentStyle(IntPtr hwnd)
        {
            IntPtr extendedStyle = GetWindowLongPtr(hwnd, GWL.GWL_EXSTYLE);
            SetWindowLongPtr(new HandleRef(null, hwnd), GWL.GWL_EXSTYLE, new IntPtr((uint)extendedStyle | WS_EX.WS_EX_TRANSPARENT));
        }

        public static IntPtr SaveWindowStyle(IntPtr hwnd)
        {
            IntPtr savedStyle = WindowHelper.GetWindowLongPtr(hwnd, GWL.GWL_STYLE);
            return savedStyle;
        }

        public static void SetWindowStyle(IntPtr hwnd, IntPtr style)
        {
            SetWindowLongPtr(new HandleRef(null, hwnd), GWL.GWL_STYLE, style);
        }

        public static void GetNiceWindowRect(IntPtr hwnd, ref RECT ActiveRect)
        {
            if (DWMHelper.DwmGetWindowAttribute(hwnd, DWMHelper.DWMWINDOWATTRIBUTE.ExtendedFrameBounds, out ActiveRect, Marshal.SizeOf(typeof(RECT))) != 0)
            {
                GetWindowRect(hwnd, ref ActiveRect);
            }
        }

        public static RECT GetWindowBorder(IntPtr hwnd)
        {
            RECT baseRect = new RECT();
            RECT niceRect = new RECT();
            RECT borderRect = new RECT();

            GetWindowRect(hwnd, ref baseRect);
            GetNiceWindowRect(hwnd, ref niceRect);

            borderRect.Left = niceRect.Left - baseRect.Left;
            borderRect.Top = niceRect.Top - baseRect.Top;
            borderRect.Right = baseRect.Right - niceRect.Right;
            borderRect.Bottom = baseRect.Bottom - niceRect.Bottom;

            return borderRect;
        }

        public static RECT GetWindowMinimum(IntPtr hwnd)
        {
            RECT oldRECT = new RECT();
            oldRECT = SaveWindowRect(hwnd);

            SetWindowPos(hwnd,
                        IntPtr.Zero,
                        0, 0,
                        0, 0,
                        SWP.SWP_NOACTIVATE | SWP.SWP_NOZORDER
                        );

            RECT minRECT = new RECT();

            //GetWindowRect(hwnd, ref minRECT);
            GetNiceWindowRect(hwnd, ref minRECT);

            SetWindowPos(hwnd,
                IntPtr.Zero,
                oldRECT.Left, oldRECT.Top,
                (oldRECT.Right - oldRECT.Left), (oldRECT.Bottom - oldRECT.Top),
                SWP.SWP_NOACTIVATE | SWP.SWP_NOZORDER
                );

            return minRECT;
        }

        public static RECT SaveWindowRect(IntPtr hwnd)
        {
            RECT SavedRECT = new RECT();
            GetWindowRect(hwnd, ref SavedRECT);
            //WindowHelper.getNiceWindowRect(awin.hwnd, ref savedRECT);
            return SavedRECT;
        }

        public static RECT SaveNiceWindowRect(IntPtr hwnd)
        {
            RECT niceRect = new RECT();
            GetNiceWindowRect(hwnd, ref niceRect);
            return niceRect;
        }

        public static void RestoreWindow(AWindow awin)
        {
            int oldX = awin.savedRECT.Left;
            int oldY = awin.savedRECT.Top;
            int oldWidth = awin.savedRECT.Right - awin.savedRECT.Left;
            int oldHeight = awin.savedRECT.Bottom - awin.savedRECT.Top;

            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
            placement = awin.placement;
            SetWindowPlacement(awin.hwnd, ref placement);
            SetWindowPos(awin.hwnd,
                         WinLib.HWND.HWND_BOTTOM,
                         oldX, oldY,
                         oldWidth, oldHeight,
                         WinLib.SWP.SWP_NOOWNERZORDER);
        }

        public static IntPtr GetParentWindow(IntPtr hwnd)
        {
            IntPtr current = WindowHelper.GetParent(hwnd);

            if (current.ToInt64() > 0)
            {
                return GetParentWindow(current);
            }
            return hwnd;
        }

        public static IntPtr GetWindowUnderCursor()
        {
            System.Drawing.Point cursorPoint = new System.Drawing.Point();
            cursorPoint = GetCursorPosition();
            IntPtr resultHWND = WindowFromPoint(cursorPoint);

            return resultHWND;
        }

        public static IntPtr GetNextWindowUnderPoint(System.Drawing.Point point, IntPtr hwnd)
        {
            IntPtr current = WindowHelper.GetWindow(hwnd, GW.GW_HWNDNEXT);

            if (current.ToInt64() > 0)
            {
                RECT currentRECT = new RECT();
                GetWindowRect(current, ref currentRECT);

                if (IsInRECT(point, currentRECT))
                {
                    return current;
                }
                else
                {
                    return GetNextWindowUnderPoint(point, current);
                }
            }
            else
            {
                return IntPtr.Zero;
            }
        }

        public static string GetTitleOfWindow(IntPtr hwnd)
        {
            string title = "";
            StringBuilder windowText = null;

            try
            {
                int maxLength = GetWindowTextLength(hwnd);
                windowText = new StringBuilder("", maxLength + 5);
                GetWindowText(hwnd, windowText, maxLength + 2);

                if (!String.IsNullOrEmpty(windowText.ToString()) && !String.IsNullOrWhiteSpace(windowText.ToString()))
                {
                    title = windowText.ToString();
                }
            }
            catch (Exception ex)
            {
                title = ex.Message;
            }
            finally
            {
                windowText = null;
            }
            return title;
        }

        public static string GetClassNameOfWindow(IntPtr hwnd)
        {
            string className = "";
            StringBuilder classText = null;

            try
            {
                int maxLength = 1000;
                classText = new StringBuilder("", maxLength + 5);
                GetClassName(hwnd, classText, maxLength + 2);

                if (!String.IsNullOrEmpty(classText.ToString()) && !String.IsNullOrWhiteSpace(classText.ToString()))
                {
                    className = classText.ToString();
                }
            }
            catch (Exception ex)
            {
                className = ex.Message;
            }
            finally
            {
                classText = null;
            }
            return className;
        }

        public static string GetBaseClassNameOfWindow(IntPtr hwnd)
        {
            string baseClassName = "";
            StringBuilder baseClassText = null;

            try
            {
                int maxLength = 1000;
                baseClassText = new StringBuilder("", maxLength + 5);
                RealGetWindowClass(hwnd, baseClassText, maxLength + 2);

                if (!String.IsNullOrEmpty(baseClassText.ToString()) && !String.IsNullOrWhiteSpace(baseClassText.ToString()))
                {
                    baseClassName = baseClassText.ToString();
                }
            }
            catch (Exception ex)
            {
                baseClassName = ex.Message;
            }
            finally
            {
                baseClassText = null;
            }
            return baseClassName;
        }

        public static GUITHREADINFO GetWindowGUIThreadInfo(IntPtr hwnd)
        {
            uint processId;
            GetWindowThreadProcessId(hwnd, out processId);

            GUITHREADINFO guithreadinfo = new GUITHREADINFO(true);
            GetGUIThreadInfo(processId, out guithreadinfo);

            return guithreadinfo;
        }

        public static Icon GetAppIcon(IntPtr hwnd)
        {
            IntPtr iconHandle = SendMessage(hwnd, ICON.WM_GETICON, ICON.ICON_BIG, 0);

            if (iconHandle == IntPtr.Zero)
                iconHandle = SendMessage(hwnd, ICON.WM_GETICON, ICON.ICON_SMALL, 0);

            if (iconHandle == IntPtr.Zero)
                iconHandle = SendMessage(hwnd, ICON.WM_GETICON, ICON.ICON_SMALL2, 0);

            if (iconHandle == IntPtr.Zero)
                iconHandle = GetClassLongPtr(hwnd, ICON.GCL_HICON);

            if (iconHandle == IntPtr.Zero)
                iconHandle = GetClassLongPtr(hwnd, ICON.GCL_HICONSM);

            if (iconHandle == IntPtr.Zero)
                return null;

            Icon icon = Icon.FromHandle(iconHandle);

            Icon testIcon = SystemIcons.Application;

            bool checkIcon = true;

            uint processId;
            GetWindowThreadProcessId(hwnd, out processId);
            Process proc = new Process();
            proc = Process.GetProcessById((int)processId);
            string path;

            try
            {
                path = proc.MainModule.FileName;
            }
            catch (Exception ex)
            {
                //if (ex is Win32Exception)
                {
                    path = "Access Denied. " + ex.ToString();
                    checkIcon = false;
                }
            }

            if (checkIcon == true)
            {
                try
                {
                    testIcon = System.Drawing.Icon.ExtractAssociatedIcon(path);
                }
                catch (Exception ex)
                {
                    testIcon = SystemIcons.Application;
                    checkIcon = false;
                }
            }
            if (checkIcon == true)
            {
                if (icon.Width < testIcon.Width)
                {
                    return testIcon;
                }
            }

            return icon;
        }

        public static ImageSource ToImageSource(Icon icon)
        {
            if (icon != null && !icon.Size.IsEmpty)
            {
                ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                return imageSource;
            }
            return null;
        }

        public static Rectangle RECTtoRectangle(RECT rect)
        {
            Rectangle newRectangle = new Rectangle();

            newRectangle.X = rect.Left;
            newRectangle.Y = rect.Top;

            newRectangle.Width = rect.Right - rect.Left;
            newRectangle.Height = rect.Bottom - rect.Top;

            return newRectangle;
        }

        public static RECT RectangleToRECT(Rectangle rectangle)
        {
            RECT newRECT = new RECT();

            newRECT.Left = rectangle.Left;
            newRECT.Top = rectangle.Top;
            newRECT.Right = rectangle.Right;
            newRECT.Bottom = rectangle.Bottom;

            return newRECT;
        }

        public static bool IsZeroRECT(RECT rect)
        {
            if (rect.Bottom == 0 && rect.Left == 0 && rect.Right == 0 && rect.Top == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsInRECT(System.Drawing.Point point, RECT rect)
        {
            if ((point.X >= rect.Left && point.X <= rect.Right)
                && (point.Y >= rect.Top && point.Y <= rect.Bottom))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static IntPtr GetClassLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size > 4)
            {
                return GetClassLongPtr64(hWnd, nIndex);
            }
            else
            {
                return new IntPtr(GetClassLongPtr32(hWnd, nIndex));
            }
        }

        public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 8)
            {
                return GetWindowLongPtr64(hWnd, nIndex);
            }
            else
            {
                return GetWindowLongPtr32(hWnd, nIndex);
            }
        }

        public static IntPtr SetWindowLongPtr(HandleRef hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 8)
            {
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            }
            else
            {
                return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
            }
        }

        public static System.Drawing.Point GetCursorPosition()
        {
            System.Drawing.Point lpPoint = new System.Drawing.Point();
            GetCursorPos(out lpPoint);

            return lpPoint;
        }

        public static void SetCursorPosition(int x, int y)
        {
            SetCursorPos(x, y);
        }

        #region Imports

        [DllImport("user32.dll")]
        public static extern IntPtr BeginDeferWindowPos(int nNumWindows);

        [DllImport("user32.dll")]
        public static extern bool EndDeferWindowPos(IntPtr hWinPosInfo);

        [DllImport("user32.dll")]
        public static extern IntPtr DeferWindowPos(IntPtr hWinPosInfo, IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);


        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
        public static extern void MoveWindow(IntPtr hwnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumDelegate lpEnumCallbackFunction, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int cmd);

        [DllImport("user32.dll")]
        public static extern long SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        public static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        [DllImport("User32.dll")]
        public static extern IntPtr GetParent(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWnd, IntPtr hWndParent);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll")]
        public static extern bool GetGUIThreadInfo(uint idThread, out GUITHREADINFO pgui);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        public static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        [DllImport("user32.dll")]
        public static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "GetClassLong")]
        public static extern Int64 GetClassLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
        public static extern IntPtr GetClassLongPtr64(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool PeekMessage(out NativeMessage lpMsg, HandleRef hWnd, uint mMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool PostMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static public extern IntPtr GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        static public extern int RealGetWindowClass(IntPtr hwnd, StringBuilder pszType, int cchType);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(System.Drawing.Point Point);

        [DllImport("user32.dll")]
        public static extern IntPtr RealChildWindowFromPoint(IntPtr hwndParent, System.Drawing.Point ptParentClientCoords);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT rect);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out System.Drawing.Point lpPoint);

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
    }

    #endregion
}

