using OSOS_Task_ASP.Dtos;

namespace OSOS_Task_ASP.Interfaces
{
    public interface IDateService
    {
        Task<DateResponseDto> CalculateEndDate(DateOnly startDate, int workingDays);
    }
}