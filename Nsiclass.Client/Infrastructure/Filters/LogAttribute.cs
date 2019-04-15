using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;

namespace BookTravel.Web.Infrastructure.Filters
{
    public class LogAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            using (var writer = new StreamWriter("logs.txt", true))
            {
                var dateTime = DateTime.UtcNow;
                var IpAddress = context.HttpContext.Connection.RemoteIpAddress;
                var userName = context.HttpContext.User?.Identity?.Name ?? "Anonymous";
                var controler = context.Controller.GetType().Name;
                var action = context.RouteData.Values["action"];

                var logMessage = $"{dateTime} - {IpAddress} - {userName} - {controler}.{action}";

                if (context.Exception != null)
                {
                    var exceptionType = context.Exception.GetType().Name;
                    var excMessage = context.Exception.Message;

                    logMessage = $"[!] {logMessage} {exceptionType} - {excMessage}";
                }

                writer.WriteLine(logMessage);
            }
            
        }
    }

}
