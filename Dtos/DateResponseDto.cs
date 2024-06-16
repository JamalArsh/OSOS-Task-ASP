using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSOS_Task_ASP.Models;

namespace OSOS_Task_ASP.Dtos
{
    public class DateResponseDto
    {
        public DateOnly EndDate{ get; set; }
        public List<DayDetails>? DayDetails { get; set; }
    }
}