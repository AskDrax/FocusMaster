using System;
using System.Collections.Generic;
using System.Text;

namespace FocusMaster
{
    public class LogEntry
    {
        public LogEntry(LogEntryType type, string text)
        {
            LogEntryType = type;
            TimeStamp = DateTime.Now;
            EntryText = text;
            FullText = "[ " + TimeStamp.ToString() + " ] [ " + LogEntryType.ToString() + " ] [ " + EntryText + " ]";
        }

        public LogEntryType LogEntryType;
        public DateTime TimeStamp;
        public string EntryText;

        public string FullText { get; set; }

        public override string ToString()
        {
            return FullText;
        }
    }
    public enum LogEntryType
    {
        None,
        WindowsEvent,
        ApplicationEvent
    }
}
