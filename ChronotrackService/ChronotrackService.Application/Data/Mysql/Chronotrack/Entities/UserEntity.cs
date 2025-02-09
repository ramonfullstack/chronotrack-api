using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronotrackService.Application
{
    [Table("user")]
    public class UserEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [Column("password")]
        public string Password { get; set; }

        [Column("max_retries")]
        public int MaxRetries { get; set; }

        [Column("salary")]
        public decimal Salary { get; set; }

        [Column("updated")]
        public DateTime Updated { get; set; }

        [Column("created")]
        public DateTime Created { get; set; }
    }
}
