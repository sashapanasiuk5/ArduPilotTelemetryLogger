using System.Collections.Generic;
using System.Xml.Linq;

namespace TelemetryLogger.GPX
{
    public class GpxGenerator
    {
        public static void Generate(List<GpxPoint> points, string filePath)
        {
            XNamespace gpxNs = "http://www.topografix.com/GPX/1/1";

            var gpxDoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(gpxNs + "gpx",
                    new XAttribute("version", "1.1"),
                    new XAttribute("creator", ".NET Generator"),
                    new XElement(gpxNs + "trk",
                        new XElement(gpxNs + "name", "Google Earth Track"),
                        new XElement(gpxNs + "trkseg",
                            points.ConvertAll(p => new XElement(gpxNs + "trkpt",
                                new XAttribute("lat", p.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                                new XAttribute("lon", p.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                                new XElement(gpxNs + "ele", p.Elevation.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                                new XElement(gpxNs + "time", p.Time.ToString("yyyy-MM-ddTHH:mm:ssZ"))
                            ))
                        )
                    )
                )
            );

            gpxDoc.Save(filePath);
        }
    }
}
