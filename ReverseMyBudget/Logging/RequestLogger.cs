using Microsoft.AspNetCore.Http;
using ReverseMyBudget.Domain;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ReverseMyBudget
{
    public class RequestLogger
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        // If using Application Insights, then remove UserId and TraceId as they will be included in 'customDimensions'
        private readonly string _messageTemplate = "{requestMethod} {requestPath} from {RemoteIP} => {statusCode} in {responseTime:0.000} ms UserId: {UserId} TraceId: {TraceId}";

        public RequestLogger(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        // Must inject IUserProvider here as it is scoped
        public async Task Invoke(HttpContext httpContext, IUserProvider userProvider)
        {
            var sw = Stopwatch.StartNew();

            // Add ASPNET trace ID as a property on all logged messages
            using (LogContext.PushProperty("TraceID", httpContext.TraceIdentifier))
            using (LogContext.Push(new UserLogEnricher(userProvider)))
            {
                try
                {
                    await _next(httpContext);

                    sw.Stop();

                    int? statusCode = httpContext.Response?.StatusCode;
                    var level = statusCode >= 500 ? LogEventLevel.Error
                        : statusCode >= 400 ? LogEventLevel.Warning
                        : LogEventLevel.Debug;

                    _logger.Write(level, _messageTemplate, httpContext.Request.Method, httpContext.Request.Path, httpContext.Connection.RemoteIpAddress, statusCode, sw.Elapsed.TotalMilliseconds);
                }
                catch (Exception ex)
                {
                    sw.Stop();

                    _logger.Error(nameof(RequestLogger), ex);
                }
            }
        }
    }
}