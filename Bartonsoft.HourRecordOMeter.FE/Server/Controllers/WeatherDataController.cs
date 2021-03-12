using Bartonsoft.HourRecordOMeter.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Bartonsoft.HourRecordOMeter.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherDataController : ControllerBase
    {

        private readonly ILogger<WeatherDataController> logger;

        public WeatherDataController(ILogger<WeatherDataController> logger)
        {
            this.logger = logger;
        }

        private double getAirDensity(double airpressure, double dewpoint, double temperature)
        {
            var ps = 611 * Math.Pow(10,(7.5 * (temperature - 273.15) / ((temperature - 273.15) + 237.3)));
            var ps0 = 611.0 * Math.Pow(10,(7.5 * (dewpoint - 273.15) / ((dewpoint - 273.15) + 237.3)));
            var st = 217 * ps / temperature;
            var st0 = 217 * ps0 / dewpoint;
            var rh = 100 * st0 / st;
            return Math.Round((airpressure * Math.Pow(10, 2) - 0.003796 * rh * ps) * 0.0034848 / (temperature), 4);
        }

        private JObject getAPIData() {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/onecall");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string latitude = "-41.305603179582434";
            string longitude = "174.78959253383022";
            string apiKey = "e293d3e8d82858659793cca3c3254743";

            string querystring = String.Format("?lat={0}&lon={1}&appid={2}", latitude, longitude, apiKey);

            HttpResponseMessage response = client.GetAsync(querystring).Result;
            return JObject.Parse(response.Content.ReadAsStringAsync().Result);
        }

        private IEnumerable<HourRecordWeatherData> getCurrentData(JObject openWeatherData) {
            // parse out current reading
            long dt = openWeatherData.SelectToken("current.dt").ToObject<long>();
            var date = (new DateTime(1970, 1, 1)).AddMilliseconds(dt * 1000);

            double temp = openWeatherData.SelectToken("current.temp").ToObject<long>();
            double pressure = openWeatherData.SelectToken("current.pressure").ToObject<long>();
            double dewpoint = openWeatherData.SelectToken("current.dew_point").ToObject<long>();

            // return it
            return Enumerable.Range(1, 1).Select(ret => new HourRecordWeatherData
            {
                DateTime = date,
                AirPressure = pressure,
                Temperature = temp,
                DewPointTemperature = dewpoint
            }).ToArray();
        }

        private IEnumerable<HourRecordWeatherData> getDailyData(JObject openWeatherData){
            // parse out daily readings
            JArray dailyReadings = openWeatherData.SelectToken("daily").ToObject<JArray>();
                
            return dailyReadings.Select(ret => new HourRecordWeatherData
            {
                DateTime = (new DateTime(1970, 1, 1)).AddSeconds(((int)ret["dt"])),
                AirPressure = (double)ret["pressure"],
                Temperature = (double)ret["temp"]["eve"],
                DewPointTemperature = (double)ret["dew_point"],
                AirDensity = getAirDensity((double)ret["pressure"], (double)ret["dew_point"], (double)ret["temp"]["eve"])
            }).ToArray();
        }

        [HttpGet]
        public IEnumerable<HourRecordWeatherData> Get(string type = "daily")
        {
            switch (type) {
                case "daily":
                    return getDailyData(getAPIData());
                case "current":
                    return getCurrentData(getAPIData());
                default:
                    return Enumerable.Range(1,1).Select(item => new HourRecordWeatherData());
            }
        }
    }
}
