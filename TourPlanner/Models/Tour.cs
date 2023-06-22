using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using TourPlanner.Configuration;

namespace TourPlanner.Models
{
    // Tour model
    // Gets used to automatically create the table in the database
    public class Tour
    {
        [Key]
        public int TourId { get; set; } // Primary key
        
        // If a property has "= null!;", it means it is not nullable in the database
        // If a property has "<type>?", it means it is nullable in the database
        // If a property has to be nullable here, but not nullable in the database, we could use the "[Required]" attribute
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string FromLocation { get; set; } = null!;
        public string ToLocation { get; set; } = null!;

        public enum TransportTypes
        {
            Car,
            Walking,
            Bicycle
        }
        public TransportTypes TransportType { get; set; }

        //the image, the distance, and the time are retrieved by a REST request using the MapQuest Directions and Static Map APIs
        public float? TourDistance { get; set; }
        public TimeSpan? EstimatedTime { get; set; }

        // List of tour logs
        public List<TourLog> Logs { get; set; } = new ();
        
        
        // -------------------------------------------------------
        // ----------------- CALCULATED VALUES -------------------
        // -------------------------------------------------------

        public string PathToRouteImage => Path.Combine(AppConfigManager.Settings.PictureDirectory, TourId + "_Route.png");



        public int? Popularity => Logs.Count != 0 ? Logs.Count : null;

        // TODO: Check if this calculation makes any sense (it probably doesn't)
        /// <summary>
        /// Returns a value from 1 (very friendly) to 10 (unsuitable for children)
        /// </summary>
        public int? ChildFriendliness
        {
            get
            {
                // If any necessary data is null, this calculated field is null too
                if (TourDistance == null || AverageTime == null || AverageDifficulty == null) return null;
                
                // TODO: Replace these values with data from config file
                const int maxTotalHours = 3;
                const int maxAverageDifficulty = 8;
                const int maxTourDistance = 6;

                const int weightTotalHours = 2;
                const int weightAverageDifficulty = 3;
                const int weightTourDistance = 1;

                var normedTotalHours = MapFloatToRange((float) AverageTime.Value.TotalHours, 0, maxTotalHours);
                var normedAverageDifficulty = MapFloatToRange(AverageDifficulty.Value, 1, maxAverageDifficulty);
                var normedTourDistance = MapFloatToRange(TourDistance.Value, 0, maxTourDistance);

                var weightedTotalHours = normedTotalHours * weightTotalHours;
                var weightedAverageDifficulty = normedAverageDifficulty * weightAverageDifficulty;
                var weightedTourDistance = normedTourDistance * weightTourDistance;

                var weightedSum = weightedTotalHours + weightedAverageDifficulty + weightedTourDistance;
                var maxValuePossible = (maxTotalHours * weightTotalHours) +
                                       (maxAverageDifficulty * weightAverageDifficulty) +
                                       (maxTourDistance * weightTourDistance);
                var minValuePossible = weightTotalHours + weightAverageDifficulty + weightTourDistance;

                var normedSum = MapFloatToRange(weightedSum, minValuePossible, maxValuePossible);

                return normedSum;
            }
        }

        public TimeSpan? AverageTime
        {
            get
            {
                var sumOfTime = Logs.Aggregate(TimeSpan.Zero, (current, log) => current + log.Duration);

                return Logs.Count != 0 ? sumOfTime.Divide(Logs.Count) : null;
            }
        }
        
        public float? AverageDifficulty
        {
            get
            {
                var sumOfDifficulty = Logs.Aggregate(0f, (current, log) => current + log.Difficulty);

                return Logs.Count != 0 ? (sumOfDifficulty / Logs.Count) : null;
            }
        }
        
        public float? AverageRating
        {
            get
            {
                var sumOfRating = Logs.Aggregate(0f, (current, log) => current + log.Rating);

                return Logs.Count != 0 ? (sumOfRating / Logs.Count) : null;
            }
        }
        
        /// <summary>
        /// Helper function to map a given float to a range of 1 to 10
        /// </summary>
        /// <param name="value">Value to norm</param>
        /// <param name="min">Min allowed value</param>
        /// <param name="max">Max allowed value</param>
        /// <returns>Value between 1 - 10</returns>
        private int MapFloatToRange(float value, float min, float max)
        {
            // Clamp the value within the specified range
            value = Math.Max(min, Math.Min(value, max));
    
            // Map the clamped value to the range of 1 to 10
            var mappedValue = 1 + ((value - min) / (max - min)) * 9;
    
            // Round the mapped value to the nearest whole number
            var roundedValue = (int)Math.Round(mappedValue);
    
            return roundedValue;
        }
    }
}
