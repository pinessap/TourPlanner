using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TourPlanner.Configuration;
using TourPlanner.Logging;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer.Workers;

public class JsonDataWorker : IWorker
{
    public void Export(List<Tour> toursToExport)
    {
        var absolutePath = Path.Combine(AppConfigManager.Settings.OutputDirectory, "export.json");
        AppLogger.Info("Try Exporting "+ toursToExport.Count + " tours to \"" + absolutePath + "\"");
        
        var jsonArray = ConvertToursToJson(toursToExport);

        TourDao.SaveToFile(absolutePath, jsonArray, true);
        
        AppLogger.Info("Export to \"" + absolutePath + "\" successful");
    }

    public void ImportOverride()
    {
        var defaultAbsoluteLocationOfFile = Path.Combine(AppConfigManager.Settings.OutputDirectory, "importData.json");
        
        AppLogger.Info("Try OverrideImport from \"" + defaultAbsoluteLocationOfFile + "\"");
        
        var jsonString = TourDao.ReadFromFile(defaultAbsoluteLocationOfFile, true);

        var toursToOverride = ConvertJsonToTours(jsonString);
        
        // Delete all tours from the database
        DeleteAllTours();
        
        // Add new tours back
        foreach (var tour in toursToOverride)
        {
            TourDao.Add(tour);
        }
        
        AppLogger.Info("OverrideImport successful");
    }

    public void ImportAppend()
    {
        var defaultAbsoluteLocationOfFile = Path.Combine(AppConfigManager.Settings.OutputDirectory, "importData.json");

        AppLogger.Info("Try AppendImport from \"" + defaultAbsoluteLocationOfFile + "\"");

        var jsonString = TourDao.ReadFromFile(defaultAbsoluteLocationOfFile, true);

        var toursToAppend = ConvertJsonToTours(jsonString);
        
        foreach (var tour in toursToAppend)
        {
            // Reset tour-ID to make sure ef treats tour as new entry
            tour.TourId = 0;
            
            // Do the same for all tourLogs
            foreach (var log in tour.Logs) { log.TourLogId = 0;}

            TourDao.Add(tour);
        }
        
        AppLogger.Info("AppendImport successful");
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

        var allTours = TourDao.GetTours();

        foreach (var tour in allTours)
        {
            TourDao.Delete(tour);
        }
    }
}