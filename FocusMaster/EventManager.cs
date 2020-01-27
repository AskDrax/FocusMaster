using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Automation;
using WinLib;

namespace FocusMaster
{
    public class EventManager
    {
        public EventManager()
        {
            Initialize();
        }

        public static bool NoClickOnlyFocus { get; set; }
        public static bool NoFocusOnlyClick { get; set; }
        public static MainWindow MainWindow { get; set; }
        public static Log CurrentLog { get; set; }
        public static MouseInput MouseInput { get; set; }
        public static ObservableCollection<WinEventHook> RegisteredHooks { get; set; }

        public delegate void Handler(object sender, WinEventProcEventArgs e);

        private IntPtr DesktopHWND;

        private IntPtr currentForegroundHWND;
        private IntPtr lastForegroundHWND;

        private string currentForegroundName;
        private string lastForegroundName;

        private IntPtr currentFocusedHWND;
        private IntPtr lastFocusedHWND;

        private string currentFocusedName;
        private string lastFocusedName;

        private IntPtr HWNDUnderMouse;
        private IntPtr LastHWNDUnderMouse;
        private IntPtr HWNDUnderUnderMouse;
        private IntPtr LastHWNDUnderUnderMouse;

        public IntPtr WindowDraggingHWND;
        public IntPtr WindowDragOverHWND;
        public IntPtr LastWindowDragOverHWND;

        public RECT currentWindowRECT;
        public RECT lastWindowRECT;

        public bool IsStartDragging = false;
        public bool IsWindowMoving = false;
        public bool IsWindowSizing = false;
        public bool IsDragMoveOver = false;
        public bool IsDragSizeOver = false;

        private System.Timers.Timer UpdateTimer { get; set; }
        private int UpdateInterval = 8;

        private System.Timers.Timer SnapTimer { get; set; }
        private int SnapDelay = 25;

        private WindowHelper.WinEventDelegate procDelegate;

        public void Initialize()
        {
            NoClickOnlyFocus = false;

            MainWindow = (MainWindow)Application.Current.MainWindow;
            DesktopHWND = WindowHelper.GetDesktopWindow();

            if (CurrentLog == null)
            {
                CurrentLog = new Log();
                CurrentLog.FilterBy = new List<LogEntryType>();
                CurrentLog.FilterBy.Add(LogEntryType.None);
                CurrentLog.FilterBy.Add(LogEntryType.WindowsEvent);
                CurrentLog.FilterBy.Add(LogEntryType.ApplicationEvent);
                CurrentLog.FilterView.Refresh();
            }


            WindowHelper.windowList = new ObservableCollection<AWindow>();
            WindowHelper.EnumAllWindows();
            DesktopHWND = WindowHelper.GetDesktopWindow();

            if (MouseInput == null)
            {
                MouseInput = new MouseInput();
                MouseInput.Install();
                MouseInput.LeftButtonDown += new MouseInput.MouseHookCallback(OnLeftMouseDown);
            }

            if (UpdateTimer == null || SnapTimer == null)
            {
                UpdateTimer = new System.Timers.Timer();
                UpdateTimer.Interval = UpdateInterval;
                UpdateTimer.AutoReset = true;
                UpdateTimer.Elapsed += UpdateTimer_Tick;
                UpdateTimer.Enabled = true;
                UpdateTimer.Start();

                SnapTimer = new System.Timers.Timer();
                SnapTimer.Interval = SnapDelay;
                SnapTimer.AutoReset = false;
                SnapTimer.Elapsed += SnapTimer_Tick;
                SnapTimer.Enabled = false;
            }

            if (procDelegate == null)
            {
                procDelegate = new WindowHelper.WinEventDelegate(WinEventProc);
            }

            RegisterHooks();
        }

