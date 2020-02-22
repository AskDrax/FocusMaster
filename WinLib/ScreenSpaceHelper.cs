using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WinLib
{
    public static class ScreenSpaceHelper
    {
        public static ObservableCollection<ScreenSpaceMap> screenSpaceMapList { get; set; }

        public static void Initialize(IEnumerable<ScreenSpaceMap> maps)
        {
            foreach (ScreenSpaceMap map in maps)
            {
                
            }

            foreach (DisplayInfo display in DisplayHelper.displayList)
            {
                //Screen newScreen = new Screen(display);
            }
        }
    }
}
