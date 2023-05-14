using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourPlanner.Models
{
    // TourLog model
    // Gets used to automatically create the table in the database (so you better not add any logic in here cuz that breaks it)
    public class TourLog
    {
        [Key] public int TourLogId { get; set; } // Primary key
        [Required] public DateTime Time { get; set; }
        public string Comment { get; set; } = null!;
        [Required] public int Difficulty { get; set; }
        [Required] public TimeSpan Duration { get; set; }
        [Required] public float Rating { get; set; }
    }
}
