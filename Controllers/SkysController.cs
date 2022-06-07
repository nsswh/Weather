using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Weather.Models;

namespace Weather.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkysController : ControllerBase
    {
        private DataContext context;
        public SkysController(DataContext ctx)
        {
            context = ctx;
        }

        [HttpGet("{city}/{days:int=15}")]
        public async Task<IQueryable<Sky>> GetSkys(string city, int days)
        {
            int diff = 0;
            // select the 1st item if city name is in the database
            var fst = context.Skys.Where(o => o.City.ToLower() == city.ToLower()).FirstOrDefault();

            if (fst == null)
            {
                // if city name is not in database, call web service to import 15 rows of the new city’s weather data into the database
                await AddNewCityWeather(city);
            }
            else
            {
                // check the city timestamp in database if it is 4 hours before
                diff = Convert.ToInt32(DateTime.UtcNow.Subtract(Convert.ToDateTime(fst.Timestamp)).TotalMinutes);
                if (diff >= 240)
                {
                    // delete all 15 rows of data from the database because it is 4 hours earlier
                    await DeleteCityWeather(city);
                    // call web service to import 15 rows of the new city’s weather data into the database
                    await AddNewCityWeather(city);
                }
            }

            // send the city weather data in the database to the frontend to display, select by city name maximum 15 items
            var sks = (IQueryable<Sky>)context.Skys.Where(x => x.City.ToLower() == city.ToLower()).
                Select(x => new Sky { City = x.City, Timestamp = x.Timestamp, Day = x.Day, Temp = x.Temp, Preciptype = x.Preciptype }).OrderBy(x => x.Day).Take(days);

            return sks;
        }

        [NonAction]
        public async Task DeleteCityWeather(string city)
        {
            try
            {
                // check if city name is in the database table
                if (context.Skys.Any(o => o.City.ToLower() == city.ToLower()))
                {
                    // add all 15 rows of data to a list
                    var lst = context.Skys.Where(x => x.City != null && x.City.ToLower() == city.ToLower()).ToList<Sky>();
                    foreach (var sk in lst)
                    {
                        // delete each row from the database table
                        context.Skys.Remove(new Sky() { SkyId = sk.SkyId });
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                // log the error message
            }
        }

        [NonAction]
        public async Task AddNewCityWeather(string city)
        {
            try
            {
                var client = new HttpClient();  // call weather web service with the API key, filtered by the city name
                string url = @"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/" + city + @"?key=ZK9D4C7CGWUEGC32PK36596K9";
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode(); // Throw an exception if error
                var body = response.Content.ReadAsStringAsync();
                dynamic dynJson = JsonConvert.DeserializeObject(body.Result);
                var lst = new List<Sky>();
                foreach (var item in dynJson.days)
                {
                    string day = item.datetime.ToString();
                    string ferenheit = item.temp.ToString();
                    string celcius = ((Convert.ToDouble(ferenheit) - 32) * 5 / 9).ToString("0.#");
                    string precip = item.preciptype.ToString();
                    if (precip == "")
                    {
                        precip = "None";
                    }
                    else if (precip.Contains("ice"))
                    {
                        precip = "Ice";
                    }
                    else if (precip.Contains("freezingrain"))
                    {
                        precip = "Freezingrain";
                    }
                    else if (precip.Contains("snow"))
                    {
                        precip = "Snow";
                    }
                    else if (precip.Contains("rain"))
                    {
                        precip = "Rain";
                    }
                    var sk = new Sky
                    {
                        City = city.ToUpper(),
                        Timestamp = DateTime.UtcNow.ToString(),
                        Day = day,
                        Temp = celcius,
                        Preciptype = precip
                    };
                    lst.Add(sk);
                }
                context.Skys.AddRange(lst);
                await context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                // log the error message
            }
        }

        //How to run the app:

        //1. Please have Visual Studio 2019 Community and node npm installed


        //2. Visual Studio 2019 Developer PowerShell command: install for Entity Framework and JsonConverter

        //C:\Weather>dotnet add package Microsoft.EntityFrameworkCore.Design --version 3.1.1

        //C:\Weather>dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 3.1.1

        //C:\Weather>dotnet tool install --global dotnet-ef --version 3.1.1

        //C:\Weather>dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson --version 3.1.1


        //3. Please ensure SQL Server Express 2016 LocalB is installed with Visual Studio 2019


        //4. You might not need this command but if you want to rebuild the database schema then delete the Migrations folder:

        //C:\Weather>dotnet ef migrations add Initial


        //5. Please use this command to create the database named Skys with sample data

        //C:\Weather>dotnet ef database update


        //6. If you need to reset the database, please use this command and then run command in step 5 again

        //C:\Weather>dotnet ef database drop --force


        //7. Command to install React related libraries

        //C:\Weather\ClientApp> npm install


        //8. Please use Visual Studio 2019 to open the solution file at: C:\Weather>Weather.sln


        //9. In Visual Studio 2019, click Debug and then select Start Debugging


        //10. The page https://localhost:44326/ will automatically show in a web browser window


        //11. Type a city name into the City input box and a number into the Days input box
        
    }
}
