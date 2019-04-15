using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Diagnostics;
using System.IO;

namespace BookTravel.Web.Infrastructure.Filters
{
    public class MeasureTimeAttribute : ActionFilterAttribute
    {
        private Stopwatch stopWatch;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            stopWatch = new Stopwatch();
            stopWatch.Start();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            stopWatch.Stop();
            using (var writer = new StreamWriter("action-times.txt", true))
            {
                var dateTime = DateTime.UtcNow;
                var controler = context.Controller.GetType().Name;
                var action = context.RouteData.Values["action"];
                var timeElapsed = stopWatch.Elapsed;

                var logMessage = $"{dateTime} – {controler}.{action} – {timeElapsed}";

                writer.WriteLine(logMessage);
            }
        }
    }
}
