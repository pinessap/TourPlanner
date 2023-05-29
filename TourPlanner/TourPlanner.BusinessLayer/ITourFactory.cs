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
        /// Saves the changes in the given tour to the database
        /// </summary>
        /// <param searchValue="modifiedTour">Tour object with modifications</param>
        /// <returns>True if successful, false if not</returns>
        bool Modify(Tour modifiedTour);

        /// <summary>
        /// Exports a given list of tours to a json file
        /// </summary>
        /// <param name="toursToExport">The tours that should get exported to a file</param>
        /// <param name="fileName">Filename without file-extension</param>
        /// <returns>True if successful, false if not</returns>
        bool Export(List<Tour> toursToExport, string fileName);

        /// <summary>
        /// Imports a given json file into the database, deletes all old entries
        /// </summary>
        /// <param name="fileName">Filename without file-extension (file type should be .json)</param>
        /// <returns>True if successful, false if not</returns>
        bool ImportOverride(string fileName);

        /// <summary>
        /// Imports a given json file into the database by appending everything as a new entry
        /// </summary>
        /// <param name="fileName">Filename without file-extension (file type should be .json)</param>
        /// <returns>True if successful, false if not</returns>
        bool ImportAppend(string fileName);

        /// <summary>
        /// Generates a PDF report from a given tour
        /// </summary>
        /// <param name="tourToGenerateReportFrom">Tour used to generate the report from</param>
        bool GenerateSingleReport(Tour tourToGenerateReportFrom);
        
        /// <summary>
        /// Generates a PFD report for a given list of tours
        /// </summary>
        /// <param name="toursToGenerateFrom">List of tours used to create the report</param>
        bool GenerateSummarizedReport(List<Tour> toursToGenerateFrom);
    }
}
