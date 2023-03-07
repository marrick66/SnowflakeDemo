using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Sinks.Kafka
{
    public class KafkaSinkOptions
    {
        public string? Location { get; set; }
        public string? TopicName { get; set; }

    }
}
