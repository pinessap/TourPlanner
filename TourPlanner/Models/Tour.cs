using System;

namespace TourPlanner.Models
{
    //one class for tour and one for tour logs? -> comparable to MediaItem from WpfIntro Example in Moodle (bruh ich weiﬂ auch nicht psssst my tummy hurts gar nciht slay)
    public class Tour
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Transport { get; set; }

        //the image, the distance, and the time should be retrieved by a REST request using the MapQuest Directions and Static Map APIs
        public double Distance { get; set; }
        public DateTime Time { get; set; }
        public string RouteInfo { get; set; } //string? image with the tour map
    }
}
