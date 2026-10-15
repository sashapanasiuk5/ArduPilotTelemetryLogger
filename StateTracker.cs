using System;
using static MAVLink;

namespace TelemetryLogger
{
    internal class StateChangedEventArgs: EventArgs
    {
        public string EventName { get; set; } 
    }

    internal class StateTracker
    {
        private bool _armed;
        private string _mode;
        private byte _gpsType;
        private byte _gps2Type;

        public event EventHandler<StateChangedEventArgs> OnStateChanged;

        public void Update(bool armed, string mode, byte gpsType, byte gps2Type)
        {
            if (_armed != armed)
            {
                var eventName = armed ? "ARMED" : "DISARMED";
                OnStateChanged?.Invoke(this, new StateChangedEventArgs() { EventName = eventName });

                _armed = armed;
            }

            if (_mode != mode)
            {
                var eventName = "MODE SET TO " + mode.ToUpper();
                OnStateChanged?.Invoke(this, new StateChangedEventArgs() { EventName = eventName });
                _mode = mode;
            }

            if(_gpsType != gpsType)
            {
                var gpsDescription = ((GPS_FIX_TYPE)gpsType).GetDescription();
                var eventName = "GPS1 MODE: " + gpsDescription;
                OnStateChanged?.Invoke(this, new StateChangedEventArgs() { EventName = eventName });
                _gpsType = gpsType;
            }

            if (_gps2Type != gps2Type)
            {
                var gpsDescription = ((GPS_FIX_TYPE)gps2Type).GetDescription();
                var eventName = "GPS2 MODE: " + gpsDescription;
                OnStateChanged?.Invoke(this, new StateChangedEventArgs() { EventName = eventName });
                _gps2Type = gps2Type;
            }
        }
    }
}
