using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Documents;

namespace TourPlanner.Models
{
    // Tour model
    // Gets used to automatically create the table in the database (so you better not add any logic in here cuz that breaks it)
    public class Tour
    {
        [Key]
        public int TourId { get; set; } // Primary key
        
        // If a property has "= null!;", it means it is not nullable in the database
        // If a property has "<type>?", it means it is nullable in the database
        // If a property has to be nullable here, but not nullable in the database, we could use the "[Required]" attribute
        // To create foreign keys, we should just watch tutorials again :)
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string FromLocation { get; set; } = null!;
        public string ToLocation { get; set; } = null!;
        public string? TransportType { get; set; }

        //the image, the distance, and the time should be retrieved by a REST request using the MapQuest Directions and Static Map APIs
        public float? TourDistance { get; set; }
        public DateTime? EstimatedTime { get; set; }

        // List of tour logs
        public List<TourLog> Logs { get; set; } = new List<TourLog>();
        
        // public string RouteInfo { get; set; } //image with the tour map (We professionally ignore that for now yes? Yes.)
    }
}
