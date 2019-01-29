using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace scheduler.Models
{
    public class Schedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string GoogleId { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        [Range(0, 24)]
        public int Hours { get; set; }

    }
}
