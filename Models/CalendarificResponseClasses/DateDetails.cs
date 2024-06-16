using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OSOS_Task_ASP.Models.CalendarificResponseClasses
{

    public class DateDetails
    {
        public string? Iso { get; set; }
        public DateTimeDetails? DateTime { get; set; }
    }

}