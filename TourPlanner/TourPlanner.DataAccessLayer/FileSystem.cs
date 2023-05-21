using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    internal class FileSystem : IDataAccess //for import and export of tour data (file format of your choice) ?
    {
        private string _filePath;

        public FileSystem()
        {
            this._filePath = "";
        }
        
        public List<Tour> GetTours()
        {
            //get media items from file system
            
            return new List<Tour>() //ZUM TESTEN
            {
                new() { Name = "Tour1" },
                new() { Name = "Tour2"},
                new() { Name = "Tour3" }
            };
        }

        public bool Add(Tour tourToAdd)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(Tour tourToDelete)
        {
            throw new System.NotImplementedException();
        }

        public bool Modify(Tour modifiedTour)
        {
            throw new System.NotImplementedException();
        }
    }
}