        public void RegisterHooks()
        {
            if (RegisteredHooks == null)
            {
                RegisteredHooks = new ObservableCollection<WinEventHook>();
            }
            //RegisteredHooks.Add(new WinEventHook(EV.EVENT_OBJECT_CREATE, OnObjectCreated));
            //RegisteredHooks.Add(new WinEventHook(EV.EVENT_OBJECT_DESTROY, OnObjectDestroyed));
            //RegisteredHooks.Add(new WinEventHook(EV.EVENT_OBJECT_NAMECHANGE, OnObjectNameChanged));
            RegisteredHooks.Add(new WinEventHook(EV.EVENT_SYSTEM_FOREGROUND, OnForegroundChanged));
            RegisteredHooks.Add(new WinEventHook(EV.EVENT_OBJECT_FOCUS, OnFocusChanged));
            RegisteredHooks.Add(new WinEventHook(EV.EVENT_SYSTEM_MOVESIZESTART, OnMoveSizeStart));
            RegisteredHooks.Add(new WinEventHook(EV.EVENT_SYSTEM_MOVESIZEEND, OnMoveSizeEnd));
            RegisteredHooks.Add(new WinEventHook(EV.EVENT_OBJECT_LOCATIONCHANGE, OnLocationChanged));
            RegisteredHooks.Add(new WinEventHook(EV.EVENT_OBJECT_STATECHANGE, OnStateChanged));
            RegisteredHooks.Add(new WinEventHook(EV.EVENT_SYSTEM_MINIMIZESTART, OnMinimizeStart));
            RegisteredHooks.Add(new WinEventHook(EV.EVENT_SYSTEM_MINIMIZEEND, OnMinimizeEnd));

            SetHooks();
        }

        public void SetHooks()
        {
            foreach (WinEventHook hook in RegisteredHooks)
            {
                if (!hook.IsHooked())
                {
                    hook.Handle = WindowHelper.SetWinEventHook(hook.EventMin, hook.EventMax, hook.HmodWinEventProc, procDelegate, hook.IdProcess, hook.IdThread, hook.DwFlags);
                }
            }
        }

        public void UnsetHooks()
        {
            foreach (WinEventHook hook in RegisteredHooks)
            {
                if (hook.IsHooked())
                {
                    WindowHelper.UnhookWinEvent(hook.Handle);
                    hook.Handle = IntPtr.Zero;
                }
            }
        }

        public void Dispose()
        {
            if (MouseInput != null)
            {
                MouseInput.LeftButtonDown -= new MouseInput.MouseHookCallback(OnLeftMouseDown);
                MouseInput.Uninstall();
            }

            if (procDelegate != null)
            {
                UnsetHooks();
            }

            if (UpdateTimer != null || SnapTimer != null)
            {
                UpdateTimer.Stop();
                UpdateTimer.Dispose();

                SnapTimer.Stop();
                SnapTimer.Dispose();
            }

            if (CurrentLog != null)
            {
                //Write Log to File on Exit?
                CurrentLog.LogEntries.Clear();
            }
        }

