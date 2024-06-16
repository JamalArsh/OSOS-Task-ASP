using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSOS_Task_ASP.Dtos
{
    public class DateRequestDto
    {
        public DateOnly StartDate {get; set;}
        public int WorkingDays{get; set;}
    }
}