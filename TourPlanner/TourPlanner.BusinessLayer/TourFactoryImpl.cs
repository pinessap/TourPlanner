using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TourPlanner.Configuration;
using TourPlanner.DataAccessLayer.DataAccessObjects;
using TourPlanner.Logging;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    internal class TourFactoryImpl : ITourFactory
    {
        /// <summary>
        /// The object containing all Tour Data information
        /// </summary>
        private readonly TourDao _tourDao = new TourDao();

        public List<Tour> GetTours()
        {
            return _tourDao.GetTours();
        }
        
        public List<Tour> Search(string searchValue, bool caseSensitive = false)
        {
            if (searchValue is null or "") AppLogger.Warn("SearchValue is empty or null");

            var items = GetTours();

            List<Tour> foundTours = new();

            try
            {
                foundTours.AddRange(items.Where(x => FullTextSearch(searchValue, x, caseSensitive)));
            }
            catch (Exception ex)
            {
                AppLogger.ThrowError("SEARCH eThrowErrorrror:", ex);
            }

            AppLogger.Info("Found " + foundTours.Count + " tours that contain \"" + searchValue + "\"");

            return foundTours;
        }
        
        public void Add(Tour tourToAdd)
        {
            AppLogger.Info("Adding new tour \"" + tourToAdd.Name + "\"");
            _tourDao.Add(tourToAdd);
            AppLogger.Info("Add tour \"" + tourToAdd.Name + "\" successful");
        }
        
        public void Delete(Tour tourToDelete)
        {
            AppLogger.Info("Deleting tour \"" + tourToDelete.Name + "\"");
            _tourDao.Delete(tourToDelete);
            AppLogger.Info("Delete tour \"" + tourToDelete.Name + "\" successful");

        }
        
        public void Modify(Tour modifiedTour)
        {
            AppLogger.Info("Modifying tour \"" + modifiedTour.Name + "\"");
            _tourDao.Modify(modifiedTour);
            AppLogger.Info("Modify tour \"" + modifiedTour.Name + "\" successful");
        }

        public void Export(List<Tour> toursToExport)
        {
            var absolutePath = Path.Combine(AppConfigManager.Settings.OutputDirectory, "export.json");
            AppLogger.Info("Try Exporting "+ toursToExport.Count + " tours to \"" + absolutePath + "\"");
            
            var jsonArray = ConvertToursToJson(toursToExport);

            _tourDao.SaveToFile(absolutePath, jsonArray, true);
            
            AppLogger.Info("Export to \"" + absolutePath + "\" successful");
        }

        public void ImportOverride()
        {
            var defaultAbsoluteLocationOfFile = Path.Combine(AppConfigManager.Settings.OutputDirectory, "importData.json");
            
            AppLogger.Info("Try OverrideImport from \"" + defaultAbsoluteLocationOfFile + "\"");
            
            var jsonString = _tourDao.ReadFromFile(defaultAbsoluteLocationOfFile, true);

            var toursToOverride = ConvertJsonToTours(jsonString);
            
            // Delete all tours from the database
            DeleteAllTours();
            
            // Add new tours back
            foreach (var tour in toursToOverride)
            {
                _tourDao.Add(tour);
            }
            
            AppLogger.Info("OverrideImport successful");
        }

        public void ImportAppend()
        {
            var defaultAbsoluteLocationOfFile = Path.Combine(AppConfigManager.Settings.OutputDirectory, "importData.json");

            AppLogger.Info("Try AppendImport from \"" + defaultAbsoluteLocationOfFile + "\"");

            var jsonString = _tourDao.ReadFromFile(defaultAbsoluteLocationOfFile, true);

            var toursToAppend = ConvertJsonToTours(jsonString);
            
            foreach (var tour in toursToAppend)
            {
                // Reset tour-ID to make sure ef treats tour as new entry
                tour.TourId = 0;
                
                // Do the same for all tourLogs
                foreach (var log in tour.Logs) { log.TourLogId = 0;}

                _tourDao.Add(tour);
            }
            
            AppLogger.Info("AppendImport successful");
        }

        public void GenerateSingleReport(Tour tourToGenerateReportFrom)
        {
            AppLogger.Info("Generating SingleReport of \"" + tourToGenerateReportFrom.Name + "\"");
            
            var pathToTourTemplate = Path.Combine(AppConfigManager.Settings.TemplateDirectory, "tourTemplate.html");
            var pathToTourLogTemplate = Path.Combine(AppConfigManager.Settings.TemplateDirectory, "tourLogTemplate.html");
            
            var htmlSourceTour = _tourDao.ReadFromFile(pathToTourTemplate);
            var htmlSourceTourLog = _tourDao.ReadFromFile(pathToTourLogTemplate);
            
            var htmlWithTourData = ReplaceHtmlPlaceholdersWithTourData(htmlSourceTour, tourToGenerateReportFrom);
            var htmlWithAllData = htmlWithTourData + CreateTourLogsHtml(tourToGenerateReportFrom.Logs, htmlSourceTourLog);
            
            var absolutePath = Path.Combine(AppConfigManager.Settings.OutputDirectory, tourToGenerateReportFrom.Name + "_Report.pdf");

            _tourDao.SaveToFile(absolutePath, htmlWithAllData, true);
        }
        
        public void GenerateSummarizedReport(List<Tour> toursToGenerateFrom)
        {
            AppLogger.Info("Generating SummarizedReport of " + toursToGenerateFrom.Count + " tours");

            var pathToTourSummaryHeaderTemplate = Path.Combine(AppConfigManager.Settings.TemplateDirectory, "tourSummaryHeaderTemplate.html");
            var pathToTourSummaryEntryTemplate = Path.Combine(AppConfigManager.Settings.TemplateDirectory, "tourSummaryEntryTemplate.html");

            var htmlSourceHeaderTemplate = _tourDao.ReadFromFile(pathToTourSummaryHeaderTemplate);
            var htmlSourceEntryTemplate = _tourDao.ReadFromFile(pathToTourSummaryEntryTemplate);
            
            var tourDataHtml = CreateSummarizedToursHtml(htmlSourceEntryTemplate, toursToGenerateFrom);
            var fullHtml = htmlSourceHeaderTemplate + tourDataHtml;
        
            var absolutePath = Path.Combine(AppConfigManager.Settings.OutputDirectory, "Summarized_Report.pdf");

            _tourDao.SaveToFile(absolutePath, fullHtml, true);
        }

        /// <summary>
        /// Searches tour for given searchValue
        /// </summary>
        /// <param name="searchValue">The string to search for</param>
        /// <param name="tourToSearch">The tour in which the string gets searched</param>
        /// <param name="caseSensitive">If true the search is case sensitive, insensitive if false</param>
        /// <returns>True if searchValue was found, false if not</returns>
        private bool FullTextSearch(string searchValue, Tour tourToSearch, bool caseSensitive)
        {
            searchValue = searchValue ?? "";
            
            // Fill search list with basic tour information
            var stringsToSearch = new List<string>
            {
                tourToSearch.Name,
                tourToSearch.Description ?? "",
                tourToSearch.ToLocation,
                tourToSearch.FromLocation,
                tourToSearch.TransportType ?? "",
                (tourToSearch.ChildFriendliness == null ? tourToSearch.ChildFriendliness.ToString() : "")!,
                (tourToSearch.Popularity == null ? tourToSearch.Popularity.ToString() : "")!
            };
            
            // Fill search list with tour log information
            foreach (var log in tourToSearch.Logs)
            {
                stringsToSearch.Add(log.Comment);
            }
            
            // Check if searchValue is present in any added information
            foreach (var interestingInformation in stringsToSearch)
            {
                var searchInformation = caseSensitive ? interestingInformation : interestingInformation.ToLower();
                searchValue = caseSensitive ? searchValue : searchValue.ToLower();
                
                if (searchInformation.Contains(searchValue))
                    return true;
            }
            
            // If nothing was found, return false
            return false;
        }

        /// <summary>
        /// Converts a list of tours to a json string
        /// </summary>
        /// <returns>A json-string looking something like this: "[{paramName1: "paramValue", ...}, ...]"</returns>
        private string ConvertToursToJson(List<Tour> toursToConvert)
        {
            // Initialize jsonArray with an opening bracket 
            var jsonArray = "[";

            try
            {
                // Add all tours as json
                foreach (var tour in toursToConvert)
                {
                    var serializedTour = JsonConvert.SerializeObject(tour);
                    jsonArray += serializedTour + ",";
                }

                // Remove trailing comma
                jsonArray.TrimEnd(',');
            }
            catch (Exception ex)
            {
                AppLogger.Error("TourToJsonConversion error:" + ex.Message);
            }
            
            // Add closing bracket
            jsonArray += "]";
            return jsonArray;
        }

        /// <summary>
        /// Converts a json string to a List of tours
        /// </summary>
        /// <param name="jsonString">A json string in the following format: "[{paramName1: "paramValue", ...}, ...]"</param>
        /// <returns></returns>
        private List<Tour> ConvertJsonToTours(string jsonString)
        {
            List<Tour>? tours = null;
            
            try
            {
                tours = JsonConvert.DeserializeObject<List<Tour>>(jsonString);
            }
            catch (Exception ex)
            {
                AppLogger.ThrowError("JsonToTourConversion error:", ex);
            }

            return tours!;
        }

        /// <summary>
        /// Delete all tours from the database
        /// </summary>
        private void DeleteAllTours()
        {
            AppLogger.Info("Deleting all tours from database.");

            var allTours = _tourDao.GetTours();

            foreach (var tour in allTours)
            {
                _tourDao.Delete(tour);
            }
        }
        
        /// <summary>
        /// Replaces all {{TourModelPropertyName}} placeholders inside html string
        /// </summary>
        /// <param name="htmlStringWithPlaceholders">Html string containing placeholders in the following format: {{TourModelPropertyName}}</param>
        /// <param name="tourWithData">Tour which contains the data-values we want to add to the html.</param>
        /// <returns>Html string with replaced placeholders</returns>
        private string ReplaceHtmlPlaceholdersWithTourData(string htmlStringWithPlaceholders, Tour tourWithData)
        {
            try
            {
                var finalHtml = htmlStringWithPlaceholders;

                foreach (var dataPoint in tourWithData.GetType().GetProperties())
                {
                    var dataPointName = dataPoint.Name;

                    var dataPointValue = "";

                    if (dataPoint.GetValue(tourWithData) == null || dataPoint.GetValue(tourWithData)!.ToString() == "")
                    {
                        dataPointValue = "<em>No data</em>";
                    }
                    else
                    {
                        dataPointValue = dataPoint.GetValue(tourWithData)!.ToString();
                    }

                    finalHtml = finalHtml.Replace("{{" + dataPointName + "}}", dataPointValue);
                }

                return finalHtml;
            }
            catch (Exception ex)
            {
                AppLogger.ThrowError("TemplateReplacing error:", ex);
            }

            return null!;
        }
        
        /// <summary>
        /// Creates an html string with a given list of tourLogs
        /// </summary>
        /// <returns>Html string with all tour log information</returns>
        private string CreateTourLogsHtml(List<TourLog> tourLogs, string tourLogTemplateHtml)
        {
            var finalHtml = "";
        
        
            foreach (var tourLog in tourLogs)
            {
                var tourLogHtml = tourLogTemplateHtml;
            
                foreach (var dataPoint in tourLog.GetType().GetProperties())
                {
                    var dataPointName = dataPoint.Name;
                    var dataPointValue = "";

                    if (dataPoint.GetValue(tourLog) == null || dataPoint.GetValue(tourLog)!.ToString() == "")
                    {
                        dataPointValue = "<em>No data</em>";
                    }
                    else
                    {
                        dataPointValue = dataPoint.GetValue(tourLog)!.ToString();
                    }
            
                    tourLogHtml = tourLogHtml.Replace("{{" + dataPointName + "}}", dataPointValue);
                }

                finalHtml += tourLogHtml;
            }

            if (finalHtml == "") finalHtml = "<p><em>No data</em></p>";

            return finalHtml;
        }

        /// <summary>
        /// Creates html string from given list of tours
        /// </summary>
        /// <param name="htmlStringWithPlaceholders">Html template for each individual tour</param>
        /// <param name="tours">List of tours containing data to replace in template</param>
        /// <returns>Html string with filled tour-templates of all tours</returns>
        private string CreateSummarizedToursHtml(string htmlStringWithPlaceholders, List<Tour> tours)
        {
            return tours.Aggregate("", (current, tour) => current + ReplaceHtmlPlaceholdersWithTourData(htmlStringWithPlaceholders, tour));
        }
    }
}
