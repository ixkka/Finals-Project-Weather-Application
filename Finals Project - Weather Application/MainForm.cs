using System;
using ComponentFactory.Krypton.Toolkit;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;


namespace Finals_Project___Weather_Application
{
    public partial class MainForm : KryptonForm
    {
        private readonly string ipInfoUrl = "http://ipinfo.io/json";
        private readonly string weatherApiKey = "24adb1d3d147425da41145554240607";
        private readonly string weatherUrl = "http://api.weatherapi.com/v1/";
        private string currentLocation;

        public MainForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(MainForm_Load);
        }

        public class WeatherCondition
        {
            public int Code { get; set; }
            public string Day { get; set; }
            public string Night { get; set; }
            public string GifName { get; set; }
            public string PngName { get; set; }
        }

        public List<WeatherCondition> weatherConditions = new List<WeatherCondition>
        {
            new WeatherCondition { Code = 1000, Day = "Sunny", GifName = "sunny.gif", PngName = "sunny.png" },
            new WeatherCondition { Code = 1003, Day = "Partly cloudy", GifName = "partlycloudy.gif", PngName = "partlycloudy.png" },
            new WeatherCondition { Code = 1006, Day = "Cloudy", GifName = "cloudy.gif", PngName = "cloudy.png" },
            new WeatherCondition { Code = 1009, Day = "Overcast", GifName = "cloudy.gif", PngName = "cloudy.png" },
            new WeatherCondition { Code = 1030, Day = "Mist", GifName = "fog.gif", PngName = "fog.png" },
            new WeatherCondition { Code = 1063, Day = "Patchy rain possible", GifName = "lightrain.gif", PngName = "lightrain.png" },
            new WeatherCondition { Code = 1066, Day = "Patchy snow possible", GifName = "lightsnow.gif", PngName = "lightsnow.png" },
            new WeatherCondition { Code = 1069, Day = "Patchy sleet possible", GifName = "lightsnow.gif", PngName = "lightsnow.png" },
            new WeatherCondition { Code = 1072, Day = "Patchy freezing drizzle possible", GifName = "lightsnow.gif", PngName = "lightsnow.png" },
            new WeatherCondition { Code = 1087, Day = "Thundery outbreaks possible", GifName = "lightning.gif", PngName = "lightning.png" },
            new WeatherCondition { Code = 1114, Day = "Blowing snow", GifName = "snowstorm.gif", PngName = "snowstorm.png" },
            new WeatherCondition { Code = 1117, Day = "Blizzard", GifName = "snowstorm.gif", PngName = "snowstorm.png" },
            new WeatherCondition { Code = 1135, Day = "Fog", GifName = "fog.gif", PngName = "fog.png" },
            new WeatherCondition { Code = 1147, Day = "Freezing fog", GifName = "fog.gif", PngName = "fog.png" },
            new WeatherCondition { Code = 1150, Day = "Patchy light drizzle", GifName = "lightrain.gif", PngName = "lightrain.png" },
            new WeatherCondition { Code = 1153, Day = "Light drizzle", GifName = "lightrain.gif", PngName = "lightrain.png" },
            new WeatherCondition { Code = 1168, Day = "Freezing drizzle", GifName = "lightsnow.gif", PngName = "lightsnow.png" },
            new WeatherCondition { Code = 1171, Day = "Heavy freezing drizzle", GifName = "snowstorm.gif", PngName = "snowstorm.png" },
            new WeatherCondition { Code = 1180, Day = "Patchy light rain", GifName = "lightrain.gif", PngName = "lightrain.png" },
            new WeatherCondition { Code = 1183, Day = "Light rain", GifName = "lightrain.gif", PngName = "lightrain.png" },
            new WeatherCondition { Code = 1186, Day = "Moderate rain at times", GifName = "rain.gif", PngName = "rain.png" },
            new WeatherCondition { Code = 1189, Day = "Moderate rain", GifName = "rain.gif", PngName = "rain.png" },
            new WeatherCondition { Code = 1192, Day = "Heavy rain at times", GifName = "torrentialrain.gif", PngName = "torrentialrain.png" },
            new WeatherCondition { Code = 1195, Day = "Heavy rain", GifName = "torrentialrain.gif", PngName = "torrentialrain.png" },
            new WeatherCondition { Code = 1198, Day = "Light freezing rain", GifName = "lightsnow.gif", PngName = "lightsnow.png" },
            new WeatherCondition { Code = 1201, Day = "Moderate or heavy freezing rain", GifName = "snowstorm.gif", PngName = "snowstorm.png" },
            new WeatherCondition { Code = 1204, Day = "Light sleet", GifName = "lightsnow.gif", PngName = "lightsnow.png" },
            new WeatherCondition { Code = 1207, Day = "Moderate or heavy sleet", GifName = "snowstorm.gif", PngName = "snowstorm.png" },
            new WeatherCondition { Code = 1210, Day = "Patchy light snow", GifName = "lightsnow.gif", PngName = "lightsnow.png" },
            new WeatherCondition { Code = 1213, Day = "Light snow", GifName = "lightsnow.gif", PngName = "lightsnow.png" },
            new WeatherCondition { Code = 1216, Day = "Patchy moderate snow", GifName = "lightsnow.gif", PngName = "lightsnow.png" },
            new WeatherCondition { Code = 1219, Day = "Moderate snow", GifName = "lightsnow.gif", PngName = "lightsnow.png" },
            new WeatherCondition { Code = 1222, Day = "Patchy heavy snow", GifName = "snowstorm.gif", PngName = "snowstorm.png" },
            new WeatherCondition { Code = 1225, Day = "Heavy snow", GifName = "snowstorm.gif", PngName = "snowstorm.png" },
            new WeatherCondition { Code = 1237, Day = "Ice pellets", GifName = "snowstorm.gif", PngName = "snowstorm.png" },
            new WeatherCondition { Code = 1240, Day = "Light rain shower", GifName = "lightrain.gif", PngName = "lightrain.png" },
            new WeatherCondition { Code = 1243, Day = "Moderate or heavy rain shower", GifName = "rain.gif", PngName = "rain.png" },
            new WeatherCondition { Code = 1246, Day = "Torrential rain shower", GifName = "torrentialrain.gif", PngName = "torrentialrain.png" },
            new WeatherCondition { Code = 1249, Day = "Light sleet showers", GifName = "lightsnow.gif", PngName = "lightsnow.png" },
            new WeatherCondition { Code = 1252, Day = "Moderate or heavy sleet showers", GifName = "snowstorm.gif", PngName = "snowstorm.png" },
            new WeatherCondition { Code = 1255, Day = "Light snow showers", GifName = "lightsnow.gif", PngName = "lightsnow.png" },
            new WeatherCondition { Code = 1258, Day = "Moderate or heavy snow showers", GifName = "snowstorm.gif", PngName = "snowstorm.png" },
            new WeatherCondition { Code = 1261, Day = "Light showers of ice pellets", GifName = "lightsnow.gif", PngName = "lightsnow.png" },
            new WeatherCondition { Code = 1264, Day = "Moderate or heavy showers of ice pellets", GifName = "snowstorm.gif", PngName = "snowstorm.png" },
            new WeatherCondition { Code = 1273, Day = "Patchy light rain with thunder", GifName = "storm.gif", PngName = "storm.png" },
            new WeatherCondition { Code = 1276, Day = "Moderate or heavy rain with thunder", GifName = "storm.gif", PngName = "storm.png" },
            new WeatherCondition { Code = 1279, Day = "Patchy light snow with thunder", GifName = "lightsnow.gif", PngName = "lightsnow.png" },
            new WeatherCondition { Code = 1282, Day = "Moderate or heavy snow with thunder", GifName = "snowstorm.gif", PngName = "snowstorm.png" }
        };


