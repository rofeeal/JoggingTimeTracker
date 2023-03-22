using System.ComponentModel.DataAnnotations;

namespace JoggingTimeTracker.API.DTOs.JoggingTime
{
    public class JoggingTimeDto
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public float DistanceInMeter { get; set; }

        public string Time { get; set; }

        public double Speed { get; set; }

    }
}
