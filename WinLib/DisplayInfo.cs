using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinLib
{
    public class DisplayInfo
    {
        public string DeviceName { get; set; }
        public string DeviceID { get; set; }
        public string DeviceFriendlyName { get; set; }
        public string Manufacturer { get; set; }
        public string IsPrimaryDisplay { get; set; }
        public string ScreenHeight { get; set; }
        public string ScreenWidth { get; set; }
        public string ScreenResolution { get; set; }
        public RECT MonitorArea { get; set; }
        public RECT WorkArea { get; set; }
    }
}
