using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    internal class FileSystem : IDataAccess //for import and export of tour data (file format of your choice) ?
    {
        private string filePath;

        public FileSystem()
        {
            //this.filePath = "";
        }
        public List<Tour> GetTours()
        {
            //get media items from file system

            //ZUM TESTEN
            return new List<Tour>() //ZUM TESTEN
            {
                new Tour() { Name = "Tour1" },
                new Tour() { Name = "Tour2"},
                new Tour() { Name = "Tour3" }
            };
        }
    }
}
