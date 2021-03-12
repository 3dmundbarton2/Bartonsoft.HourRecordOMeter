using System;

namespace Bartonsoft.HourRecordOMeter.Shared
{
    public class HourRecordWeatherData
    {
        public DateTime DateTime { get; set; }
        public double Temperature { get; set; }
        public double DewPointTemperature { get; set; }
        public double AirPressure { get; set; }
        public double AirDensity { get; set; }
    }
}
