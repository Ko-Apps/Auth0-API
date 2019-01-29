using System.ComponentModel.DataAnnotations;

namespace scheduler.Models
{
    public class ScheduleViewModel
    {
        [Required]
        public string GoogleId { get; set; }

        [Required(ErrorMessage = "You should provide a Date")]
        [MaxLength(50)]
        public string Date { get; set; }

        [Required(ErrorMessage = "You should provide the hours")]
        [Range(0, 24)]
        public int Hours { get; set; }
    }
}
