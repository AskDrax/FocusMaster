using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WinLib
{
    public static class DisplayHelper
    {
        public delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);

        public static ObservableCollection<DisplayInfo> displayList { get; set; }

        public static int NumberOfDisplays;

        public static string PrimaryScreenResolution;
        public static double PrimaryScreenWidth;
        public static double PrimaryScreenHeight;

        public static string VirtualScreenResolution;
        public static double VirtualScreenWidth;
        public static double VirtualScreenHeight;


        public static void Initialize()
        {
            DisplayInfoCollection displayInfo = new DisplayInfoCollection();
            displayInfo = displayInfo.GetDisplays();
            displayList = displayInfo;

            NumberOfDisplays = displayList.Count();
            PrimaryScreenWidth = SystemParameters.PrimaryScreenWidth;
            PrimaryScreenHeight = SystemParameters.PrimaryScreenHeight;
            VirtualScreenWidth = SystemParameters.VirtualScreenWidth;
            VirtualScreenHeight = SystemParameters.VirtualScreenHeight;

            PrimaryScreenResolution = PrimaryScreenWidth.ToString() + " x " + PrimaryScreenHeight.ToString();
            VirtualScreenResolution = VirtualScreenWidth.ToString() + " x " + VirtualScreenHeight.ToString();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

        [DllImport("user32.dll")]
        public static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumDelegate lpfnEnum, IntPtr dwData);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFOEX lpmi);

    }
}
