using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    internal interface IDataAccess
    {
        public List<Tour> GetTours();
    }
}
