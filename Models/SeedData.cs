using System;
using System.Linq;

namespace Weather.Models
{
    public static class SeedData
    {
        public static void SeedDatabase(DataContext context)
        {
            if (context.Skys.Count() == 0)
            {
                context.Skys.AddRange(
                new Sky
                {
                    City = "Nonamecity",
                    Timestamp = DateTime.UtcNow.ToString(),
                    Day = "2022-05-31",
                    Temp = "-7.1",
                    Preciptype = "Ice"
                },
                new Sky
                {
                    City = "Nonamecity",
                    Timestamp = DateTime.UtcNow.ToString(),
                    Day = "2022-06-01",
                    Temp = "-1.7",
                    Preciptype = "Freezingrain"
                },
                new Sky
                {
                    City = "Nonamecity",
                    Timestamp = DateTime.UtcNow.ToString(),
                    Day = "2022-06-02",
                    Temp = "-3.6",
                    Preciptype = "Snow"
                },
                new Sky
                {
                    City = "Nonamecity",
                    Timestamp = DateTime.UtcNow.ToString(),
                    Day = "2022-06-03",
                    Temp = "7.3",
                    Preciptype = "Rain"
                },
                new Sky
                {
                    City = "Nonamecity",
                    Timestamp = DateTime.UtcNow.ToString(),
                    Day = "2022-06-04",
                    Temp = "18.2",
                    Preciptype = "None"
                });
                context.SaveChanges();
            }
        }
    }
}
