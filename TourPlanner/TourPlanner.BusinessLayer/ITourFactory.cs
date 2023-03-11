using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    public interface ITourFactory
    {
        IEnumerable<Tour> GetTours();
        IEnumerable<Tour> Search(string name, bool caseSensitive = false);
    }
}
