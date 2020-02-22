using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.Windows.Media;
using WinLib;
using System.Collections.ObjectModel;

namespace WinLib
{
    public class ScreenSpaceMap
    {
        public Border MapBorder { get; set; }
        public ObservableCollection<Screen> Screens { get; set; }

        public double Width
        {
            get { return MapBorder.ActualWidth; }
        }
        public double Height
        {
            get { return MapBorder.ActualHeight; }
        }

        public double SourceWidth { get; set; }
        public double SourceHeight { get; set; }

        public double WidthScale
        {
            get { return Width / SourceWidth; }
        }
        public double HeightScale
        {
            get { return Height / SourceHeight; }
        }

        public ScreenSpaceMap(Border mapBorder)
        {
            MapBorder = mapBorder;
            Screens = new ObservableCollection<Screen>();
            //Shouldn't always set these from VirtualScreen, for example if we wanted a Map of only 1 of 2 Screens etc.
            SourceWidth = DisplayHelper.VirtualScreenWidth;
            SourceHeight = DisplayHelper.VirtualScreenHeight;
         
        }

        public void Update()
        {
            foreach (Screen screen in Screens)
            {
                screen.Update();
            }
        }
    }


}
