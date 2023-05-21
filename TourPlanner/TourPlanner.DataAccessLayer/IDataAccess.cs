using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    internal interface IDataAccess
    {
        /// <summary>
        /// Get all tours from the database
        /// </summary>
        public List<Tour> GetTours();
        
        /// <summary>
        /// Adds given tour to the database
        /// </summary>
        /// <returns>True if successful, false on an error</returns>
        public bool Add(Tour tourToAdd);
        
        /// <summary>
        /// Removes the given tour from the database
        /// </summary>
        /// <returns>True if successful, false on an error</returns>
        public bool Delete(Tour tourToDelete);
        
        /// <summary>
        /// Saves the given modified tour to the database
        /// </summary>
        /// <param name="modifiedTour">Tour object with modifications</param>
        /// <returns>True if successful, false if not</returns>
        public bool Modify(Tour modifiedTour);
    }
}
