using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Media;

namespace WinLib
{
    public class AWindow
    {
        public Guid GUID { get; set; }
        public IntPtr hwnd { get; set; }
        public string title { get; set; }
        public string className { get; set; }
        public string baseClassName { get; set; }
        public IntPtr savedParent { get; set; }
        public IntPtr savedStyle { get; set; }
        public AutomationElement automationElement { get; set; }
        public WINDOWPLACEMENT placement { get; set; }
        public RECT savedRECT { get; set; }
        public RECT niceRECT { get; set; }
        public RECT borderRECT { get; set; }
        public RECT minRECT { get; set; }
        public Icon icon { get; set; }
        public ImageSource iconImage { get; set; }
        public WINDOWINFO info { get; set; }

    }
}
