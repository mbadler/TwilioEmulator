using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwilioEmulator.Code
{
    public static class TwilioCallStatuses
    {
        public static string QUEUED = "queued";
        public static string RINGING = "ringing";
        public static string INPROGRESS = "in-progress";
        public static string COMPLETED = "completed";
        public static string BUSY = "busy";
        public static string FAILED = "failed";
        public static string NOANSWER = "no-answer";
    }
}
