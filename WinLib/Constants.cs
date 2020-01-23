using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace WinLib
{
    //https://docs.microsoft.com/en-us/windows/win32/menurc/wm-syscommand
    public static class SC
    {
        public const int
        SC_CLOSE = 0xF060,
        SC_CONTEXTHELP = 0xF180,
        SC_DEFAULT = 0xF160,
        SC_HOTKEY = 0xF150,
        SC_HSCROLL = 0xF080,
        SCF_ISSECURE = 0x00000001,
        SC_KEYMENU = 0xF100,
        SC_MAXIMIZE = 0xF030,
        SC_MINIMIZE = 0xF020,
        SC_MONITORPOWER = 0xF170,
        SC_MOUSEMENU = 0xF090,
        SC_MOVE = 0xF010,
        SC_NEXTWINDOW = 0xF040,
        SC_PREVWINDOW = 0xF050,
        SC_RESTORE = 0xF120,
        SC_SCREENSAVE = 0xF140,
        SC_SIZE = 0xF000,
        SC_TASKLIST = 0xF130,
        SC_VSCROLL = 0xF070;

    }

    //https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-windowplacement
    public static class WPF
    {
        public static readonly int
        WPF_ASYNCWINDOWPLACEMENT = 0x0004,
        WPF_RESTORETOMAXIMIZED = 0x0002,
        WPF_SETMINPOSITION = 0x0001;
    }

    //https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-windowplacement
    public static class SW
    {
        public static readonly uint
        SW_HIDE = 0,
        SW_MAXIMIZE = 3,
        SW_MINIMIZE = 6,
        SW_RESTORE = 9,
        SW_SHOW = 5,
        SW_SHOWMAXIMIZED = 3,
        SW_SHOWMINIMIZED = 2,
        SW_SHOWMINNOACTIVE = 7,
        SW_SHOWNA = 8,
        SW_SHOWNOACTIVATE = 4,
        SW_SHOWNORMAL = 1;
    }

    //https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowpos
    public static class SWP
    {
        public static readonly int
        SWP_ASYNCWINDOWPOS = 0x4000,
        SWP_DEFERERASE = 0x2000,
        SWP_DRAWFRAME = 0x0020,
        SWP_FRAMECHANGED = 0x0020,
        SWP_HIDEWINDOW = 0x0080,
        SWP_NOACTIVATE = 0x0010,
        SWP_NOCOPYBITS = 0x0100,
        SWP_NOMOVE = 0x0002,
        SWP_NOOWNERZORDER = 0x0200,
        SWP_NOREDRAW = 0x0008,
        SWP_NOREPOSITION = 0x0200,
        SWP_NOSENDCHANGE = 0x0400,
        SWP_NOSIZE = 0x0001,
        SWP_NOZORDER = 0x0004,
        SWP_SHOWWINDOW = 0x0040;
    }

    //https://docs.microsoft.com/en-us/windows/win32/winmsg/window-styles
    public static class WS
    {
        public static readonly uint
        WS_BORDER = 0x00800000,
        WS_CAPTION = 0x00C00000,
        WS_CHILD = 0x40000000,
        WS_CHILDWINDOW = 0x40000000,
        WS_CLIPCHILDREN = 0x02000000,
        WS_CLIPSIBLINGS = 0x04000000,
        WS_DISABLED = 0x08000000,
        WS_DLGFRAME = 0x00400000,
        WS_GROUP = 0x00020000,
        WS_HSCROLL = 0x00100000,
        WS_ICONIC = 0x20000000,
        WS_MAXIMIZE = 0x01000000,
        WS_MAXIMIZEBOX = 0x00010000,
        WS_MINIMIZE = 0x20000000,
        WS_MINIMIZEBOX = 0x00020000,
        WS_OVERLAPPED = 0x00000000,
        WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
        WS_POPUP = 0x80000000,
        WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
        WS_SIZEBOX = 0x00040000,
        WS_SYSMENU = 0x00080000,
        WS_TABSTOP = 0x00010000,
        WS_THICKFRAME = 0x00040000,
        WS_TILED = 0x00000000,
        WS_TILEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
        WS_VISIBLE = 0x10000000,
        WS_VSCROLL = 0x00200000;
    }

    //https://docs.microsoft.com/en-us/windows/win32/winmsg/extended-window-styles
    public static class WS_EX
    {
        public static readonly uint
        WS_EX_ACCEPTFILES = 0x00000010,
        WS_EX_APPWINDOW = 0x00040000,
        WS_EX_CLIENTEDGE = 0x00000200,
        WS_EX_COMPOSITED = 0x02000000,
        WS_EX_CONTEXTHELP = 0x00000400,
        WS_EX_CONTROLPARENT = 0x00010000,
        WS_EX_DLGMODALFRAME = 0x00000001,
        WS_EX_LAYERED = 0x00080000,
        WS_EX_LAYOUTRTL = 0x00400000,
        WS_EX_LEFT = 0x00000000,
        WS_EX_LEFTSCROLLBAR = 0x00004000,
        WS_EX_LTRREADING = 0x00000000,
        WS_EX_MDICHILD = 0x00000040,
        WS_EX_NOACTIVATE = 0x08000000,
        WS_EX_NOINHERITLAYOUT = 0x00100000,
        WS_EX_NOPARENTNOTIFY = 0x00000004,
        WS_EX_NOREDIRECTIONBITMAP = 0x00200000,
        WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE,
        WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST,
        WS_EX_RIGHT = 0x00001000,
        WS_EX_RIGHTSCROLLBAR = 0x00000000,
        WS_EX_RTLREADING = 0x00002000,
        WS_EX_STATICEDGE = 0x00020000,
        WS_EX_TOOLWINDOW = 0x00000080,
        WS_EX_TOPMOST = 0x00000008,
        WS_EX_TRANSPARENT = 0x00000020,
        WS_EX_WINDOWEDGE = 0x00000100;
    }

    //https://docs.microsoft.com/en-us/windows/win32/winmsg/window-notifications
    public static class WM
    {
        public static readonly int
        WM_ACTIVATEAPP = 0x001C,
        WM_CANCELMODE = 0x001F,
        WM_CHILDACTIVATE = 0x0022,
        WM_CLOSE = 0x0010,
        WM_COMPACTING = 0x0041,
        WM_CREATE = 0x0001,
        WM_DESTROY = 0x0002,
        WM_DPICHNANGED = 0x02E0,
        WM_ENABLE = 0x000A,
        WM_ENTERSIZEMOVE = 0x0231,
        WM_EXITSIZEMOVE = 0x0232,
        WM_GETICON = 0x007F,
        WM_GETMINMAXINFO = 0x0024,
        WM_INPUTLANGCHANGE = 0x0051,
        WM_INPUTLANGCHANGEREQUEST = 0x0050,
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_MOVE = 0x0003,
        WM_MOVING = 0x0216,
        WM_NCACTIVATE = 0x0086,
        WM_NCCALCSIZE = 0x0083,
        WM_NCCREATE = 0x0081,
        WM_NCDESTROY = 0x0082,
        WM_NULL = 0x0000,
        WM_QUERYDRAGICON = 0x0037,
        WM_QUERYOPEN = 0x0013,
        WM_QUIT = 0x0012,
        WM_SHOWWINDOW = 0x0018,
        WM_SIZE = 0x0005,
        WM_SIZING = 0x0214,
        WM_STYLECHANGED = 0x007D,
        WM_STYLECHANGING = 0x007C,
        WM_SYSCOMMAND = 0x0112,
        WM_THEMECHANGED = 0x031A,
        WM_USERCHANGED = 0x0054,
        WM_WINDOWPOSCHANGED = 0x0047,
        WM_WINDOWPOSCHANGING = 0x0046;
    }

    public static class MK
    {
        public static readonly int
        MK_LButton = 0x0001,
        MK_RButton = 0x0002,
        MK_MButton = 0x0010;
    }

    //https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-mouseactivate
    public static class MA
    {
        public static readonly int
        MA_ACTIVATE = 1,
        MA_ACTIVATEANDEAT = 2,
        MA_NOACTIVATE = 3,
        MA_NOACTIVATEANDEAT = 4;
    }

    //https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowpos
    public static class HWND
    {
        public static readonly IntPtr
        HWND_BOTTOM = new IntPtr(1),
        HWND_NOTOPMOST = new IntPtr(-2),
        HWND_TOP = new IntPtr(0),
        HWND_TOPMOST = new IntPtr(-1);
    }

    //https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-guithreadinfo
    public static class GUI
    {
        public static readonly uint
        GUI_CARETBLINKING = 0x00000001,
        GUI_INMENUMODE = 0x00000004,
        GUI_INMOVESIZE = 0x00000002,
        GUI_POPUPMENUMODE = 0x00000010,
        GUI_SYSTEMMENUMODE = 0x00000008;
    }

    //https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getguithreadinfo
    public static class CURSOR
    {
        public static readonly int
        CURSOR_LTR = 0xf00c,
        CURSOR_RTL = 0xf00d,
        CURSOR_THAI = 0xf00e,
        CURSOR_USA = 0xfff;
    }

    //https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindow
    public static class GW
    {
        public static readonly uint
        GW_CHILD = 5,
        GW_ENABLEDPOPUP = 6,
        GW_HWNDFIRST = 0,
        GW_HWNDLAST = 1,
        GW_HWNDNEXT = 2,
        GW_HWNDPREV = 3,
        GW_OWNER = 4;
    }

    //https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowlonga
    public static class GWL
    {
        public static readonly int
        GWL_STYLE = -16,
        GWL_EXSTYLE = -20,
        GWL_HINSTANCE = -6,
        GWL_HWNDPARENT = -8,
        GWL_ID = -12,
        GWL_USERDATA = -21,
        GWL_WNDPROC = -4;
    }

    //https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getclasslonga
    public static class GCL
    {
        public static readonly int
        GCW_ATOM = -32,
        GCL_CBCLSEXTRA = -20,
        GCL_CBWNDEXTRA = -18,
        GCL_HBRBACKGROUND = -10,
        GCL_HCURSOR = -12,
        GCL_HICON = -14,
        GCL_HICONSM = -34,
        GCL_HMODULE = -16,
        GCL_MENUNAME = -8,
        GCL_STYLE = -26,
        GCL_WNDPROC = -24;
    }

    //TODO: Link to MSDN page
    public static class ICON
    {
        public static readonly int
        ICON_SMALL = 0,
        ICON_BIG = 1,
        ICON_SMALL2 = 2,
        WM_GETICON = 0x7F,
        GCL_HICON = -14,
        GCL_HICONSM = -34;
    }

    //https://docs.microsoft.com/en-us/windows/desktop/winauto/event-constants
    public static class EV
    {
        public static readonly uint
        EVENT_AIA_START = 0xA000,
        EVENT_AIA_END = 0xAFFF,
        EVENT_MIN = 0x00000001,
        EVENT_MAX = 0x7FFFFFFF,
        EVENT_OBJECT_ACCELERATORCHANGE = 0x8012,
        EVENT_OBJECT_CLOAKED = 0x8017,
        EVENT_OBJECT_CONTENTSCROLLED = 0x8015,
        EVENT_OBJECT_CREATE = 0x8000,
        EVENT_OBJECT_DEFACTIONCHANGE = 0x8011,
        EVENT_OBJECT_DESCRIPTIONCHANGE = 0x800D,
        EVENT_OBJECT_DESTROY = 0x8001,
        EVENT_OBJECT_DRAGSTART = 0x8021,
        EVENT_OBJECT_DRAGCANCEL = 0x8022,
        EVENT_OBJECT_DRAGCOMPLETE = 0x8023,
        EVENT_OBJECT_DRAGENTER = 0x8024,
        EVENT_OBJECT_DRAGLEAVE = 0x8025,
        EVENT_OBJECT_DRAGDROPPED = 0x8026,
        EVENT_OBJECT_END = 0x80FF,
        EVENT_OBJECT_FOCUS = 0x8005,
        EVENT_OBJECT_HELPCHANGE = 0x8010,
        EVENT_OBJECT_HIDE = 0x8003,
        EVENT_OBJECT_HOSTEDOBJECTSINVALIDATED = 0x8020,
        EVENT_OBJECT_IME_HIDE = 0x8028,
        EVENT_OBJECT_IME_SHOW = 0x8027,
        EVENT_OBJECT_IME_CHANGE = 0x8029,
        EVENT_OBJECT_INVOKED = 0x8013,
        EVENT_OBJECT_LIVEREGIONCHANGED = 0x8019,
        EVENT_OBJECT_LOCATIONCHANGE = 0x800B,
        EVENT_OBJECT_NAMECHANGE = 0x800C,
        EVENT_OBJECT_PARENTCHANGE = 0x800F,
        EVENT_OBJECT_REORDER = 0x8004,
        EVENT_OBJECT_SELECTION = 0x8006,
        EVENT_OBJECT_SELECTIONADD = 0x8007,
        EVENT_OBJECT_SELECTIONREMOVE = 0x8008,
        EVENT_OBJECT_SELECTIONWITHIN = 0x8009,
        EVENT_OBJECT_SHOW = 0x8002,
        EVENT_OBJECT_STATECHANGE = 0x800A,
        EVENT_OBJECT_TEXTEDIT_CONVERSIONTARGETCHANGED = 0x8030,
        EVENT_OBJECT_TEXTSELECTIONCHANGED = 0x8014,
        EVENT_OBJECT_UNCLOAKED = 0x8018,
        EVENT_OBJECT_VALUECHANGE = 0x800E,
        EVENT_OEM_DEFINED_START = 0x0101,
        EVENT_OEM_DEFINED_END = 0x01FF,
        EVENT_SYSTEM_ALERT = 0x0002,
        EVENT_SYSTEM_ARRANGEMENTPREVIEW = 0x8016,
        EVENT_SYSTEM_CAPTUREEND = 0x0009,
        EVENT_SYSTEM_CAPTURESTART = 0x0008,
        EVENT_SYSTEM_CONTEXTHELPEND = 0x000D,
        EVENT_SYSTEM_CONTEXTHELPSTART = 0x000C,
        EVENT_SYSTEM_DESKTOPSWITCH = 0x0020,
        EVENT_SYSTEM_DIALOGEND = 0x0011,
        EVENT_SYSTEM_DIALOGSTART = 0x0010,
        EVENT_SYSTEM_DRAGDROPEND = 0x000F,
        EVENT_SYSTEM_DRAGDROPSTART = 0x000E,
        EVENT_SYSTEM_END = 0x00FF,
        EVENT_SYSTEM_FOREGROUND = 0x0003,
        EVENT_SYSTEM_MENUPOPUPEND = 0x0007,
        EVENT_SYSTEM_MENUPOPUPSTART = 0x0006,
        EVENT_SYSTEM_MENUEND = 0x0005,
        EVENT_SYSTEM_MENUSTART = 0x0004,
        EVENT_SYSTEM_MINIMIZEEND = 0x0017,
        EVENT_SYSTEM_MINIMIZESTART = 0x0016,
        EVENT_SYSTEM_MOVESIZEEND = 0x000B,
        EVENT_SYSTEM_MOVESIZESTART = 0x000A,
        EVENT_SYSTEM_SCROLLINGEND = 0x0013,
        EVENT_SYSTEM_SCROLLINGSTART = 0x0012,
        EVENT_SYSTEM_SOUND = 0x0001,
        EVENT_SYSTEM_SWITCHEND = 0x0015,
        EVENT_SYSTEM_SWITCHSTART = 0x0014,
        EVENT_UIA_EVENTID_START = 0x4E00,
        EVENT_UIA_EVENTID_END = 0x4EFF,
        EVENT_UIA_PROPID_START = 0x7500,
        EVENT_UIA_PROPID_END = 0x75FF;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NativeMessage
    {
        public IntPtr handle;
        public uint msg;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public System.Drawing.Point p;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        internal RECT(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public int Left;        // x position of upper-left corner
        public int Top;         // y position of upper-left corner
        public int Right;       // x position of lower-right corner
        public int Bottom;      // y position of lower-right corner
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PSIZE
    {
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;

        public static implicit operator Point(POINT point)
        {
            return new Point(point.x, point.y);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWINFO
    {
        public uint cbSize;
        public RECT rcWindow;
        public RECT rcClient;
        public uint dwStyle;
        public uint dwExStyle;
        public uint dwWindowStatus;
        public uint cxWindowBorders;
        public uint cyWindowBorders;
        public ushort atomWindowType;
        public ushort wCreatorVersion;

        public WINDOWINFO(Boolean? filler)
         : this()   // Allows automatic initialization of "cbSize" with "new WINDOWINFO(null/true/false)".
        {
            cbSize = (UInt32)(Marshal.SizeOf(typeof(WINDOWINFO)));
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GUITHREADINFO
    {
        public uint cbSize;
        public uint flags;
        public IntPtr hwndActive;
        public IntPtr hwndFocus;
        public IntPtr hwndCapture;
        public IntPtr hwndMenuOwner;
        public IntPtr hwndMoveSize;
        public IntPtr hwndCaret;
        public RECT rcCaret;

        public GUITHREADINFO(Boolean? filler)
         : this()   //Allows automatic initialization of "cbSize" with "new GUITHREADINFO(null/true/false)".
        {
            cbSize = (UInt32)(Marshal.SizeOf(typeof(GUITHREADINFO)));
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPLACEMENT
    {
        public uint length;
        public uint flags;
        public uint showCmd;
        public System.Drawing.Point ptMinPosition;
        public System.Drawing.Point ptMaxPosition;
        public RECT rcNormalPosition;
        public RECT rcDevice;

        public WINDOWPLACEMENT(Boolean? filler)
            : this() //Allows automatic initialization of "length" with "new WINDOWPLACEMENT(null/true/false)".
        {
            length = (UInt32)(Marshal.SizeOf(typeof(WINDOWPLACEMENT)));
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPOS
    {
        public IntPtr hwnd;
        public IntPtr hwndInsertAfter;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public uint flags;
    }
}
