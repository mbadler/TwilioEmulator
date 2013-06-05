using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Http;
using System.Web.Http.SelfHost;
using TwilioEmulator;
using TwilioEmulator.Properties;

namespace TwilioEmulator.Code
{
    public class SystemController : IDisposable
    {
        private static HttpSelfHostServer server = (HttpSelfHostServer)null;
        private static readonly Lazy<SystemController> _instance = new Lazy<SystemController>((Func<SystemController>)(() => new SystemController()));
        public List<CallInstance> CallList = new List<CallInstance>();

        public Form1 theForm { get; set; }

        public int ActivePort { get; set; }

        public static SystemController Instance
        {
            get
            {
                return SystemController._instance.Value;
            }
        }

        static SystemController()
        {
        }

        private SystemController()
        {
        }

        private void StartWebServer()
        {
            string port = Settings.Default.Port;
            HttpSelfHostConfiguration configuration = new HttpSelfHostConfiguration("http://localhost:" + port);
            string name1 = "Default";
            string routeTemplate1 = "2010-04-01/Accounts/{sid}/{controller}.json/{id}";
            object defaults1 = (object)new
            {
                id = RouteParameter.Optional
            };
            HttpRouteCollectionExtensions.MapHttpRoute(configuration.Routes, name1, routeTemplate1, defaults1);
            string name2 = "Direct";
            string routeTemplate2 = "2010-04-01/Accounts/{sid}/{controller}/{id}";
            object defaults2 = (object)new
            {
                id = RouteParameter.Optional
            };
            HttpRouteCollectionExtensions.MapHttpRoute(configuration.Routes, name2, routeTemplate2, defaults2);
            HttpSelfHostServer httpSelfHostServer = new HttpSelfHostServer(configuration);
            try
            {
                httpSelfHostServer.OpenAsync().Wait();
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException.Message.Contains("Your process does not have access rights to this namespace"))
                    throw new Exception("You don't have rights to open this port - Try running as administrator or giving permission via NETSTAT", (Exception)ex);
                else
                    throw ex;
            }
            this.ActivePort = int.Parse(port);
        }

        public void StartUp()
        {
            this.StartWebServer();
        }

        public void Dispose()
        {
            SystemController.server.Dispose();
        }

        public void LogServerMessage(string Text)
        {
            this.theForm.LogServerMessage(Text);
        }

        public void LogObjectDump(object obj, string name)
        {
            StringWriter stringWriter = new StringWriter();
            ObjectDumper2.Dump(obj, name, (TextWriter)stringWriter, true);
            this.LogServerMessage(stringWriter.ToString());
        }
    }
}