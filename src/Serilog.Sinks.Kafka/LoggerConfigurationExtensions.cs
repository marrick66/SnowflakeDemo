using Serilog.Configuration;
using Serilog.Formatting;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.Kafka
{
    public static class LoggerConfigurationExtensions
    {
        public static LoggerConfiguration Kafka(
              this LoggerSinkConfiguration loggerConfiguration,
              KafkaSinkOptions options,
              ITextFormatter? formatter = null)
        {
            //Some small default options:
            var batchingOptions = new PeriodicBatchingSinkOptions
            {
                BatchSizeLimit = 100,
                Period = TimeSpan.FromSeconds(2),
                EagerlyEmitFirstEvent = true,
                QueueLimit = 500
            };

            var batchingSink = new PeriodicBatchingSink(
                new KafkaSink(options, formatter), 
                batchingOptions);

            return loggerConfiguration.Sink(batchingSink);
        }
    }
}
