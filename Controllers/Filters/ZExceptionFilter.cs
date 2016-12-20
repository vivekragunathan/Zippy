using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Zippy.Controllers.Contract;
using Zippy.Utils;

namespace Zippy.Controllers.Filters
{
    public class ZExceptionFilter : ActionFilterAttribute, IExceptionFilter
    {
        private readonly ILogger logger;

        public ZExceptionFilter(ILoggerFactory factory)
        {
            Throw.IfNull(factory, $"{nameof(factory)} cannot be null");
            this.logger = factory.CreateLogger<ZExceptionFilter>();
        }

        public void OnException(ExceptionContext context)
        {
            var payload = new ZResponse(context.Exception);
            context.Result = new JsonResult(payload, Helpers.DefaultJsonSettings);

            this.logger.LogError($"Error processing request. {context.Exception}");
        }
    }
}
