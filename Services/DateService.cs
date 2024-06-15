using OSOS_Task_ASP.Dtos;
using OSOS_Task_ASP.Interfaces;
using OSOS_Task_ASP.Models;
using OSOS_Task_ASP.Models.HolidayApiResponse;
using Newtonsoft.Json.Linq;

namespace OSOS_Task_ASP.Services
{
    public class DateService : IDateService
    {
        public async Task<DateResponseDto?> CalculateEndDate(DateTime startDate, int workingDays)
        {
            Console.WriteLine("Service method has called");
            Console.WriteLine("Service received start date: " + startDate.ToString());
            Console.WriteLine("Service received working days: " + workingDays);

            var currentDate = startDate;
            var daysCounted = 0;
            var daysDetails = new List<DayDetails>();
            List<DateTime> holidays;

            try
            {
                holidays = await GetHolidaysAsync(startDate.Year, startDate.AddDays(workingDays).Year);
            }
            catch (Exception e)
            {
                return null;
            }


            while(daysCounted < workingDays)
            {

                if(currentDate.DayOfWeek != DayOfWeek.Saturday && 
                   currentDate.DayOfWeek != DayOfWeek.Sunday &&
                   !holidays.Contains(currentDate))
                {
                    daysCounted++;
                }

                daysDetails.Add(new DayDetails
                {
                    Date = currentDate,
                    DayOfWeek = currentDate.DayOfWeek.ToString(),
                    IsHoliday = holidays.Contains(currentDate)
                });
                
                currentDate = currentDate.AddDays(1);

            }
            return new DateResponseDto
            {
                EndDate = currentDate.AddDays(-1),
                DayDetails = daysDetails
            };
        }

        private async Task<List<DateTime>> GetHolidaysAsync(int startYear, int endYear)
        {
            Console.WriteLine("GetHolidays method has been called");
            Console.WriteLine("GetHolidays reveived year: " + startYear);

            Console.WriteLine("GetHolidays method has been called");
            Console.WriteLine("GetHolidays reveived year: " + endYear);

            var apiKey = Environment.GetEnvironmentVariable("CALENDARIFIC_API_KEY");
            List<DateTime> holidayDates = new List<DateTime>();

            for(int year = startYear; year <= endYear; year++)
            {
                var url = $"https://calendarific.com/api/v2/holidays?&api_key={apiKey}&country=LK&year={year}";

                using HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JObject holidaysJson = JObject.Parse(jsonResponse);
                    JArray? holidays = holidaysJson["response"]["holidays"] as JArray;

                    Console.WriteLine("List of holidays");

                    foreach (var holiday in holidays)
                    {
                        DateDetails dateDetails = holiday["date"].ToObject<DateDetails>();
                        DateTime holidayDate = dateDetails.ToDateTime();
                        Console.WriteLine(holidayDate.ToString());
                        holidayDates.Add(holidayDate);
                    }
                }
                else
                {
                    throw new HttpRequestException($"Failed to retrieve holidays. Status code: {response.StatusCode}");
                }
            }
            return holidayDates;
        }
    }
}