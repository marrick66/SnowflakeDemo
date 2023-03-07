using System.Diagnostics;

namespace Demo.API.Middleware
{
    /// <summary>
    /// Crude middleware to calculate the elapsed time of the API endpoint
    /// and log it.
    /// </summary>
    public class LogExecutionMiddleware
    {
        private readonly RequestDelegate _next;

        public LogExecutionMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var logger = context
                .RequestServices
                .GetService<ILogger<LogExecutionMiddleware>>();

            long start = default;

            var path = context.Request.Path;

            if(logger != null)
                start = Stopwatch.GetTimestamp();

            try
            {
                await _next(context);
            }
            finally
            {
                if(logger != null)
                {
                    var elapsed = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());

                    using(logger.BeginScope(new Dictionary<string, object>
                        {
                            { "Path", path },
                            { "StatusCode", context.Response.StatusCode },
                            { "Elapsed",  elapsed }
                        }))
                    {
                        logger.LogInformation("HTTP method executed.");
                    }
                }
            }

        }

        private static double GetElapsedMilliseconds(long start, long end)
        {
            return (end - start) * 1000 / (double)Stopwatch.Frequency;
        }
    }
}
