using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace TwilioEmulator.Code.MessageHandler
{
    // adapted from http://weblogs.asp.net/fredriknormen/archive/2012/06/09/log-message-request-and-response-in-asp-net-webapi.aspx
     

    public  class HttpLoggingMessageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            var corrId = string.Format("{0}{1}", DateTime.Now.Ticks, Thread.CurrentThread.ManagedThreadId);
            var requestInfo = string.Format("{0} {1}", request.Method, request.RequestUri);

            //var requestMessage = request.Content.ReadAsByteArrayAsync();

            HttpLoggingEventArgs lge = new HttpLoggingEventArgs()
            {
                Method = request.Method.Method,
                Uri=request.RequestUri.PathAndQuery,
                CorrelationID = corrId,
                Direction = "In"
            };

            FireEvent(lge);

            var response = base.SendAsync(request, cancellationToken);
            var result = response.Result;
            lge.Direction = "Out";
            if (!result.IsSuccessStatusCode)
            {
                lge.Result = result.ReasonPhrase;
            }
            else
            {
                lge.Result = "OK";
            }
            
           FireEvent(lge);

            return response;
        }

        public event EventHandler<HttpLoggingEventArgs>  OnHttpLog;
       
        public void FireEvent(HttpLoggingEventArgs args)
        {
            Task.Factory.StartNew(() =>
                {
                    if (OnHttpLog != null)
                    {
                        OnHttpLog(this,args);
                    }
                });
        }

    }

    public class HttpLoggingEventArgs:EventArgs
    {
        public string Method { get; set; }
        public string Uri { get; set; }
        public string CorrelationID { get; set; }
        public string Direction {get;set;}
        public string Result {get;set;}
       
    }

    
}
