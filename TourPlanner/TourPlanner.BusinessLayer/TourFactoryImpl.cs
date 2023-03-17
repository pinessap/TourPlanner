using System;
using System.Collections.Generic;
using System.Linq;
using TourPlanner.DataAccessLayer.DataAccessObjects;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    internal class TourFactoryImpl : ITourFactory
    {
        /// <summary>
        /// The object containing all Tour Data information
        /// </summary>
        private readonly TourDao _tourDao = new TourDao();
        
        /// <summary>
        /// Get all tours in the database
        /// </summary>
        public List<Tour> GetTours()
        {
            return _tourDao.GetTours();
        }
        
        /// <summary>
        /// Search for a string in all tour names
        /// </summary>
        /// <param name="name">The string to search for</param>
        /// <param name="caseSensitive">Default search is not case sensitive (false)</param>
        /// <returns>List of tours which contain the search string in their name</returns>
        public List<Tour> Search(string name, bool caseSensitive = false)
        {
            var items = GetTours();

            List<Tour> foundTours = new();

            foundTours.AddRange(caseSensitive ? 
                items.Where(x => x.Name.Contains(name)) : 
                items.Where(x => x.Name.ToLower().Contains(name.ToLower()))
            );

            return foundTours;
        }

        /// <summary>
        /// Adds a dummy tour to the database
        /// </summary>
        /// <returns>True if successful, false if not</returns>
        public bool Add()
        {
            // Get existing tours and either set the lastTourId to 0 or whatever the last id in the DB is
            var existingTours = GetTours();
            var lastTourId = existingTours.Count > 0 ? GetTours().Last().TourId : 0;
            
            // BUG: If the database is empty, but the autoincrement field is not at 0, the "new" first entry might f.ex. get the idValue 7 instead of the assumed value 1
            var newTourName = "Tour with Database-ID " + (lastTourId + 1);

            var tourToAdd = new Tour
            {
                Name = newTourName,
                Description = "Mc?",
                FromLocation = "Productivity",
                ToLocation = "Nati's Folterkeller"
            };
            
            return _tourDao.Add(tourToAdd);
        }
        
        /// <summary>
        /// Deletes the given tour from the database
        /// </summary>
        /// <returns>True if successful, false if not</returns>
        public bool Delete(Tour tourToDelete)
        {
            return _tourDao.Delete(tourToDelete);
        }
    }
}
