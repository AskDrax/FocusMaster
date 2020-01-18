using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;
using WinLib;

namespace FocusMaster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public IntPtr HWND;

        public NavigationService NavService;
        public Pages.StartPage StartPage;
        public Pages.SettingsPage SettingsPage;
        public Pages.LogPage LogPage;

        public EventManager EventManager;

        #region ControlBox
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState != WindowState.Maximized)
            {
                WindowState = WindowState.Maximized;

            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            //overshoot fix
            if (WindowState == WindowState.Maximized)
            {
                BorderBrush = new SolidColorBrush(Colors.Transparent);
                BorderThickness = new Thickness(7, 7, 7, 7);
            }
            else
            {
                BorderBrush = (SolidColorBrush)Application.Current.FindResource("WindowBorderBrush");
                BorderThickness = new Thickness(1, 1, 1, 1);
            }
        }

        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HWND = new WindowInteropHelper(this).Handle;
            WindowHelper.FixWindowStyle(HWND);

            StartPage = new Pages.StartPage();
            SettingsPage = new Pages.SettingsPage();
            LogPage = new Pages.LogPage();

            EventManager = new EventManager();
            EventManager.Initialize();

            NavService = ContentFrame.NavigationService;
            ContentFrame.Navigate(StartPage);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(SettingsPage);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            EventManager.Dispose();
        }
    }
}