        private async void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                currentLocation = await GetCurrentLocation();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching current location. Default location will be used: " + ex.Message);
            }
            await FetchWeatherForLocation(currentLocation);
        }

        private async Task<string> GetCurrentLocation()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(ipInfoUrl);
                response.EnsureSuccessStatusCode();
                string ipInfoData = await response.Content.ReadAsStringAsync();

                JObject ipInfo = JObject.Parse(ipInfoData);
                return ipInfo["city"].ToString();
            }
        }

        private async Task FetchWeatherForLocation(string location)
        {
            try
            {
                string weatherData = await GetWeatherData(location);
                DisplayWeatherData(weatherData);

                string forecastData = await GetForecastData(location);
                DisplayForecastData(forecastData);
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show("Error fetching weather data: " + httpEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private async Task<string> GetWeatherData(string location)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"{weatherUrl}current.json?key={weatherApiKey}&q={location}";
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }

        private async Task<string> GetForecastData(string location)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"{weatherUrl}forecast.json?key={weatherApiKey}&q={location}&days=6";
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }

        private void DisplayWeatherData(string jsonData)
        {
            JObject weatherData = JObject.Parse(jsonData);

            double temperature = double.Parse(weatherData["current"]["temp_c"].ToString());
            string roundedTemperature = Math.Round(temperature).ToString();

            int conditionCode = int.Parse(weatherData["current"]["condition"]["code"].ToString());
            WeatherCondition condition = weatherConditions.FirstOrDefault(c => c.Code == conditionCode);
            string conditionDescription = condition != null ? condition.Day : "Unknown condition";
            string GifName = condition != null ? condition.GifName : "cloudy.gif";

            string location = weatherData["location"]["name"].ToString();
            string country = weatherData["location"]["country"].ToString();

            string localtime = weatherData["location"]["localtime"].ToString();
            DateTime dateTime = DateTime.Parse(localtime);
            string day = dateTime.DayOfWeek.ToString();
            string date = dateTime.ToString("MMMM d");
            string formattedDate = dateTime.ToString("M/d");
            string time = dateTime.ToString("h:mm tt");


            tempTodayLbl.Text = $"{roundedTemperature}°";
            locationLbl.Text = $"{location}";
            countryLbl.Text = $"{country}";
            dayTodayLbl.Text = $"{day}";
            dateTodayLbl.Text = $"{date}";
            timeUpdatedLbl.Text = $"Updated {formattedDate} {time}";

            string iconPath = Path.Combine(Application.StartupPath, "Assets", GifName);
            if (File.Exists(iconPath))
            {
                todayIcon.Image = Image.FromFile(iconPath);
            }
        }

        private void DisplayForecastData(string jsonData)
        {
            JObject forecastData = JObject.Parse(jsonData);
            JArray forecastArray = (JArray)forecastData["forecast"]["forecastday"];

            for (int i = 1; i < forecastArray.Count; i++)
            {
                var day = forecastArray[i];
                DateTime dateTime = DateTime.Parse(day["date"].ToString());
                string dayOfWeek = dateTime.DayOfWeek.ToString();
                string date = dateTime.ToString("MMMM d");

                double minTemp = double.Parse(day["day"]["mintemp_c"].ToString());
                string roundedMin = Math.Round(minTemp).ToString();
                double maxTemp = double.Parse(day["day"]["maxtemp_c"].ToString());
                string roundedMax = Math.Round(maxTemp).ToString();

                int conditionCode = int.Parse(day["day"]["condition"]["code"].ToString());
                WeatherCondition condition = weatherConditions.FirstOrDefault(c => c.Code == conditionCode);
                string conditionDescription = condition != null ? condition.Day : "Unknown condition";
                string PngName = condition != null ? condition.PngName : "cloudy.png";
                string iconPath = Path.Combine(Application.StartupPath, "Assets", PngName);

                switch (i)
                {
                    case 1:                        
                        if (File.Exists(iconPath))
                        {
                            day1Icon.Image = Image.FromFile(iconPath);
                        }

                        day1TempsLbl.Text = $"{roundedMin}°/{roundedMax}°";
                        day1Lbl.Text = $"{dayOfWeek}";
                        day1DateLbl.Text = $"{date}";
                        break;

                    case 2:
                        if (File.Exists(iconPath))
                        {
                            day2Icon.Image = Image.FromFile(iconPath);
                        }

                        day2TempsLbl.Text = $"{roundedMin}°/{roundedMax}°";
                        day2Lbl.Text = $"{dayOfWeek}";
                        day2DateLbl.Text = $"{date}";
                        break;

                    case 3:
                        if (File.Exists(iconPath))
                        {
                            day3Icon.Image = Image.FromFile(iconPath);
                        }

                        day3TempsLbl.Text = $"{roundedMin}°/{roundedMax}°";
                        day3Lbl.Text = $"{dayOfWeek}";
                        day3DateLbl.Text = $"{date}";
                        break;  

                    case 4:
                        if (File.Exists(iconPath))
                        {
                            day4Icon.Image = Image.FromFile(iconPath);
                        }

                        day4TempsLbl.Text = $"{roundedMin}°/{roundedMax}°";
                        day4Lbl.Text = $"{dayOfWeek}";
                        day4DateLbl.Text = $"{date}";
                        break;  

                    case 5:
                        if (File.Exists(iconPath))
                        {
                            day5Icon.Image = Image.FromFile(iconPath);
                        }

                        day5TempsLbl.Text = $"{roundedMin}°/{roundedMax}°";
                        day5Lbl.Text = $"{dayOfWeek}";
                        day5DateLbl.Text = $"{date}";
                        break;
                }
            }
        }

        private void searchTxtBox_Enter(object sender, EventArgs e)
        {
            if(searchTxtBox.Text == "Search")
            {
                searchTxtBox.Clear();
            }
        }

        private void searchTxtBox_Leave(object sender, EventArgs e)
        {
            if (searchTxtBox != null && string.IsNullOrWhiteSpace(searchTxtBox.Text))
            {
                searchTxtBox.Text = "Search";
            }
        }

        private async void searchBtn_Click(object sender, EventArgs e)
        {
            string location = searchTxtBox.Text;
            if (!string.IsNullOrWhiteSpace(location) && location != "Search")
            {
                await FetchWeatherForLocation(location);
                currentLocation = location;
            }
        }

        private async void refreshBtn_Click(object sender, EventArgs e)
        {
            await FetchWeatherForLocation(currentLocation);
        }
    };
}
