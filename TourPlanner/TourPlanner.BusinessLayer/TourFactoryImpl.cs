﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TourPlanner.Configuration;
using TourPlanner.BusinessLayer.Reports;
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
        /// The object containing all report generation functions
        /// </summary>
        private readonly IReportGenerator _reportFactory = ReportGeneratorFactory.Instance;

        public List<Tour> GetTours()
        {
            return _tourDao.GetTours();
        }
        
        public List<Tour> Search(string searchValue, bool caseSensitive = false)
        {
            var items = GetTours();

            List<Tour> foundTours = new();

            foundTours.AddRange(items.Where(x => FullTextSearch(searchValue, x, caseSensitive)));

            return foundTours;
        }
        
        public bool Add(Tour tourToAdd)
        {
            return _tourDao.Add(tourToAdd);
        }
        
        public bool Delete(Tour tourToDelete)
        {
            return _tourDao.Delete(tourToDelete);
        }
        
        public bool Modify(Tour modifiedTour)
        {
            return _tourDao.Modify(modifiedTour);
        }

        // TODO: Change logic so saving of file happens in Presentation layer by letting user save file himself in filesystem
        public bool Export(List<Tour> toursToExport, string fileName)
        {
            var jsonArray = ConvertToursToJson(toursToExport);
            
            return CreateJsonFile(fileName, jsonArray);
        }

        public bool ImportOverride(string absoluteLocationOfFile)
        {
            var jsonString = ReadJsonFile(absoluteLocationOfFile);

            var toursToOverride = ConvertJsonToTours(jsonString);
            
            // Delete all tours from the database
            if (!DeleteAllTours()) return false;
            
            // Add new tours back
            foreach (var tour in toursToOverride)
            {
                if (!_tourDao.Add(tour))
                    return false;
            }

            return true;
        }

        public bool ImportAppend(string absoluteLocationOfFile)
        {
            var jsonString = ReadJsonFile(absoluteLocationOfFile);

            var toursToAppend = ConvertJsonToTours(jsonString);
            
            foreach (var tour in toursToAppend)
            {
                // Reset tour-ID to make sure ef treats tour as new entry
                tour.TourId = 0;

                if (!_tourDao.Add(tour))
                    return false;
            }

            return true;
        }

        // TODO: Change logic so saving of file happens in Presentation layer by letting user save file himself in filesystem
        public bool GenerateSingleReport(Tour tourToGenerateReportFrom)
        {
            return _reportFactory.GenerateSingleReport(tourToGenerateReportFrom);
        }
        
        // TODO: Change logic so saving of file happens in Presentation layer by letting user save file himself in filesystem
        public bool GenerateSummarizedReport(List<Tour> toursToGenerateFrom)
        {
            return _reportFactory.GenerateSummarizedReport(toursToGenerateFrom);
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
            
            // Add all tours as json
            foreach (var tour in toursToConvert)
            {
                var serializedTour = JsonConvert.SerializeObject(tour);
                jsonArray += serializedTour + ",";
            }
            
            // Remove trailing comma
            jsonArray.TrimEnd(',');
            
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
            var tours = JsonConvert.DeserializeObject<List<Tour>>(jsonString);

            return tours!;
        }
        
        /// <summary>
        /// Creates a json file from a provided json string
        /// </summary>
        /// <param name="fileName">Filename without file-extension</param>
        /// <param name="jsonString">A valid json string</param>
        /// <returns>True if successful, false if not</returns>
        private bool CreateJsonFile(string fileName, string jsonString)
        {
            var absolutePath = Path.Combine(AppConfig.Instance.OutputDirectory, fileName + ".json");
            
            // Create the directory if it does not already exist
            var directoryPath = Path.GetDirectoryName(absolutePath);
            Directory.CreateDirectory(directoryPath!);

            // If File does not exist, create it, otherwise overwrite it
            using var writer = File.CreateText(absolutePath);

            writer.WriteLine(jsonString);
            
            writer.Close();

            return true;
        }

        /// <summary>
        /// Reads the contents of a .json file
        /// </summary>
        /// <param name="absoluteLocationOfFile">Absolute location of file, f.ex. "C:\\Users\\Samuel\\Downloads\\importMe.json"</param>
        /// <returns>Content of .json file as string</returns>
        private string ReadJsonFile(string absoluteLocationOfFile)
        {
            if (File.Exists(absoluteLocationOfFile))
            {
                return File.ReadAllText(absoluteLocationOfFile);
            }
            
            // TODO: Better error handling
            return "";
        }

        /// <summary>
        /// Delete all tours from the database
        /// </summary>
        /// <returns>True if successful, false if not</returns>
        private bool DeleteAllTours()
        {
            var allTours = _tourDao.GetTours();

            foreach (var tour in allTours)
            {
                if (!_tourDao.Delete(tour)) return false;
            }

            return true;
        }
    }
}
