using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WinLib;

namespace FocusMaster.Pages.SettingsPages
{
    /// <summary>
    /// Interaction logic for ScreenSpacePage.xaml
    /// </summary>
    public partial class ScreenSpacePage : Page
    {
        public ScreenSpaceMap Map { get; set; }
        public ScreenSpacePage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Map = new ScreenSpaceMap(this.ScreenSpaceBorder);
            foreach (DisplayInfo display in DisplayHelper.displayList)
            {
                Screen screen = new Screen(Map, display);
                Map.Screens.Add(screen);
                this.ScreenSpaceMapGrid.Children.Add(screen.ScreenBorder);
                Map.Update();
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Map != null)
                Map.Update();
        }
    }
}
