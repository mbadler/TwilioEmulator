using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace TwilioEmulator.Code
{
    public interface ILogger
    {
        void LogSimpleObject(object obj, string name);
        void LogDictionaryOfObjects(string name ,Dictionary<string, object> LogObj);
        void LogLine(string text);
    }

    public static class LoggerExtension
    {
        public static string GetObjectText(this ILogger lg, object obj)
        {
            StringWriter stringWriter = new StringWriter();
            ObjectDumper2.Dump(obj, "", (TextWriter)stringWriter, true);
            return stringWriter.ToString();

        }

        public static void Log2Nodes(this ILogger l,string name,string Node1Name,object node1object,string Node2Name,object node2object,bool LogAsError)
        {
            var a = new Dictionary<string, object>();
            a.Add(Node1Name, node1object);
            a.Add(Node2Name, node2object);
            l.LogDictionaryOfObjects(name, a);
        }
    }

    public class ConsoleLogger : ILogger
    {
        public void LogObject(object obj, string name)
        {
            Console.WriteLine("-- " + DateTime.Now.ToString() + " " + name);
            Console.WriteLine(this.GetObjectText(obj));
        }


    
        void ILogger.LogSimpleObject(object obj, string name)
        {
            throw new NotImplementedException();
        }

        void ILogger.LogDictionaryOfObjects(string name, Dictionary<string, object> LogObj)
        {
            throw new NotImplementedException();
        }

        void ILogger.LogLine(string text)
        {
            throw new NotImplementedException();
        }
    }

   
}
