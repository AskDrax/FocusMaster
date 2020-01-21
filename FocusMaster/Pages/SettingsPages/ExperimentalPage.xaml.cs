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

namespace FocusMaster.Pages.SettingsPages
{
    /// <summary>
    /// Interaction logic for ExperimentalPage.xaml
    /// </summary>
    public partial class ExperimentalPage : Page
    {
        public ExperimentalPage()
        {
            InitializeComponent();
        }

        private void NoClickOnlyFocusCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            EventManager.NoClickOnlyFocus = true;
            NoFocusOnlyClickCheckBox.IsChecked = false;
        }

        private void NoClickOnlyFocusCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            EventManager.NoClickOnlyFocus = false;
        }

        private void NoFocusOnlyClickCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            EventManager.NoFocusOnlyClick = true;
            NoClickOnlyFocusCheckBox.IsChecked = false;
        }

        private void NoFocusOnlyClickCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            EventManager.NoFocusOnlyClick = false;
        }
    }
}
