using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OSOS_Task_ASP.Models.HolidayApiResponse
{
    public class HolidayApiResponse
    {
        public MetaData? Meta { get; set; }
        public HolidayResponse? Response { get; set; }
    }

    public class MetaData
    {
        public int Code { get; set; }
    }

    public class HolidayResponse
    {
        public List<Holiday>? Holidays { get; set; }
    }

    public class Holiday
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateDetails? Date { get; set; }
        public List<string>? Type { get; set; }

    // Method to convert DateDetails to DateTime
        [JsonIgnore] // Ignore this property during serialization
        public DateTime HolidayDate => Date?.ToDateTime() ?? DateTime.MinValue;
    }

    public class DateDetails
    {
        public string? Iso { get; set; }
        public DateTimeDetails? DateTime { get; set; }

    // Method to convert DateDetails to DateTime
        public DateTime ToDateTime()
        {
            if (DateTime != null)
            {
                return new DateTime(DateTime.Year, DateTime.Month, DateTime.Day);
            }
            else
            {
                throw new InvalidOperationException("Invalid date format.");
            }
        }
    }

    public class DateTimeDetails
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }
}