using OSOS_Task_ASP.Dtos;
using OSOS_Task_ASP.Interfaces;
using OSOS_Task_ASP.Models;
using OSOS_Task_ASP.Models.CalendarificResponseClasses;
using Newtonsoft.Json.Linq;

namespace OSOS_Task_ASP.Services
{
    public class DateService : IDateService
    {
        public async Task<DateResponseDto?> CalculateEndDate(DateOnly startDate, int workingDays)
        {
            var currentDate = startDate;
            var daysCounted = 0;
            var daysDetails = new List<DayDetails>();
            List<DateOnly> holidays;

            try
            {
                holidays = await GetHolidaysAsync(startDate.Year, startDate.AddDays(workingDays).Year);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
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

        private async Task<List<DateOnly>> GetHolidaysAsync(int startYear, int endYear)
        {
            var apiKey = Environment.GetEnvironmentVariable("CALENDARIFIC_API_KEY");
            List<DateOnly> holidayDates = new List<DateOnly>();

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

                    foreach (var holiday in holidays)
                    {
                        DateDetails dateDetails = holiday["date"].ToObject<DateDetails>();

                        DateOnly holidayDate = new DateOnly(dateDetails.DateTime.Year,
                                                            dateDetails.DateTime.Month, 
                                                            dateDetails.DateTime.Day);

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