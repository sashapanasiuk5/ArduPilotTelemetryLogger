using System;

namespace TelemetryLogger.GPX
{
    public class GpxPoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Elevation { get; set; }
        public DateTime Time { get; set; }

        public GpxPoint(double lat, double lng, double elevation, DateTime time)
        {
            Latitude = lat;
            Longitude = lng;
            Elevation = elevation;
            Time = time;
        }
       
    }
}
