using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JoggingTimeTracker.Core.Models
{
    public class JoggingTime
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JoggingTimeId { get; set; }

        [Required]
        [DataType(DataType.Date, ErrorMessage = "The Date field must be a valid date.")]
        public DateTime Date { get; set; }

        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "The Distance field must be greater than or equal to 0.")]
        public float DistanceInMeter { get; set; }

        [Required]
        public TimeSpan Time { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double Speed { get { return DistanceInMeter / (float)Time.TotalHours; } }
    }
}
