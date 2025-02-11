namespace ChronotrackService.Application
{
    public interface IExtraHourService
    {
        Task<List<ExtraHoursEntity>> GetExtraHoursByUserAsync(int idUser);
        Task<ExtraHoursEntity> SaveOrUpdateExtraHourAsync(ExtraHoursEntity extraHour);
        Task<ExtraHoursEntity> GetExtraHourByIdAsync(int id);
        Task<byte[]> GenerateCsvReportAsync(int idUser);
    }
}