        private void UpdateTimer_Tick(object sender, ElapsedEventArgs e)
        {
            MainWindow.Dispatcher.Invoke(() =>
            {
                MouseInput.GetMousePoint();
                HWNDUnderMouse = WindowHelper.WindowFromPoint(MouseInput.mousePoint);
                IntPtr parentHWND = WindowHelper.GetParentWindow(HWNDUnderMouse);
                HWNDUnderMouse = parentHWND;

                HWNDUnderUnderMouse = WindowHelper.GetNextWindowUnderPoint(MouseInput.mousePoint, HWNDUnderMouse);

                if (LastHWNDUnderUnderMouse == null)
                {
                    LastHWNDUnderUnderMouse = HWNDUnderUnderMouse;
                }

                if (LastWindowDragOverHWND == null)
                {
                    LastWindowDragOverHWND = WindowDragOverHWND;
                }

                if (HWNDUnderMouse != LastHWNDUnderMouse && IsWindowMoving == false && IsWindowSizing == false)
                {
                    OnMouseWindowChanged(null, new MouseWindowEventArgs() { MousePoint = MouseInput.mousePoint, HWND = HWNDUnderMouse });
                    LastHWNDUnderMouse = HWNDUnderMouse;

                }

                if (IsWindowMoving)
                {
                    if (HWNDUnderMouse != WindowDraggingHWND)
                    {
                        HWNDUnderUnderMouse = HWNDUnderMouse;
                    }

                    if (HWNDUnderUnderMouse != IntPtr.Zero && HWNDUnderUnderMouse != LastHWNDUnderUnderMouse && HWNDUnderUnderMouse != DesktopHWND)
                    {
                        OnMouseWindowDragMoveLeave(null, new MouseWindowDragEventArgs() { DraggingHWND = WindowDraggingHWND, MousePoint = MouseInput.mousePoint, ToHWND = LastHWNDUnderUnderMouse });
                        OnMouseWindowDragMoveEnter(null, new MouseWindowDragEventArgs() { DraggingHWND = WindowDraggingHWND, MousePoint = MouseInput.mousePoint, ToHWND = HWNDUnderUnderMouse });
                        WindowDragOverHWND = HWNDUnderUnderMouse;
                        LastWindowDragOverHWND = WindowDragOverHWND;
                        LastHWNDUnderUnderMouse = HWNDUnderUnderMouse;
                        IsDragMoveOver = true;
                    }
                }

                if (IsWindowSizing)
                {
                    if (HWNDUnderMouse != WindowDraggingHWND)
                    {
                        HWNDUnderUnderMouse = HWNDUnderMouse;
                    }

                    if (HWNDUnderUnderMouse != IntPtr.Zero && HWNDUnderUnderMouse != LastHWNDUnderUnderMouse && HWNDUnderUnderMouse != DesktopHWND)
                    {
                        OnMouseWindowDragSizeLeave(null, new MouseWindowDragEventArgs() { DraggingHWND = WindowDraggingHWND, MousePoint = MouseInput.mousePoint, ToHWND = LastHWNDUnderUnderMouse });
                        OnMouseWindowDragSizeEnter(null, new MouseWindowDragEventArgs() { DraggingHWND = WindowDraggingHWND, MousePoint = MouseInput.mousePoint, ToHWND = HWNDUnderUnderMouse });
                        WindowDragOverHWND = HWNDUnderUnderMouse;
                        LastWindowDragOverHWND = WindowDragOverHWND;
                        LastHWNDUnderUnderMouse = HWNDUnderUnderMouse;
                        IsDragSizeOver = true;
                    }
                }
            });
        }

        private void SnapTimer_Tick(object sender, ElapsedEventArgs e)
        {
            SnapTimer.Enabled = false;

            MainWindow.Dispatcher.Invoke(() =>
            {
                //UpdatePos()...
            });
        }

        private void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            foreach (WinEventHook hook in RegisteredHooks)
            {
                if (hook.IsHooked() && (eventType >= hook.EventMin && eventType <= hook.EventMax))
                {
                    WinEventProcEventArgs args = new WinEventProcEventArgs()
                    {
                        _hWinEventHook = hWinEventHook,
                        _eventType = eventType,
                        _hwnd = hwnd,
                        _idObject = idObject,
                        _idChild = idChild,
                        _dwEventThread = dwEventThread,
                        _dwmsEventTime = dwmsEventTime,
                    };
                    hook.eventHandler.Invoke(hWinEventHook, args);
                }
            }
        }

        protected virtual void OnObjectCreated(object sender, WinEventProcEventArgs e)
        {
            if (WindowHelper.IsWindow(e._hwnd))
            {
                string title = WindowHelper.GetTitleOfWindow(e._hwnd);
                OnWindowCreated(sender, new OpenCloseEventArgs() { HWND = e._hwnd, Title = title });
                return;
            }

            CurrentLog.Add(LogEntryType.WindowsEvent, "Object Created: (" + e._hwnd.ToString() + ")");
        }

        protected virtual void OnObjectDestroyed(object sender, WinEventProcEventArgs e)
        {
            AWindow awin = WindowHelper.windowList.FirstOrDefault(awindow => awindow.Hwnd == e._hwnd);
            if (awin != null)
            {
                OnWindowDestroyed(sender, new OpenCloseEventArgs() { HWND = e._hwnd, Title = awin.Title });
                return;
            }

            CurrentLog.Add(LogEntryType.WindowsEvent, "Object Destroyed: (" + e._hwnd.ToString() + ")");
        }

        protected virtual void OnObjectNameChanged(object sender, WinEventProcEventArgs e)
        {
            string newName = WindowHelper.GetTitleOfWindow(e._hwnd);
            AWindow awin = WindowHelper.windowList.FirstOrDefault(awindow => awindow.Hwnd == e._hwnd);
            if (awin != null)
            {
                string oldName = awin.Title;
                OnWindowNameChanged(sender, new NameChangeEventArgs() { HWND = e._hwnd, oldName = oldName, newName = newName });
                return;
            }

            CurrentLog.Add(LogEntryType.WindowsEvent, "Object Name Changed: " + newName + "(" + e._hwnd.ToString() + ")");
        }

