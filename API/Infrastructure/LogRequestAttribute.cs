using System;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using TwilioEmulator.Code;

namespace TwilioEmulator.API.Infrastructure
{
    public class LogRequestAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            Task.Factory.StartNew((Action)(() => SystemController.Instance.LogServerMessage(filterContext.Request.RequestUri.PathAndQuery)));
        }
    }
}
