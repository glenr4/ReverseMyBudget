using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace ReverseMyBudget
{
    public class LoggingExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public LoggingExceptionFilter(ILogger logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.Error(context.Exception, "Uncaught Exception");
        }
    }
}