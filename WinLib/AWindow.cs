using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Media;

namespace WinLib
{
    public class AWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Guid guid;
        public Guid GUID
        {
            get { return guid; }
            set 
            { 
                guid = value;
                NotifyPropertyChanged("guid");
            }
        }

        private IntPtr hwnd;
        public IntPtr Hwnd
        {
            get { return hwnd; }
            set
            {
                hwnd = value;
                NotifyPropertyChanged("hwnd");
            }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                NotifyPropertyChanged("title");
            }
        }

        private string className;
        public string ClassName
        {
            get { return className; }
            set
            {
                className = value;
                NotifyPropertyChanged("className");
            }
        }

        private string baseClassName;
        public string BaseClassName
        {
            get { return baseClassName; }
            set
            {
                baseClassName = value;
                NotifyPropertyChanged("baseClassName");
            }
        }

        private uint threadProcessId;
        public uint ThreadProcessId
        {
            get { return threadProcessId; }
            set
            {
                threadProcessId = value;
                NotifyPropertyChanged("threadProcessId");
            }
        }

        private GUITHREADINFO guiThreadInfo;
        public GUITHREADINFO GuiThreadInfo
        {
            get { return guiThreadInfo; }
            set
            {
                guiThreadInfo = value;
                NotifyPropertyChanged("guiThreadInfo");
            }
        }

        private IntPtr savedParent;
        public IntPtr SavedParent
        {
            get { return savedParent; }
            set
            {
                savedParent = value;
                NotifyPropertyChanged("savedParent");
            }
        }

        private IntPtr savedStyle;
        public IntPtr SavedStyle
        {
            get { return savedStyle; }
            set
            {
                savedStyle = value;
                NotifyPropertyChanged("savedStyle");
            }
        }

        private IntPtr savedStyleEx;
        public IntPtr SavedStyleEx
        {
            get { return savedStyleEx; }
            set
            {
                savedStyleEx = value;
                NotifyPropertyChanged("savedStyleEx");
            }
        }

        private WindowStyles windowStyles;
        public WindowStyles WindowStyles
        {
            get { return windowStyles; }
            set
            {
                windowStyles = value;
                NotifyPropertyChanged("windowStyles");
            }
        }

        private AutomationElement automationElement;
        public AutomationElement AutomationElement
        {
            get { return automationElement; }
            set
            {
                automationElement = value;
                NotifyPropertyChanged("automationElement");
            }
        }

        private WINDOWPLACEMENT placement;
        public WINDOWPLACEMENT Placement
        {
            get { return placement; }
            set
            {
                placement = value;
                NotifyPropertyChanged("placement");
            }
        }

        private WINDOWPLACEMENT lastPlacement;
        public WINDOWPLACEMENT LastPlacement
        {
            get { return lastPlacement; }
            set
            {
                lastPlacement = value;
                NotifyPropertyChanged("lastPlacement");
            }
        }

        private RECT savedRECT;
        public RECT SavedRECT
        {
            get { return savedRECT; }
            set
            {
                savedRECT = value;
                NotifyPropertyChanged("savedRECT");
            }
        }

        private RECT niceRECT;
        public RECT NiceRECT
        {
            get { return niceRECT; }
            set
            {
                niceRECT = value;
                NotifyPropertyChanged("niceRECT");
            }
        }

        private RECT borderRECT;
        public RECT BorderRECT
        {
            get { return borderRECT; }
            set
            {
                borderRECT = value;
                NotifyPropertyChanged("borderRECT");
            }
        }

        private RECT minRECT;
        public RECT MinRECT
        {
            get { return minRECT; }
            set
            {
                minRECT = value;
                NotifyPropertyChanged("minRECT");
            }
        }

        private Icon icon;
        public Icon Icon
        {
            get { return icon; }
            set
            {
                icon = value;
                NotifyPropertyChanged("icon");
            }
        }

        private ImageSource iconImage;
        public ImageSource IconImage
        {
            get { return iconImage; }
            set
            {
                iconImage = value;
                NotifyPropertyChanged("iconImage");
            }
        }

        private WINDOWINFO info;
        public WINDOWINFO Info
        {
            get { return info; }
            set
            {
                info = value;
                NotifyPropertyChanged("info");
            }
        }

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
