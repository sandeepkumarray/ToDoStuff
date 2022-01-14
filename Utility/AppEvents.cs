using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff
{
    public static class AppEvents
    {
        public static event EventHandler<LogEventArgs> LogEvent;
        public static void ResetLogEvent() { LogEvent = null; }
        public static void OnLogEvent(object sender, LogEventArgs e)
        {
            if (LogEvent != null)
            {
                LogEvent(sender, e);
            }
        }
    }


    public class LogEventArgs : EventArgs
    {
        public string Message;

        public LogEventArgs()
        {

        }
    }
}
