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
using FocusMaster.Controls;

namespace FocusMaster.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        public NavigationService NavService;
        public SettingsPages.HomePage HomePage;
        public SettingsPages.DisplaysPage DisplaysPage;
        public SettingsPages.WindowsPage WindowsPage;
        public SettingsPages.AutomationPage AutomationPage;
        public SettingsPages.LogsPage LogsPage;
        public SettingsPages.ExperimentalPage ExperimentalPage;

        private void SelectNav(object sender, RoutedEventArgs e)
        {
            ImageTextButton itb = sender as ImageTextButton;
            StackPanel sp = itb.Parent as StackPanel;

            foreach (ImageTextButton it in sp.Children)
            {
                if (it != itb)
                    it.IsChecked = false;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            HomePage = new SettingsPages.HomePage();
            DisplaysPage = new SettingsPages.DisplaysPage();
            WindowsPage = new SettingsPages.WindowsPage();
            AutomationPage = new SettingsPages.AutomationPage();
            LogsPage = new SettingsPages.LogsPage();
            ExperimentalPage = new SettingsPages.ExperimentalPage();

            NavService = SettingsFrame.NavigationService;
            SettingsFrame.Navigate(HomePage);
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            SelectNav(sender, e);
            NavService.Navigate(HomePage);
        }

        private void DisplaysButton_Click(object sender, RoutedEventArgs e)
        {
            SelectNav(sender, e);
            NavService.Navigate(DisplaysPage);
        }

        private void WindowsButton_Click(object sender, RoutedEventArgs e)
        {
            SelectNav(sender, e);
            NavService.Navigate(WindowsPage);
        }

        private void AutomationButton_Click(object sender, RoutedEventArgs e)
        {
            SelectNav(sender, e);
            NavService.Navigate(AutomationPage);
        }

        private void LogsButton_Click(object sender, RoutedEventArgs e)
        {
            SelectNav(sender, e);
            NavService.Navigate(LogsPage);
        }

        private void ExperimentalButton_Click(object sender, RoutedEventArgs e)
        {
            SelectNav(sender, e);
            NavService.Navigate(ExperimentalPage);
        }
    }
}
