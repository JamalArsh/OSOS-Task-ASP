namespace OSOS_Task_ASP.Models
{
    public class DayDetails
    {
        public DateOnly Date{ get; set; }
        public string? DayOfWeek { get; set;}
        public bool IsHoliday{ get; set;}
    }
}