        protected virtual void OnForegroundChanged(object sender, WinEventProcEventArgs e)
        {
            MouseInput.GetMousePoint();

            lastForegroundHWND = currentForegroundHWND;
            currentForegroundHWND = e._hwnd;

            lastForegroundName = currentForegroundName;
            currentForegroundName = WindowHelper.GetTitleOfWindow(e._hwnd);

            if (lastForegroundHWND != currentForegroundHWND)
                CurrentLog.Add(LogEntryType.WindowsEvent, "Foreground Window Changed: from " + lastForegroundName + " (" + lastForegroundHWND.ToString() + ") to " + currentForegroundName + " (" + currentForegroundHWND.ToString() + ") at " + MouseInput.mousePoint.ToString());
        }

        protected virtual void OnFocusChanged(object sender, WinEventProcEventArgs e)
        {
            MouseInput.GetMousePoint();

            lastFocusedHWND = currentFocusedHWND;
            currentFocusedHWND = e._hwnd;

            lastFocusedName = currentFocusedName;
            currentFocusedName = WindowHelper.GetTitleOfWindow(e._hwnd);

            if (lastFocusedHWND != currentFocusedHWND)
                CurrentLog.Add(LogEntryType.WindowsEvent, "Focused Window Changed: from " + lastFocusedName + " (" + lastFocusedHWND.ToString() + ") to " + currentFocusedName + " (" + currentFocusedHWND.ToString() + ") at " + MouseInput.mousePoint.ToString());

            //OnStateChanged(sender, e);
        }

        protected virtual void OnStateChanged(object sender, WinEventProcEventArgs e)
        {
            AWindow awin = WindowHelper.windowList.FirstOrDefault(awindow => awindow.Hwnd == e._hwnd);
            if (awin != null)
            {
                WINDOWPLACEMENT newPlacement = new WINDOWPLACEMENT(true);
                WindowHelper.GetWindowPlacement(e._hwnd, ref newPlacement);
                awin.LastPlacement = awin.Placement;
                awin.Placement = newPlacement;

                OnWindowStateChanged(sender, new WindowStateChangedEventArgs() { HWND = e._hwnd, oldPlacement = awin.LastPlacement, newPlacement = awin.Placement });  
            }
        }

        protected virtual void OnMinimizeStart(object sender, WinEventProcEventArgs e)
        {
            OnStateChanged(sender, e);
        }

        protected virtual void OnMinimizeEnd(object sender, WinEventProcEventArgs e)
        {
            OnStateChanged(sender, e);
        }

        protected virtual void OnMoveSizeStart(object sender, WinEventProcEventArgs e)
        {
            MouseInput.GetMousePoint();

            WindowDraggingHWND = e._hwnd;
            IsStartDragging = true;
            WindowHelper.GetWindowRect(e._hwnd, ref currentWindowRECT);
            lastWindowRECT = currentWindowRECT;

            string name = WindowHelper.GetTitleOfWindow(e._hwnd);
            CurrentLog.Add(LogEntryType.WindowsEvent, "Window MoveSize Started: " + name + " (" + e._hwnd.ToString() + ") at " + MouseInput.mousePoint.ToString());
        }

        protected virtual void OnMoveSizeEnd(object sender, WinEventProcEventArgs e)
        {
            MouseInput.GetMousePoint();

            WindowHelper.GetWindowRect(e._hwnd, ref currentWindowRECT);

            if (IsWindowMoving)
            {
                OnWindowMoveEnd(sender, new MouseWindowEventArgs() { HWND = e._hwnd, MousePoint = MouseInput.mousePoint });
                IsWindowMoving = false;
            }

            if (IsWindowSizing)
            {
                OnWindowSizeEnd(sender, new MouseWindowEventArgs() { HWND = e._hwnd, MousePoint = MouseInput.mousePoint });
                IsWindowSizing = false;
            }

            if (IsDragMoveOver)
            {
                OnMouseWindowDragMoveDrop(sender, new MouseWindowDragEventArgs() { DraggingHWND = e._hwnd, ToHWND = WindowDragOverHWND, MousePoint = MouseInput.mousePoint });
                IsDragMoveOver = false;
            }

            if (IsDragSizeOver)
            {
                OnMouseWindowDragSizeDrop(sender, new MouseWindowDragEventArgs() { DraggingHWND = e._hwnd, ToHWND = WindowDragOverHWND, MousePoint = MouseInput.mousePoint });
                IsDragSizeOver = false;
            }
        }

