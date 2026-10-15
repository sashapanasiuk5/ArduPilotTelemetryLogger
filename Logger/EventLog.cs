using System.Globalization;

namespace TelemetryLogger.Logger
{
    internal class EventLog : BaseLog
    {
        public string EventName { get; set; }
        public override string ToCsvRow()
        {
            CultureInfo inv = CultureInfo.InvariantCulture;

            string timeStr = Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff", inv);
            string gpsTimeStr = GpsTime.ToString("yyyy-MM-dd HH:mm:ss.fff", inv);

            return $"{timeStr}," +
                   $"{gpsTimeStr}," +
                   EventName;
        }
    }
}
