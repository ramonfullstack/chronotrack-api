using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronotrackService.Application
{
    [Table("extra_hours")]
    public class ExtraHoursEntity
    {
        [Column("id"), Key]
        public int Id { get; set; }

        [Column("id_user")]
        public int id_user { get; set; }

        [Column("hours_worked")]
        public decimal? HoursWorked { get; set; }

        [Column("base_rate_day")]
        public decimal? BaseRateDay { get; set; }

        [Column("value_hour_base")]
        public decimal? ValueHourBase { get; set; }

        [Column("total_value_earned_day")]
        public decimal? TotalValueEarnedDay { get; set; }

        [Column("day_of_week")]
        public string? DayOfWeek { get; set; }

        [Column("created")]
        public DateTime Created { get; set; }

        [Column("updated")]
        public DateTime Updated { get; set; }
    }
}
