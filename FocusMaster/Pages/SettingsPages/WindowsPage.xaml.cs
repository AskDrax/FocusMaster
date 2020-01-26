using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WinLib;
using System.Collections.ObjectModel;

namespace FocusMaster.Pages.SettingsPages
{
    /// <summary>
    /// Interaction logic for WindowsPage.xaml
    /// </summary>
    public partial class WindowsPage : Page
    {
        public WindowsPage()
        {
            InitializeComponent();

            MainWindow = (MainWindow)Application.Current.MainWindow;
        }

        public MainWindow MainWindow { get; set; }

        public AWindow selectedWindow { get; set; }

        private void windowListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedWindow = windowListBox.SelectedItem as AWindow;

            IconImage.Source = selectedWindow.IconImage;
            TitleText.Text = selectedWindow.Title;
            HWNDText.Text = selectedWindow.Hwnd.ToString();

            StylesListBox.Items.Clear();

            foreach (KeyValuePair<string, uint> style in selectedWindow.WindowStyles.Styles)
            {
                TextBlock item = new TextBlock();
                item.Text = style.Key + " (" + style.Value.ToString() + ")";
                StylesListBox.Items.Add(item);
            }

            ExStylesListBox.Items.Clear();

            foreach (KeyValuePair<string, uint> exstyle in selectedWindow.WindowStyles.ExStyles)
            {
                TextBlock item = new TextBlock();
                item.Text = exstyle.Key + " (" + exstyle.Value.ToString() + ")";
                ExStylesListBox.Items.Add(item);
            }
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await refreshList();
            VisualStateManager.GoToState(RefreshButton, "Unhovered", true);
        }

        private async Task<bool> refreshList()
        {
            await MainWindow.Dispatcher.InvokeAsync(() => WindowHelper.EnumAllWindows());
            return true;
        }

        private void ListenForOpenCloseButton_Checked(object sender, RoutedEventArgs e)
        {
            WindowHelper.ListenForWindowOpen();
            WindowHelper.ListenForWindowClose();
        }

        private void ListenForOpenCloseButton_Unchecked(object sender, RoutedEventArgs e)
        {
            WindowHelper.CloseAutomationEvents();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
