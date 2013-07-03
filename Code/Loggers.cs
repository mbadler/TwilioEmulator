using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace TwilioEmulator.Code
{
    public interface ILogger
    {
        void LogObj(LogObj logObj);
        
    }

    public static class LoggerExtension
    {
        public static string GetObjectText(this ILogger lg, object obj)
        {
            StringWriter stringWriter = new StringWriter();
            ObjectDumper2.Dump(obj, "", (TextWriter)stringWriter, true);
            return stringWriter.ToString();

        }

        
    }


    public class LogObj
    {

       
        public string Caption { get; set; }
        public CallInstance CallInstance { get; set; }
        public LogSymbol LogSymbol { get; set; }
        public List<KeyValuePair<string, object>> logObjs = new List<KeyValuePair<string, object>>();
        public bool IsInError { get; set; }
        public LogObj AddNode(string Key, object value)
        {
            logObjs.Add(new KeyValuePair<string, object>(Key, value));
            return this;
        }
        public LogObj LogIt()
        {
            SystemController.Instance.Logger.LogObj(this);
            return this;
        }

        public LogObj AddException(Exception ex)
        {
            string Ex = ex.ToString();
            logObjs.Add(new KeyValuePair<string, object>("Exception", ex));
            return this;
        }
    }

  

   
}
