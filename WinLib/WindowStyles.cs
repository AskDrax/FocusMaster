using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WinLib
{
    public class WindowStyles
    {
        public Dictionary<string, uint> Styles { get; set; }
        public Dictionary<string, uint> ExStyles { get; set; }

        public static Dictionary<string, uint> AllStyles { get; set; }
        public static Dictionary<string, uint> AllExStyles { get; set; }

        public WindowStyles FromHWND(IntPtr hwnd)
        {
            PopulateWindowStyles();
            Styles = new Dictionary<string, uint>();
            uint currentStyle = (uint)WindowHelper.GetWindowStyle(hwnd).ToInt64();

            foreach (KeyValuePair<string, uint> style in AllStyles)
            {
                if ((currentStyle & style.Value) != 0)
                {
                    Styles.Add(style.Key, style.Value);
                }
            }

            ExStyles = new Dictionary<string, uint>();
            uint currentExStyle = (uint)WindowHelper.GetWindowStyleEx(hwnd);

            foreach (KeyValuePair<string, uint> exstyle in AllExStyles)
            {
                if ((currentExStyle & exstyle.Value) != 0)
                {
                    ExStyles.Add(exstyle.Key, exstyle.Value);
                }
            }

            return this;
        }
        public void PopulateWindowStyles()
        {
            AllStyles = new Dictionary<string, uint>();

            foreach (FieldInfo field in typeof(WS).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                AllStyles.Add(field.Name, (uint)field.GetValue(this));
            }

            AllExStyles = new Dictionary<string, uint>();

            foreach (FieldInfo field in typeof(WS_EX).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                AllExStyles.Add(field.Name, (uint)field.GetValue(this));
            }
        }
    }
}
