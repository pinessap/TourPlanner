using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    public interface ITourFactory
    {
        /// <summary>
        /// Get all tours in the database
        /// </summary>
        List<Tour> GetTours();
        
        /// <summary>
        /// Search for a string in all tour names
        /// </summary>
        /// <param searchValue="searchValue">The string to search for</param>
        /// <param searchValue="caseSensitive">Default search is not case sensitive (false)</param>
        /// <returns>List of tours which contain the search string in their searchValue</returns>
        List<Tour> Search(string searchValue, bool caseSensitive = false);
        
        /// <summary>
        /// Adds a dummy tour to the database
        /// </summary>
        /// <returns>True if successful, false if not</returns>
        bool Add();
        
        /// <summary>
        /// Deletes the given tour from the database
        /// </summary>
        /// <returns>True if successful, false if not</returns>
        bool Delete(Tour tourToDelete);
        
        /// <summary>
        /// Saves the given modified tour to the database
        /// </summary>
        /// <param searchValue="modifiedTour">Tour object with modifications</param>
        /// <returns>True if successful, false if not</returns>
        bool Modify(Tour modifiedTour);
    }
}
