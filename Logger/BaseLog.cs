using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelemetryLogger.Logger
{
    public abstract class BaseLog
    {
        public DateTime Timestamp { get; set; }
        public DateTime GpsTime { get; set; }
        public abstract string ToCsvRow();
    }
}
