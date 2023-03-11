using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public class TourDAO
    {
        private IDataAccess dataAccess;

        public TourDAO()
        {
            //check which data source
            dataAccess = new Database();
        }

        public List<Tour> GetTours()
        {
            return dataAccess.GetTours();
        }
    }
}
