using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer.DataAccessObjects
{
    public class TourDao
    {
        private readonly IDataAccess _dataAccess;

        public TourDao()
        {
            //check which data source
            _dataAccess = Database.Instance;
        }
        
        public List<Tour> GetTours()
        {
            return _dataAccess.GetTours();
        }

        public bool Add(Tour tourToAdd)
        {
            return _dataAccess.Add(tourToAdd);
        }

        public bool Delete(Tour tourToDelete)
        {
            return _dataAccess.Delete(tourToDelete);
        }
    }
}
