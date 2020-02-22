using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Controls;
using WinLib;
using System.Windows.Media;
using System.Windows;

namespace WinLib
{
    public class Screen
    {
        public ScreenSpaceMap ParentMap { get; set; }
        public Border ScreenBorder { get; set; }
        public System.Drawing.Point TopLeft { get; set; }
        public System.Drawing.Point TopRight { get; set; }
        public System.Drawing.Point BottomLeft { get; set; }
        public System.Drawing.Point BottomRight { get; set; }
        public RECT Rect;

        public double Width { get; set; }
        public double Height { get; set; }

        public int ActualLeft
        {
            get { return Rect.Left; }
            set { Rect.Left = value; }
        }
        public int ActualTop
        {
            get { return Rect.Top; }
            set { Rect.Top = value; }
        }
        public int ActualRight
        {
            get { return Rect.Right; }
            set { Rect.Right = value; }
        }
        public int ActualBottom
        {
            get { return Rect.Bottom; }
            set { Rect.Bottom = value; }
        }

        public double ActualWidth
        {
            get { return ActualRight - ActualLeft; }
        }
        public double ActualHeight
        {
            get { return ActualBottom - ActualTop; }
        }
        
        public double MappedLeft
        {
            get { return (ActualLeft * ParentMap.WidthScale); }
        }
        public double MappedTop
        {
            get { return (ActualTop * ParentMap.HeightScale); }
        }
        public double MappedRight
        {
            get { return (ActualRight * ParentMap.WidthScale); }
        }
        public double MappedBottom
        {
            get { return (ActualBottom * ParentMap.HeightScale); }
        }
        public double MappedWidth
        {
            get { return MappedRight - MappedLeft; }
        }
        public double MappedHeight
        {
            get { return MappedBottom - MappedTop; }
        }

        public Screen(ScreenSpaceMap parentMap, DisplayInfo display)
        {
            ParentMap = parentMap;

            ScreenBorder = new Border();
            ScreenBorder.BorderThickness = new Thickness(4.0f);
            ScreenBorder.BorderBrush = (SolidColorBrush)Application.Current.FindResource("ButtonHighlightBrush");
            ScreenBorder.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(100, 50, 50, 50));
            ScreenBorder.HorizontalAlignment = HorizontalAlignment.Left;
            ScreenBorder.VerticalAlignment = VerticalAlignment.Top;

            Rect = display.MonitorArea;
            TopLeft = new System.Drawing.Point(Rect.Left, Rect.Top);
            TopRight = new System.Drawing.Point(Rect.Right, Rect.Top);
            BottomLeft = new System.Drawing.Point(Rect.Left, Rect.Bottom);
            BottomRight = new System.Drawing.Point(Rect.Right, Rect.Bottom);
            Width = Convert.ToDouble(display.ScreenWidth);
            Height = Convert.ToDouble(display.ScreenHeight);
            //Width = display.MonitorArea.Right - display.MonitorArea.Left;
            //Height = display.MonitorArea.Bottom - display.MonitorArea.Top;
        }

        public void Update()
        {
            ScreenBorder.Width = MappedWidth;
            ScreenBorder.Height = MappedHeight;

            TranslateTransform tt = new TranslateTransform();
            tt.X = MappedLeft;
            tt.Y = MappedTop;

            if (tt.X < 0)
            {
                double offsetx = (0 - tt.X);
                foreach (Screen screen in ParentMap.Screens)
                {
                    TranslateTransform ttt = new TranslateTransform();
                    ttt.X = offsetx;
                    ttt.Y = 0;
                    screen.ScreenBorder.RenderTransform = ttt;
                }
                tt.X = 0;
            }
            

            if (tt.Y < 0)
            {
                double offsety = (0 - tt.Y);
                foreach (Screen screen in ParentMap.Screens)
                {
                    TranslateTransform ttt = new TranslateTransform();
                    ttt.X = 0;
                    ttt.Y = offsety;
                    screen.ScreenBorder.RenderTransform = ttt;
                }
                tt.Y = 0;
            }
            
            ScreenBorder.RenderTransform = tt;
        }
    }
}
