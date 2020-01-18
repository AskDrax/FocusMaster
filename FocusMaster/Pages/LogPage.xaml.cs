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

namespace FocusMaster.Pages
{
    /// <summary>
    /// Interaction logic for LogPage.xaml
    /// </summary>
    public partial class LogPage : Page
    {
        public LogPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            logListBox.ItemsSource = EventManager.CurrentLog.FilterView;
        }

        public void ScrollToCurrent()
        {
            logListBox.SelectedIndex = logListBox.Items.Count - 1;
            logListBox.ScrollIntoView(logListBox.SelectedItem);
        }
    }
}
