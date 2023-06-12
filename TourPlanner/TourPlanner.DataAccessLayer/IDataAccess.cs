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
        public void Add(Tour tourToAdd);
        
        /// <summary>
        /// Removes the given tour from the database
        /// </summary>
        public void Delete(Tour tourToDelete);
        
        /// <summary>
        /// Saves the given modified tour to the database
        /// </summary>
        /// <param name="modifiedTour">Tour object with modifications</param>
        public void Modify(Tour modifiedTour);

        /// <summary>
        /// Saves a string to a file.
        /// </summary>
        /// <param name="absoluteFilePath">The full path of the file, f.ex. "C:\Downloads\myfile.json"</param>
        /// <param name="fileContent">The full content of the file</param>
        /// <param name="manualUserSave">If true, explorer window pops up letting user save file manually.</param>
        public void SaveToFile(string absoluteFilePath, string fileContent, bool manualUserSave = false);

        /// <summary>
        /// Reads string contents from a file. Throws an FileNotFoundException if file is not found and manual selection is disabled.
        /// </summary>
        /// <param name="absoluteFilePath">The full path of the file, f.ex. "C:\Downloads\myfile.json"</param>
        /// <param name="manualUserSelectWhenNotFound">If true, explorer window pops up letting user select file to read manually if path is not found.</param>
        /// <returns>Content of the file</returns>
        public string ReadFromFile(string absoluteFilePath, bool manualUserSelectWhenNotFound = false);
    }
}
