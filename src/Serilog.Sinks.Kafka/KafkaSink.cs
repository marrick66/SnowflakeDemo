using Confluent.Kafka;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.PeriodicBatching;
using System.Text;

namespace Serilog.Sinks.Kafka
{

    public class KafkaSink : IBatchedLogEventSink
    {
        private readonly string _topicName;
        private readonly ProducerBuilder<Null, string> _builder;
        private readonly ITextFormatter? _textFormatter;


        public KafkaSink(
            KafkaSinkOptions options,
            ITextFormatter? formatter = null)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = options.Location,
            };

            _builder = new ProducerBuilder<Null, string>(config);
            _topicName = options.TopicName
                ?? throw new ArgumentNullException(nameof(options.TopicName));

            _textFormatter = formatter;

        }

        public async Task EmitBatchAsync(IEnumerable<LogEvent> batch)
        {
            using (var producer = _builder.Build())
            {
                foreach (var @event in batch)
                {
                    var evtStream = new MemoryStream();
                    using (var writer = new StreamWriter(evtStream))
                    using (var reader = new StreamReader(evtStream))
                    {
                        if (_textFormatter != null)
                            _textFormatter.Format(@event, writer);
                        else
                            writer.WriteLine(@event.RenderMessage());

                        writer.Flush();

                        var json = Encoding.UTF8.GetString(evtStream.GetBuffer());

                        await producer.ProduceAsync(
                            _topicName,
                            new Message<Null, string>
                            {
                                Value = json
                            });
                    }

                }
            }
        }

        public Task OnEmptyBatchAsync()
        {
            return Task.CompletedTask;
        }
    }
}