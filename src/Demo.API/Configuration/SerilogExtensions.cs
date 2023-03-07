using Demo.API.Middleware;
using Serilog;
using Serilog.Filters;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Kafka;

namespace Demo.API.Configuration
{
    public static class SerilogExtensions
    {
        public static IServiceCollection AddSerilogKafka(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var options = configuration
                .GetSection("SerilogKafka")
                .Get<KafkaSinkOptions>();

            var formatter = new RenderedCompactJsonFormatter();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Filter.ByIncludingOnly(Matching.FromSource<LogExecutionMiddleware>())
                .Enrich.FromLogContext()
                .WriteTo.Kafka(options, formatter)
                .WriteTo.Console(formatter)
                .CreateLogger();


            return services.AddLogging(
                builder => builder.AddSerilog());
        }
    }
}