        protected virtual void OnLocationChanged(object sender, WinEventProcEventArgs e)
        {
            MouseInput.GetMousePoint();

            WindowHelper.GetWindowRect(e._hwnd, ref currentWindowRECT);

            if (IsStartDragging && e._hwnd == WindowDraggingHWND)
            {
                if (currentWindowRECT.Equals(lastWindowRECT) == false)
                {
                    int currentWidth = currentWindowRECT.Right - currentWindowRECT.Left;
                    int currentHeight = currentWindowRECT.Bottom - currentWindowRECT.Top;
                    int lastWidth = lastWindowRECT.Right - lastWindowRECT.Left;
                    int lastHeight = lastWindowRECT.Bottom - lastWindowRECT.Top;

                    if (currentWidth == lastWidth && currentHeight == lastHeight)
                    {
                        OnWindowMoveStart(sender, new MouseWindowEventArgs() { HWND = e._hwnd, MousePoint = MouseInput.mousePoint });
                        WindowDraggingHWND = e._hwnd;
                        IsWindowMoving = true;
                        IsStartDragging = false;
                    }
                    else
                    {
                        OnWindowSizeStart(sender, new MouseWindowEventArgs() { HWND = e._hwnd, MousePoint = MouseInput.mousePoint });
                        WindowDraggingHWND = e._hwnd;
                        IsWindowSizing = true;
                        IsStartDragging = false;
                    }
                }
            }
        }

        protected virtual void OnWindowCreated(object sender, OpenCloseEventArgs e)
        {
            AWindow awin = WindowHelper.GetAWINDOW(e.HWND);
            WindowHelper.windowList.Add(awin);

            CurrentLog.Add(LogEntryType.WindowsEvent, "Window Created: " + e.Title + ", (" + e.HWND.ToString() + ")");
        }

        protected virtual void OnWindowDestroyed(object sender, OpenCloseEventArgs e)
        {
            AWindow awin = WindowHelper.windowList.FirstOrDefault(awindow => awindow.Hwnd == e.HWND);
            if (awin != null)
            {
                WindowHelper.windowList.Remove(awin);
                CurrentLog.Add(LogEntryType.WindowsEvent, "Window Destroyed: " + e.Title + ", (" + e.HWND.ToString() + ")");
            }
        }

        protected virtual void OnWindowNameChanged(object sender, NameChangeEventArgs e)
        {
            AWindow awin = WindowHelper.windowList.FirstOrDefault(awindow => awindow.Hwnd == e.HWND);
            if (awin != null)
            {
                awin.Title = e.newName;
                CurrentLog.Add(LogEntryType.WindowsEvent, "Window Name Changed: " + e.oldName + " to " + e.newName + " (" + e.HWND.ToString() + ")");
            }
        }

        protected virtual void OnWindowStateChanged(object sender, WindowStateChangedEventArgs e)
        {
            string FromString;
            string ToString;

            uint oldShowState = e.oldPlacement.showCmd;
            uint newShowState = e.newPlacement.showCmd;

            FromString = WindowHelper.ShowCmdToString(oldShowState);
            ToString = WindowHelper.ShowCmdToString(newShowState);

            if (oldShowState == newShowState)
            {
                CurrentLog.FilterView.Refresh();
                MainWindow.LogPage.ScrollToCurrent();
            }
            else
            {
                CurrentLog.Add(LogEntryType.WindowsEvent, "Window State Changed: " + WindowHelper.GetTitleOfWindow(e.HWND).ToString() + " from " + FromString + " (" + oldShowState.ToString() + ") to " + ToString + " (" + newShowState.ToString() + ")");
            }
            
        }

        protected virtual void OnWindowMoveStart(object sender, MouseWindowEventArgs e)
        {
            string name = WindowHelper.GetTitleOfWindow(e.HWND);
            CurrentLog.Add(LogEntryType.WindowsEvent, "Window Move Started: " + name + " (" + e.HWND.ToString() + ") at " + e.MousePoint.ToString());
        }

