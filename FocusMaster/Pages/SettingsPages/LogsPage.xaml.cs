﻿using System;
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
    /// Interaction logic for LogsPage.xaml
    /// </summary>
    public partial class LogsPage : Page
    {
        public LogsPage()
        {
            InitializeComponent();
        }

        public MainWindow MainWindow { get; set; }

        public NavigationService NavService;
        public LogPage LogPage;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow = (MainWindow)Application.Current.MainWindow;

            LogPage = MainWindow.LogPage;

            NavService = LogFrame.NavigationService;
            LogFrame.Navigate(LogPage);
        }
    }
}
