using System.ComponentModel.DataAnnotations;

namespace JoggingTimeTracker.API.DTOs.JoggingTime
{
    public class JoggingTimeAddDto
    {
        [Required]
        [DataType(DataType.Date, ErrorMessage = "The Date field must be a valid date.")]
        public DateTime Date { get; set; }

        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "The Distance field must be greater than or equal to 0.")]
        public float DistanceInMeter { get; set; }

        [Required]
        public string Time { get; set; }
    }
}