        protected virtual void OnWindowSizeStart(object sender, MouseWindowEventArgs e)
        {
            string name = WindowHelper.GetTitleOfWindow(e.HWND);
            CurrentLog.Add(LogEntryType.WindowsEvent, "Window Size Started: " + name + " (" + e.HWND.ToString() + ") at " + e.MousePoint.ToString());
        }

        protected virtual void OnWindowMoveEnd(object sender, MouseWindowEventArgs e)
        {
            string name = WindowHelper.GetTitleOfWindow(e.HWND);
            CurrentLog.Add(LogEntryType.WindowsEvent, "Window Move Ended: " + name + " (" + e.HWND.ToString() + ") at " + e.MousePoint.ToString());
        }

        protected virtual void OnWindowSizeEnd(object sender, MouseWindowEventArgs e)
        {
            string name = WindowHelper.GetTitleOfWindow(e.HWND);
            CurrentLog.Add(LogEntryType.WindowsEvent, "Window Size Ended: " + name + " (" + e.HWND.ToString() + ") at " + e.MousePoint.ToString());
        }

        protected virtual void OnMouseWindowChanged(object sender, MouseWindowEventArgs e)
        {
            if (HWNDUnderMouse != currentForegroundHWND && WindowHelper.IsInWindowList(HWNDUnderMouse))
            {
                if (NoClickOnlyFocus)
                {
                    MouseInput.supressNext = true;
                    MouseInput.targetHWND = HWNDUnderMouse;
                }
                else if (NoFocusOnlyClick)
                {
                    MouseInput.supressNext = false;
                    WindowHelper.RemoveWindowStyleEx(LastHWNDUnderMouse, new IntPtr(WS_EX.WS_EX_NOACTIVATE));
                    WindowHelper.AddWindowStyleEx(HWNDUnderMouse, new IntPtr(WS_EX.WS_EX_NOACTIVATE));
                }
                else
                {
                    MouseInput.supressNext = false;

                    WindowHelper.RemoveWindowStyleEx(HWNDUnderMouse, new IntPtr(WS_EX.WS_EX_NOACTIVATE));
                    WindowHelper.RemoveWindowStyleEx(LastHWNDUnderMouse, new IntPtr(WS_EX.WS_EX_NOACTIVATE));
                }              
            }
            else
            {
                MouseInput.supressNext = false;

                WindowHelper.RemoveWindowStyleEx(HWNDUnderMouse, new IntPtr(WS_EX.WS_EX_NOACTIVATE));
                WindowHelper.RemoveWindowStyleEx(LastHWNDUnderMouse, new IntPtr(WS_EX.WS_EX_NOACTIVATE));
            }

            string name = WindowHelper.GetTitleOfWindow(e.HWND);
            CurrentLog.Add(LogEntryType.WindowsEvent, "Mouse is now over: " + name + " (" + e.HWND.ToString() + ") at " + e.MousePoint.ToString());
        }

        protected virtual void OnMouseWindowDragMoveLeave(object sender, MouseWindowDragEventArgs e)
        {
            string draggingWindowTitle = WindowHelper.GetTitleOfWindow(e.DraggingHWND);
            string targetWindowTitle = WindowHelper.GetTitleOfWindow(e.ToHWND);
            CurrentLog.Add(LogEntryType.WindowsEvent, "Mouse Drag-Moved Window: " + draggingWindowTitle + " out of Window: " + targetWindowTitle + " at " + e.MousePoint.ToString());
        }

        protected virtual void OnMouseWindowDragSizeLeave(object sender, MouseWindowDragEventArgs e)
        {
            string draggingWindowTitle = WindowHelper.GetTitleOfWindow(e.DraggingHWND);
            string targetWindowTitle = WindowHelper.GetTitleOfWindow(e.ToHWND);
            CurrentLog.Add(LogEntryType.WindowsEvent, "Mouse Drag-Sized Window: " + draggingWindowTitle + " out of Window: " + targetWindowTitle + " at " + e.MousePoint.ToString());
        }

