using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinLib
{
    public class DisplayInfoCollection : ObservableCollection<DisplayInfo>
    {
        public DisplayInfoCollection GetDisplays()
        {
            DisplayInfoCollection col = new DisplayInfoCollection();

            DisplayHelper.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero,
                delegate (IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData)
                {
                    MONITORINFOEX mi = new MONITORINFOEX();
                    mi.Size = (uint)Marshal.SizeOf(mi);
                    bool success = DisplayHelper.GetMonitorInfo(hMonitor, ref mi);
                    if (success)
                    {
                        DisplayInfo di = new DisplayInfo();
                        di.ScreenWidth = (mi.Monitor.Right - mi.Monitor.Left).ToString();
                        di.ScreenHeight = (mi.Monitor.Bottom - mi.Monitor.Top).ToString();
                        di.MonitorArea = mi.Monitor;
                        di.WorkArea = mi.WorkArea;
                        di.IsPrimaryDisplay = mi.Flags.ToString();
                        di.DeviceName = mi.DeviceName;
                        di.ScreenResolution = di.ScreenWidth + " x " + di.ScreenHeight;
                        di.ScreenTopLeft = di.MonitorArea.Left.ToString() + "," + di.MonitorArea.Top.ToString();
                        col.Add(di);
                    }
                    return true;
                }, IntPtr.Zero);

            IEnumerable<DisplayDetails> dd = DisplayDetails.GetMonitorDetails().OrderBy(d => d.Address);

            if (dd.Count() >= col.Count())
            {
                int current = 0;
                foreach (DisplayDetails d in dd)
                {
                    col[current].DeviceFriendlyName = d.Model;
                    col[current].DeviceID = d.MonitorID;
                    col[current].Manufacturer = d.Manufacturer;

                    current++;
                }
            }

            return col;
        }

        private string GetFriendlyName(string deviceName)
        {
            string output = "";

            foreach (var display in DisplayDetails.GetMonitorDetails())
            {
                output = "Model = " + display.Model.ToString() + ", MonitorID = " + display.MonitorID.ToString() + " | ";
            }

            return output;
        }
    }
}
