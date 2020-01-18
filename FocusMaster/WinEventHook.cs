using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using WinLib;

namespace FocusMaster
{
    public class WinEventHook
    {
        public WinEventHook(string eventName, EventManager.Handler handler)
        {
            EventMin = GetEventIdFromEventName(eventName);
            EventMax = EventMin;
            HmodWinEventProc = IntPtr.Zero;
            IdProcess = 0;
            IdThread = 0;
            DwFlags = 0;

            EventMinString = eventName;
            EventMaxString = eventName;

            Handle = IntPtr.Zero;
            eventHandler = new EventManager.Handler(handler);
        }
        public WinEventHook(uint eventId, EventManager.Handler handler)
        {
            EventMin = eventId;
            EventMax = eventId;
            HmodWinEventProc = IntPtr.Zero;
            IdProcess = 0;
            IdThread = 0;
            DwFlags = 0;

            EventMinString = GetEventNameFromEventId(eventId);
            EventMaxString = EventMinString;

            Handle = IntPtr.Zero;
            eventHandler = new EventManager.Handler(handler);
        }
        public WinEventHook(string eventMin, string eventMax, EventManager.Handler handler)
        {
            EventMin = GetEventIdFromEventName(eventMin);
            EventMax = GetEventIdFromEventName(eventMax);
            HmodWinEventProc = IntPtr.Zero;
            IdProcess = 0;
            IdThread = 0;
            DwFlags = 0;

            EventMinString = eventMin;
            EventMaxString = eventMax;

            Handle = IntPtr.Zero;
            eventHandler = new EventManager.Handler(handler);
        }
        public WinEventHook(uint eventMin, uint eventMax, EventManager.Handler handler)
        {
            EventMin = eventMin;
            EventMax = eventMax;
            HmodWinEventProc = IntPtr.Zero;
            IdProcess = 0;
            IdThread = 0;
            DwFlags = 0;

            EventMinString = GetEventNameFromEventId(eventMin);
            EventMaxString = GetEventNameFromEventId(eventMax);

            Handle = IntPtr.Zero;
            eventHandler = new EventManager.Handler(handler);
        }
        
        public uint EventMin;
        public uint EventMax;
        public IntPtr HmodWinEventProc;
        public uint IdProcess;
        public uint IdThread;
        public uint DwFlags;

        public string EventMinString;
        public string EventMaxString;
        public IntPtr Handle;

        public EventManager.Handler eventHandler;

        public bool IsHooked()
        {
            if (Handle != IntPtr.Zero)
                return true;
            return false;
        }

        public string GetEventNameFromEventId(uint eventId)
        {
            BindingFlags bindingFlags = BindingFlags.Static;

            foreach (FieldInfo field in typeof(EV).GetFields(bindingFlags))
            {
                uint val = (uint)field.GetRawConstantValue();
                if (val == eventId)
                    return field.Name;
            }
            return null;
        }

        public uint GetEventIdFromEventName(string name)
        {
            BindingFlags bindingFlags = BindingFlags.Static;

            foreach (FieldInfo field in typeof(EV).GetFields(bindingFlags))
            {
                if (name == field.Name)
                    return (uint)field.GetRawConstantValue();
            }
            return 0;
        }
    }
}
