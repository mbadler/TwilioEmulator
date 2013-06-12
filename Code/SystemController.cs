using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Http;
using System.Web.Http.SelfHost;
using TwilioEmulator;
using TwilioEmulator.Properties;
using Twilio;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TwilioEmulator.Offices;


namespace TwilioEmulator.Code
{
    public class SystemController : IDisposable
    {
        private static HttpSelfHostServer server = (HttpSelfHostServer)null;

        #region Static 
        private static readonly Lazy<SystemController> _instance = new Lazy<SystemController>((Func<SystemController>)(() => new SystemController()));
        private static readonly Lazy<Office> _office = new Lazy<Office>((Func<Office>)(() => new Office()));

        public static bool ConsoleMode { get; set; }

        public ILogger Logger { get; set; }

        public static SystemController Instance
        {
            get
            {
                return SystemController._instance.Value;
            }
        }

        public static Office Office
        {
            get
            {
                return SystemController._office.Value;
            }
        }

       

        #endregion

        public MainForm theForm { get; set; }

        public int ActivePort { get; set; }

        private static int _waitInterval = 1000;

        public static int WaitInterval
        {
            get { return _waitInterval; }
            set { _waitInterval = value; }
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

            JsonMediaTypeFormatter jsonFormatter = configuration.Formatters.JsonFormatter;
            JsonSerializerSettings jSettings = new Newtonsoft.Json.JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            jSettings.Converters.Add(new TwilioDateTimeConvertor());
            jsonFormatter.SerializerSettings = jSettings;

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

       

        
    }

    public class TwilioDateTimeConvertor : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return DateTime.Parse(reader.Value.ToString());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToString("ddd, dd MMM yyyy HH:mm:ss '+0000'"));
        }
    }
}