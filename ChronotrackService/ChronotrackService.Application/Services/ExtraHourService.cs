using Microsoft.EntityFrameworkCore;

namespace ChronotrackService.Application
{
    public class ExtraHourService : IExtraHourService
    {
        private readonly ChronotrackContext chronotrackContext;

        public ExtraHourService(
            ChronotrackContext chronotrackContext)
        {
            this.chronotrackContext = chronotrackContext;
        }

        public async Task<List<ExtraHoursEntity>> GetExtraHoursByUserAsync(int idUser)
        {
            return await chronotrackContext.EntraHours
                .Where(e => e.IdUser == idUser)
                .ToListAsync();
        }

        public async Task<ExtraHoursEntity> SaveOrUpdateExtraHourAsync(ExtraHoursEntity extraHour)
        {
            if (extraHour.Id == 0) 
            {
                extraHour.Created = DateTime.UtcNow;
                extraHour.Updated = DateTime.UtcNow;
                await chronotrackContext.EntraHours.AddAsync(extraHour);
            }
            else
            {
                chronotrackContext.EntraHours.Update(extraHour);
            }

            await chronotrackContext.SaveChangesAsync();
            return extraHour;
        }

        public async Task<ExtraHoursEntity> GetExtraHourByIdAsync(int id)
        {
            return await chronotrackContext.EntraHours
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