        protected virtual void OnMouseWindowDragMoveEnter(object sender, MouseWindowDragEventArgs e)
        {
            string draggingWindowTitle = WindowHelper.GetTitleOfWindow(e.DraggingHWND);
            string targetWindowTitle = WindowHelper.GetTitleOfWindow(e.ToHWND);
            CurrentLog.Add(LogEntryType.WindowsEvent, "Mouse Drag-Moved Window: " + draggingWindowTitle + " onto Window: " + targetWindowTitle + " at " + e.MousePoint.ToString());
        }

        protected virtual void OnMouseWindowDragSizeEnter(object sender, MouseWindowDragEventArgs e)
        {
            string draggingWindowTitle = WindowHelper.GetTitleOfWindow(e.DraggingHWND);
            string targetWindowTitle = WindowHelper.GetTitleOfWindow(e.ToHWND);
            CurrentLog.Add(LogEntryType.WindowsEvent, "Mouse Drag-Sized Window: " + draggingWindowTitle + " onto Window: " + targetWindowTitle + " at " + e.MousePoint.ToString());
        }

        protected virtual void OnMouseWindowDragMoveDrop(object sender, MouseWindowDragEventArgs e)
        {
            string draggingWindowTitle = WindowHelper.GetTitleOfWindow(e.DraggingHWND);
            string targetWindowTitle = WindowHelper.GetTitleOfWindow(e.ToHWND);
            CurrentLog.Add(LogEntryType.WindowsEvent, "Mouse Drag-Move-Dropped Window: " + draggingWindowTitle + " onto Window: " + targetWindowTitle + " at " + e.MousePoint.ToString());
        }

        protected virtual void OnMouseWindowDragSizeDrop(object sender, MouseWindowDragEventArgs e)
        {
            string draggingWindowTitle = WindowHelper.GetTitleOfWindow(e.DraggingHWND);
            string targetWindowTitle = WindowHelper.GetTitleOfWindow(e.ToHWND);
            CurrentLog.Add(LogEntryType.WindowsEvent, "Mouse Drag-Size-Dropped Window: " + draggingWindowTitle + " onto Window: " + targetWindowTitle + " at " + e.MousePoint.ToString());
        }

        protected virtual void OnLeftMouseDown(MouseInput.MSLLHOOKSTRUCT mouseStruct)
        {
            CurrentLog.Add(LogEntryType.None, "Mouse Left-Clicked at: X=" + mouseStruct.pt.x.ToString() + ",Y=" + mouseStruct.pt.y.ToString());

            //OnStateChanged(null, new WinEventProcEventArgs() { _hwnd = currentForegroundHWND});
        }
    }

    public class WinEventProcEventArgs : EventArgs
    {
        public IntPtr _hWinEventHook;
        public uint _eventType;
        public IntPtr _hwnd;
        public int _idObject;
        public int _idChild;
        public uint _dwEventThread;
        public uint _dwmsEventTime;
    }

    public class NameChangeEventArgs : EventArgs
    {
        public string oldName { get; set; }
        public string newName { get; set; }
        public IntPtr HWND { get; set; }
    }

    public class ForegroundChangeEventArgs : EventArgs
    {
        public IntPtr fromHWND { get; set; }
        public IntPtr toHWND { get; set; }
        public string fromName { get; set; }
        public string toName { get; set; }
    }

    public class OpenCloseEventArgs : EventArgs
    {
        public string Title { get; set; }
        public IntPtr HWND { get; set; }
    }

    public class MouseWindowEventArgs : EventArgs
    {
        public System.Drawing.Point MousePoint { get; set; }
        public IntPtr HWND { get; set; }
    }

    public class MouseWindowDragEventArgs : EventArgs
    {
        public System.Drawing.Point MousePoint { get; set; }
        public IntPtr DraggingHWND { get; set; }
        public IntPtr ToHWND { get; set; }
    }

    public class FocusChangedEventArgs : EventArgs
    {
        public System.Drawing.Point MousePoint { get; set; }
        public IntPtr fromHWND { get; set; }
        public IntPtr toHWND { get; set; }
    }

    public class WindowStateChangedEventArgs : EventArgs
    {
        public IntPtr HWND { get; set; }
        public WINDOWPLACEMENT oldPlacement { get; set; }
        public WINDOWPLACEMENT newPlacement { get; set; }
    }
}
