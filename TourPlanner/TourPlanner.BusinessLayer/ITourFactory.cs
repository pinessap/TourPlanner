using System.Collections.Generic;
using System.Threading.Tasks;
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
        void Add(Tour tourToAdd);
        
        /// <summary>
        /// Deletes the given tour from the database
        /// </summary>
        void Delete(Tour tourToDelete);
        
        /// <summary>
        /// Saves the changes in the given tour to the database
        /// </summary>
        /// <param searchValue="modifiedTour">Tour object with modifications</param>
        void Modify(Tour modifiedTour);

        /// <summary>
        /// Exports a given list of tours to a json file. File Explorer opens and lets user save file.
        /// </summary>
        /// <param name="toursToExport">The tours that should get exported to a file</param>
        void Export(List<Tour> toursToExport);

        /// <summary>
        /// Imports a given json file into the database, deletes all old entries. If file given is not found, opens File Explorer to let user select one.
        /// </summary>
        void ImportOverride();

        /// <summary>
        /// Imports a given json file into the database by appending everything as a new entry. If file given is not found, opens File Explorer to let user select one.
        /// </summary>
        void ImportAppend();

        /// <summary>
        /// Generates a PDF report from a given tour
        /// </summary>
        /// <param name="tourToGenerateReportFrom">Tour used to generate the report from</param>
        void GenerateSingleReport(Tour tourToGenerateReportFrom);
        
        /// <summary>
        /// Generates a PFD report for a given list of tours
        /// </summary>
        /// <param name="toursToGenerateFrom">List of tours used to create the report</param>
        void GenerateSummarizedReport(List<Tour> toursToGenerateFrom);

        Task AddApiInformation(Tour tourWithoutApiValues);
    }
}
