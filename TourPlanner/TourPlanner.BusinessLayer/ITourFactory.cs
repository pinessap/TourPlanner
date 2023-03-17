using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    public interface ITourFactory
    {
        List<Tour> GetTours();
        List<Tour> Search(string name, bool caseSensitive = false);
        bool Add();
        bool Delete(Tour tourToDelete);
    }
}
