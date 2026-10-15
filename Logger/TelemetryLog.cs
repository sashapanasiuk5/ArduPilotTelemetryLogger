using System.Globalization;

namespace TelemetryLogger.Logger
{
    internal class TelemetryLog: BaseLog
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double AltitudeAslM { get; set; }
        public double SpeedMps { get; set; }
        public double RollDeg { get; set; }
        public double PitchDeg { get; set; }
        public double YawDeg { get; set; }
        public double VoltageV { get; set; }
        public double CurrentA { get; set; }
        public bool IsStale { get; set; }
        public bool ClockSyncWarning { get; set; }

        public override string ToCsvRow()
        {
            CultureInfo inv = CultureInfo.InvariantCulture;

            string timeStr = Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff", inv);
            string gpsTimeStr = GpsTime.ToString("yyyy-MM-dd HH:mm:ss.fff", inv);

            return $"{timeStr}," +
                   $"{gpsTimeStr}," +
                   $"{Latitude.ToString(inv)}," +
                   $"{Longitude.ToString(inv)}," +
                   $"{AltitudeAslM.ToString(inv)}," +
                   $"{SpeedMps.ToString(inv)}," +
                   $"{RollDeg.ToString(inv)}," +
                   $"{PitchDeg.ToString(inv)}," +
                   $"{YawDeg.ToString(inv)}," +
                   $"{VoltageV.ToString(inv)}," +
                   $"{CurrentA.ToString(inv)}," +
                   $"{IsStale}," +
                   $"{ClockSyncWarning}";
        }
    }
}
