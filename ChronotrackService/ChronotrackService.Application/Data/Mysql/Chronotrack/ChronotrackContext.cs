using ChronotrackService.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace ChronotrackService.Application
{
    public class ChronotrackContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ExtraHoursEntity> EntraHours { get; set; }
        public ChronotrackContext(DbContextOptions<ChronotrackContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity
                {
                    Id = 1,
                    Name = "Ramon Silva",
                    Email = "ramonss.bque@gmail.com",
                    Password = "Ramon@@1995",
                    Salary = 15000,
                    MaxRetries = 3,
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                }
            );

            modelBuilder.Entity<ExtraHoursEntity>().HasData(
               new ExtraHoursEntity
               {
                   Id = 1,
                   id_user = 1,
                   BaseRateDay = 2,
                   DayOfWeek = "Domingo",
                   HoursWorked = 3,
                   ValueHourBase = 70,
                   TotalValueEarnedDay = 420,
                   Created = DateTime.Now,
                   Updated = DateTime.Now,
               }
           );
        }
    }
}
