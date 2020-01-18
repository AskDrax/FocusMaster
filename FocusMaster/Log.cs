using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace FocusMaster
{
    public class Log
    {
        public Log()
        {
            MainWindow = (MainWindow)Application.Current.MainWindow;

            LogPage = MainWindow.LogPage;

            LogEntries = new ObservableCollection<LogEntry>();

            FilterBy = new LogEntryType[]
            {
                LogEntryType.None,
                LogEntryType.WindowsEvent,
                LogEntryType.ApplicationEvent
            };

            FilterView = (ListCollectionView)CollectionViewSource.GetDefaultView(LogEntries);
            
        }

        public MainWindow MainWindow { get; set; }
        public Pages.LogPage LogPage { get; set; }

        public ObservableCollection<LogEntry> LogEntries { get; set; }
        public LogEntryType[] FilterBy { get; set; }

        public ListCollectionView FilterView;

        public void Add(LogEntryType type, string text)
        {
            LogEntry previousLogEntry;

            if (LogEntries.Count == 0)
                LogEntries.Add(new LogEntry(LogEntryType.ApplicationEvent, "Log Started..."));

            previousLogEntry = LogEntries.Last();
                
            LogEntry newLogEntry = new LogEntry(type, text);
            if (newLogEntry.EntryText != previousLogEntry.EntryText)
                LogEntries.Add(newLogEntry);

            LogPage.ScrollToCurrent();
        }

        public bool EventTypeFilter(object logEntry)
        {
            LogEntry entry = logEntry as LogEntry;

            if (FilterBy.Contains(LogEntryType.None) && entry.LogEntryType == LogEntryType.None)
                return true;

            if (FilterBy.Contains(LogEntryType.WindowsEvent) && entry.LogEntryType == LogEntryType.WindowsEvent)
                return true;

            if (FilterBy.Contains(LogEntryType.ApplicationEvent) && entry.LogEntryType == LogEntryType.ApplicationEvent)
                return true;

            return false;
        }
    }
}
