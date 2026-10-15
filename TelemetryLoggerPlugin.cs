using MissionPlanner.GCSViews;
using MissionPlanner.Plugin;
using MissionPlanner.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TelemetryLogger.GPX;
using TelemetryLogger.Logger;

namespace TelemetryLogger
{
    public class TelemetryLoggerPlugin : Plugin
    {
        public override string Name => "TelemetryLoggerPlugin";

        public override string Version => "1.0.0";

        public override string Author => "Oleksandr Panasiuk";

        private bool _isConnected;

        private TelemetryFileLogger _telemetryLogger = new TelemetryFileLogger("telemetry-", new string[] { "timestamp", "gps_time", "latitude", "longitude", "altitude_asl_m", "speed_mps", "roll_deg", "pitсh_deg", "yaw_deg", "voltage_V", "current_A", "is_stale", "clock_sync_warning" });

        private TelemetryFileLogger _eventsLogger = new TelemetryFileLogger("events-", new string[] { "timestamp", "gps_time", "event" });

        private StateTracker _stateTracker =  new StateTracker();

        private List<GpxPoint> _points = new List<GpxPoint>();

        public override bool Exit()
        {
            _eventsLogger.Dispose();
            _telemetryLogger.Dispose();

            Host.comPort.MAVDetected -= OnConnected;
            Host.comPort.CommsClose -= OnDisConnected;

            return true;
        }

        public override bool Init()
        {
            var frequencySetting = Settings.Instance["TelemetryLogFrequency"];

            if (string.IsNullOrEmpty(frequencySetting))
            {
                this.loopratehz = 1;
            }
            else
            {
                var frequency = decimal.Parse(frequencySetting);
                this.loopratehz = (float)frequency;
            }

            return true;
        }

        public override bool Loaded()
        {
            SoftwareConfig.AddPluginViewPage(typeof(TelemetryLoggerConfigView), "Telemetry Logger", SoftwareConfig.pageOptions.isDisConnected);

            Host.comPort.MAVDetected += OnConnected;
            Host.comPort.CommsClose += OnDisConnected;

            return true;
        }

        public override bool Loop()
        {
            if (_isConnected)
            {
                var isStale = (DateTime.UtcNow - Host.comPort.MAV.lastvalidpacket).Duration() > TimeSpan.FromSeconds(3);
                var clockSynkWarning = (DateTime.UtcNow - Host.cs.gpstime).Duration() > TimeSpan.FromSeconds(1);

                var log = new TelemetryLog()
                {
                    Timestamp = DateTime.UtcNow,
                    GpsTime = Host.cs.gpstime,
                    Latitude = Host.cs.lat,
                    Longitude = Host.cs.lng,
                    AltitudeAslM = Host.cs.altasl,
                    SpeedMps = Host.cs.airspeed,
                    PitchDeg = Host.cs.pitch,
                    RollDeg = Host.cs.roll,
                    YawDeg = Host.cs.yaw,
                    VoltageV = Host.cs.battery_voltage,
                    CurrentA = Host.cs.current,
                    IsStale = isStale,
                    ClockSyncWarning = clockSynkWarning
                };
                _telemetryLogger.Write(log);

                _stateTracker.Update(Host.cs.armed, Host.cs.mode, (byte)Host.cs.gpsstatus, (byte)Host.cs.gpsstatus2);

                _telemetryLogger.DelayedFlush();
                _eventsLogger.DelayedFlush();

                _points.Add(new GpxPoint(Host.cs.lat, Host.cs.lng, Host.cs.altasl, DateTime.UtcNow));
            }

            return base.Loop();
        }
        public static string GetLogDir()
        {
            var logPath = Settings.Instance["TelemetryLogDir"];

            if (string.IsNullOrEmpty(logPath))
            {
                logPath = GetDefaultLogDir();
            }
            return logPath;
        }

        private void OnConnected(object sender, (byte,byte) args)
        {
            var logDir = GetLogDir();
            _eventsLogger.StartLogging(logDir);
            _telemetryLogger.StartLogging(logDir);

            _isConnected = true;

            _points.Clear();
            _stateTracker.OnStateChanged += OnStateChanged;
        }

        private void OnDisConnected(object sender, EventArgs args)
        {
            _eventsLogger.Dispose();
            _telemetryLogger.Dispose();

            _isConnected = false;

            var gpxFileName = DateTime.UtcNow.ToString("yyyy-MM-ddTHHmm", CultureInfo.InvariantCulture) + ".gpx";
            GpxGenerator.Generate(_points, GetLogDir() + "\\" + gpxFileName);

            _stateTracker.OnStateChanged -= OnStateChanged;
        }

        private void OnStateChanged(object sender, StateChangedEventArgs args)
        {
            var log = new EventLog()
            {
                Timestamp = DateTime.UtcNow,
                GpsTime = Host.cs.gpstime,
                EventName = args.EventName
            };

            _eventsLogger.Write(log);
        }

        private static string GetDefaultLogDir()
        {
            string directory = Settings.GetUserDataDirectory() + @"logs\TelemetryLogger";
            if (!Directory.Exists(directory))
            {
                try
                {
                    Directory.CreateDirectory(directory);
                }
                catch
                {

                }
            }

            return directory;
        }
    }
}