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
            public string Condition { get; set; }
            public string GifName { get; set; }
            public string PngName { get; set; }
            public string BackgroundImg { get; set; }
        }

        public List<WeatherCondition> weatherConditions = new List<WeatherCondition>
        {
            new WeatherCondition { Code = 1000, Condition = "Sunny", GifName = "sunny.gif", PngName = "sunny.png", BackgroundImg = "sunny-bg.png" },
            new WeatherCondition { Code = 1003, Condition = "Partly cloudy", GifName = "partlycloudy.gif", PngName = "partlycloudy.png", BackgroundImg = "partlycloudy-bg.png" },
            new WeatherCondition { Code = 1006, Condition = "Cloudy", GifName = "cloudy.gif", PngName = "cloudy.png", BackgroundImg = "cloudy-bg.png" },
            new WeatherCondition { Code = 1009, Condition = "Overcast", GifName = "cloudy.gif", PngName = "cloudy.png", BackgroundImg = "cloudy-bg.png" },
            new WeatherCondition { Code = 1030, Condition = "Mist", GifName = "fog.gif", PngName = "fog.png", BackgroundImg = "fog-bg.png" },
            new WeatherCondition { Code = 1063, Condition = "Patchy rain possible", GifName = "lightrain.gif", PngName = "lightrain.png", BackgroundImg = "lightrain-bg.png" },
            new WeatherCondition { Code = 1066, Condition = "Patchy snow possible", GifName = "lightsnow.gif", PngName = "lightsnow.png", BackgroundImg = "lightsnow-bg.png" },
            new WeatherCondition { Code = 1069, Condition = "Patchy sleet possible", GifName = "lightsnow.gif", PngName = "lightsnow.png", BackgroundImg = "lightsnow-bg.png" },
            new WeatherCondition { Code = 1072, Condition = "Patchy freezing drizzle possible", GifName = "lightsnow.gif", PngName = "lightsnow.png", BackgroundImg = "lightsnow-bg.png" },
            new WeatherCondition { Code = 1087, Condition = "Thundery outbreaks possible", GifName = "lightning.gif", PngName = "lightning.png", BackgroundImg = "lightning-bg.png" },
            new WeatherCondition { Code = 1114, Condition = "Blowing snow", GifName = "snowstorm.gif", PngName = "snowstorm.png", BackgroundImg = "snowstorm-bg.png" },
            new WeatherCondition { Code = 1117, Condition = "Blizzard", GifName = "snowstorm.gif", PngName = "snowstorm.png", BackgroundImg = "snowstorm-bg.png" },
            new WeatherCondition { Code = 1135, Condition = "Fog", GifName = "fog.gif", PngName = "fog.png", BackgroundImg = "fog-bg.png" },
            new WeatherCondition { Code = 1147, Condition = "Freezing fog", GifName = "fog.gif", PngName = "fog.png", BackgroundImg = "fog-bg.png" },
            new WeatherCondition { Code = 1150, Condition = "Patchy light drizzle", GifName = "lightrain.gif", PngName = "lightrain.png", BackgroundImg = "lightrain-bg.png" },
            new WeatherCondition { Code = 1153, Condition = "Light drizzle", GifName = "lightrain.gif", PngName = "lightrain.png", BackgroundImg = "lightrain-bg.png" },
            new WeatherCondition { Code = 1168, Condition = "Freezing drizzle", GifName = "lightsnow.gif", PngName = "lightsnow.png", BackgroundImg = "lightsnow-bg.png" },
            new WeatherCondition { Code = 1171, Condition = "Heavy freezing drizzle", GifName = "snowstorm.gif", PngName = "snowstorm.png", BackgroundImg = "snowstorm-bg.png" },
            new WeatherCondition { Code = 1180, Condition = "Patchy light rain", GifName = "lightrain.gif", PngName = "lightrain.png", BackgroundImg = "lightrain-bg.png" },
            new WeatherCondition { Code = 1183, Condition = "Light rain", GifName = "lightrain.gif", PngName = "lightrain.png", BackgroundImg = "lightrain-bg.png" },
            new WeatherCondition { Code = 1186, Condition = "Moderate rain at times", GifName = "rain.gif", PngName = "rain.png", BackgroundImg = "rain-bg.png" },
            new WeatherCondition { Code = 1189, Condition = "Moderate rain", GifName = "rain.gif", PngName = "rain.png", BackgroundImg = "rain-bg.png" },
            new WeatherCondition { Code = 1192, Condition = "Heavy rain at times", GifName = "torrentialrain.gif", PngName = "torrentialrain.png", BackgroundImg = "torrentialrain-bg.png" },
            new WeatherCondition { Code = 1195, Condition = "Heavy rain", GifName = "torrentialrain.gif", PngName = "torrentialrain.png", BackgroundImg = "torrentialrain-bg.png" },
            new WeatherCondition { Code = 1198, Condition = "Light freezing rain", GifName = "lightsnow.gif", PngName = "lightsnow.png", BackgroundImg = "lightsnow-bg.png" },
            new WeatherCondition { Code = 1201, Condition = "Moderate or heavy freezing rain", GifName = "snowstorm.gif", PngName = "snowstorm.png", BackgroundImg = "snowstorm-bg.png" },
            new WeatherCondition { Code = 1204, Condition = "Light sleet", GifName = "lightsnow.gif", PngName = "lightsnow.png", BackgroundImg = "lightsnow-bg.png" },
            new WeatherCondition { Code = 1207, Condition = "Moderate or heavy sleet", GifName = "snowstorm.gif", PngName = "snowstorm.png", BackgroundImg = "snowstorm-bg.png" },
            new WeatherCondition { Code = 1210, Condition = "Patchy light snow", GifName = "lightsnow.gif", PngName = "lightsnow.png", BackgroundImg = "lightsnow-bg.png" },
            new WeatherCondition { Code = 1213, Condition = "Light snow", GifName = "lightsnow.gif", PngName = "lightsnow.png", BackgroundImg = "lightsnow-bg.png" },
            new WeatherCondition { Code = 1216, Condition = "Patchy moderate snow", GifName = "lightsnow.gif", PngName = "lightsnow.png", BackgroundImg = "lightsnow-bg.png" },
            new WeatherCondition { Code = 1219, Condition = "Moderate snow", GifName = "lightsnow.gif", PngName = "lightsnow.png", BackgroundImg = "lightsnow-bg.png" },
            new WeatherCondition { Code = 1222, Condition = "Patchy heavy snow", GifName = "snowstorm.gif", PngName = "snowstorm.png", BackgroundImg = "snowstorm-bg.png" },
            new WeatherCondition { Code = 1225, Condition = "Heavy snow", GifName = "snowstorm.gif", PngName = "snowstorm.png", BackgroundImg = "snowstorm-bg.png" },
            new WeatherCondition { Code = 1237, Condition = "Ice pellets", GifName = "snowstorm.gif", PngName = "snowstorm.png", BackgroundImg = "snowstorm-bg.png" },
            new WeatherCondition { Code = 1240, Condition = "Light rain shower", GifName = "lightrain.gif", PngName = "lightrain.png", BackgroundImg = "lightrain-bg.png" },
            new WeatherCondition { Code = 1243, Condition = "Moderate or heavy rain shower", GifName = "rain.gif", PngName = "rain.png", BackgroundImg = "rain-bg.png" },
            new WeatherCondition { Code = 1246, Condition = "Torrential rain shower", GifName = "torrentialrain.gif", PngName = "torrentialrain.png", BackgroundImg = "torrentialrain-bg.png" },
            new WeatherCondition { Code = 1249, Condition = "Light sleet showers", GifName = "lightsnow.gif", PngName = "lightsnow.png", BackgroundImg = "lightsnow-bg.png" },
            new WeatherCondition { Code = 1252, Condition = "Moderate or heavy sleet showers", GifName = "snowstorm.gif", PngName = "snowstorm.png", BackgroundImg = "snowstorm-bg.png" },
            new WeatherCondition { Code = 1255, Condition = "Light snow showers", GifName = "lightsnow.gif", PngName = "lightsnow.png", BackgroundImg = "lightsnow-bg.png" },
            new WeatherCondition { Code = 1258, Condition = "Moderate or heavy snow showers", GifName = "snowstorm.gif", PngName = "snowstorm.png", BackgroundImg = "snowstorm-bg.png" },
            new WeatherCondition { Code = 1261, Condition = "Light showers of ice pellets", GifName = "lightsnow.gif", PngName = "lightsnow.png", BackgroundImg = "lightsnow-bg.png" },
            new WeatherCondition { Code = 1264, Condition = "Moderate or heavy showers of ice pellets", GifName = "snowstorm.gif", PngName = "snowstorm.png", BackgroundImg = "snowstorm-bg.png" },
            new WeatherCondition { Code = 1273, Condition = "Patchy light rain with thunder", GifName = "storm.gif", PngName = "storm.png", BackgroundImg = "storm-bg.png" },
            new WeatherCondition { Code = 1276, Condition = "Moderate or heavy rain with thunder", GifName = "storm.gif", PngName = "storm.png", BackgroundImg = "storm-bg.png" },
            new WeatherCondition { Code = 1279, Condition = "Patchy light snow with thunder", GifName = "lightsnow.gif", PngName = "lightsnow.png", BackgroundImg = "lightsnow-bg.png" },
            new WeatherCondition { Code = 1282, Condition = "Moderate or heavy snow with thunder", GifName = "snowstorm.gif", PngName = "snowstorm.png", BackgroundImg = "snowstorm-bg.png" }
        };


        private async void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                currentLocation = await GetCurrentLocation();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not fetch current location: " + ex.Message);
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
                string hourlyData = await GetHourlyData(location);
                string forecastData = await GetForecastData(location);
                DisplayWeatherData(weatherData);
                DisplayHourlyForecastData(hourlyData);
                DisplayForecastData(forecastData);
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show("Could not fetch weather data: " + httpEx.Message);
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

        private async Task<string> GetHourlyData(string location)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"{weatherUrl}forecast.json?key={weatherApiKey}&q={location}&hours=6";
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
            int humidity = int.Parse(weatherData["current"]["humidity"].ToString());
            int uvIndex = int.Parse(weatherData["current"]["uv"].ToString());

            int conditionCode = int.Parse(weatherData["current"]["condition"]["code"].ToString());
            WeatherCondition condition = weatherConditions.FirstOrDefault(c => c.Code == conditionCode);
            string conditionDescription = condition != null ? condition.Condition : "Unknown condition";
            string GifName = condition != null ? condition.GifName : "cloudy.gif";
            string BackgroundImg = condition != null ? condition.BackgroundImg : "cloudy-bg.gif";
            string illustPath = Path.Combine(Application.StartupPath, "Assets", BackgroundImg);

            string location = weatherData["location"]["name"].ToString();
            string country = weatherData["location"]["country"].ToString();

            string localtime = weatherData["location"]["localtime"].ToString();
            DateTime dateTime = DateTime.Parse(localtime);
            string day = dateTime.DayOfWeek.ToString();
            string date = dateTime.ToString("MMM d");
            string formattedDate = dateTime.ToString("M/d");
            string time = dateTime.ToString("h:mm tt").ToUpper();

            bool isNight = dateTime.Hour < 6 || dateTime.Hour > 18;

            if (isNight && condition != null && condition.Code == 1000)
            {
                conditionDescription = "Clear";
                GifName = "night.gif";
            } 
            else if (isNight && condition != null && condition.Code == 1003)
            {
                GifName = "partlycloudynight.gif";
            }

            string iconPath = Path.Combine(Application.StartupPath, "Assets", GifName);

            tempTodayLbl.Text = $"{roundedTemperature}°";
            locationLbl.Text = $"{location}";
            countryLbl.Text = $"{country}";
            dayTodayLbl.Text = $"{day}, {date}";
            timeUpdatedLbl.Text = $"Updated {formattedDate} {time}";
            conditionLbl.Text = $"Weather:   {conditionDescription}";
            humidityLbl.Text = $"Humidity: {humidity}%";
            uvLbl.Text = $"UV Index:  {uvIndex}";
            todayIcon.Image = Image.FromFile(iconPath);
            todayIllust.Image = Image.FromFile(illustPath);
        }

        private void DisplayHourlyForecastData(string jsonData)
        {
            JObject forecastData = JObject.Parse(jsonData);
            JArray hourlyData = (JArray)forecastData["forecast"]["forecastday"][0]["hour"];

            string localtime = forecastData["location"]["localtime"].ToString();
            DateTime currentDateTime = DateTime.Parse(localtime);
            int currentHour = currentDateTime.Hour;

            for (int i = 1; i <= 5; i++)
            {
                int hourIndex = (currentHour + i) % 24;

                var hourData = hourlyData[hourIndex];
                DateTime dateTime = DateTime.Parse(hourData["time"].ToString());
                string time = dateTime.ToString("h tt").ToUpper();

                double temperature = double.Parse(hourData["temp_c"].ToString());
                string roundedTemperature = Math.Round(temperature).ToString();

                int conditionCode = int.Parse(hourData["condition"]["code"].ToString());
                WeatherCondition condition = weatherConditions.FirstOrDefault(c => c.Code == conditionCode);
                string conditionDescription = condition != null ? condition.Condition : "Unknown condition";
                string GifName = condition != null ? condition.GifName : "cloudy.gif";

                bool isNight = dateTime.Hour < 6 || dateTime.Hour > 18;

                if (isNight && condition != null && condition.Code == 1000)
                {
                    GifName = "night.gif";
                }
                else if (isNight && condition != null && condition.Code == 1003)
                {
                    GifName = "partlycloudynight.gif";
                }

                string iconPath = Path.Combine(Application.StartupPath, "Assets", GifName);

                Image weatherIcon = null;
                if (File.Exists(iconPath))
                {
                    weatherIcon = Image.FromFile(iconPath);
                }

                switch (i)
                {
                    case 1:
                        if (weatherIcon != null) hr1Icon.Image = weatherIcon;
                        hr1Lbl.Text = $"{time}";
                        hr1TempLbl.Text = $"{roundedTemperature}°";
                        break;

                    case 2:
                        if (weatherIcon != null) hr2Icon.Image = weatherIcon;
                        hr2Lbl.Text = $"{time}";
                        hr2TempLbl.Text = $"{roundedTemperature}°";
                        break;

                    case 3:
                        if (weatherIcon != null) hr3Icon.Image = weatherIcon;
                        hr3Lbl.Text = $"{time}";
                        hr3TempLbl.Text = $"{roundedTemperature}°";
                        break;

                    case 4:
                        if (weatherIcon != null) hr4Icon.Image = weatherIcon;
                        hr4Lbl.Text = $"{time}";
                        hr4TempLbl.Text = $"{roundedTemperature}°";
                        break;

                    case 5:
                        if (weatherIcon != null) hr5Icon.Image = weatherIcon;
                        hr5Lbl.Text = $"{time}";
                        hr5TempLbl.Text = $"{roundedTemperature}°";
                        break;
                }
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
                string conditionDescription = condition != null ? condition.Condition : "Unknown condition";
                string PngName = condition != null ? condition.PngName : "cloudy.png";
                string iconPath = Path.Combine(Application.StartupPath, "Assets", PngName);

                switch (i)
                {
                    case 1:                        
                        day1Icon.Image = Image.FromFile(iconPath);
                        day1TempsLbl.Text = $"{roundedMin}°/{roundedMax}°";
                        day1Lbl.Text = $"{dayOfWeek}";
                        day1DateLbl.Text = $"{date}";
                        break;

                    case 2:
                        day2Icon.Image = Image.FromFile(iconPath);
                        day2TempsLbl.Text = $"{roundedMin}°/{roundedMax}°";
                        day2Lbl.Text = $"{dayOfWeek}";
                        day2DateLbl.Text = $"{date}";
                        break;

                    case 3:
                        day3Icon.Image = Image.FromFile(iconPath);
                        day3TempsLbl.Text = $"{roundedMin}°/{roundedMax}°";
                        day3Lbl.Text = $"{dayOfWeek}";
                        day3DateLbl.Text = $"{date}";
                        break;  

                    case 4:
                        day4Icon.Image = Image.FromFile(iconPath);
                        day4TempsLbl.Text = $"{roundedMin}°/{roundedMax}°";
                        day4Lbl.Text = $"{dayOfWeek}";
                        day4DateLbl.Text = $"{date}";
                        break;  

                    case 5:
                        day5Icon.Image = Image.FromFile(iconPath);
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

        private void conditionLbl_Click(object sender, EventArgs e)
        {

        }
    };
}
