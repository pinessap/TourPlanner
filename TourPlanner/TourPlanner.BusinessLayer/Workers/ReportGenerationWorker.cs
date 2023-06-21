using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TourPlanner.Configuration;
using TourPlanner.Logging;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer.Workers;

public class ReportGenerationWorker : IWorker
{
        public void GenerateSingleReport(Tour tourToGenerateReportFrom)
        {
            AppLogger.Info("Generating SingleReport of \"" + tourToGenerateReportFrom.Name + "\"");
            
            var pathToTourTemplate = Path.Combine(AppConfigManager.Settings.TemplateDirectory, "tourTemplate.html");
            var pathToTourLogTemplate = Path.Combine(AppConfigManager.Settings.TemplateDirectory, "tourLogTemplate.html");
            
            var htmlSourceTour = TourDao.ReadFromFile(pathToTourTemplate);
            var htmlSourceTourLog = TourDao.ReadFromFile(pathToTourLogTemplate);
            
            var htmlWithTourData = ReplaceHtmlPlaceholdersWithTourData(htmlSourceTour, tourToGenerateReportFrom);
            var htmlWithAllData = htmlWithTourData + CreateTourLogsHtml(tourToGenerateReportFrom.Logs, htmlSourceTourLog);
            
            var absolutePath = Path.Combine(AppConfigManager.Settings.OutputDirectory, tourToGenerateReportFrom.Name + "_Report.pdf");

            TourDao.SaveToFile(absolutePath, htmlWithAllData, true);
        }
        
        public void GenerateSummarizedReport(List<Tour> toursToGenerateFrom)
        {
            AppLogger.Info("Generating SummarizedReport of " + toursToGenerateFrom.Count + " tours");

            var pathToTourSummaryHeaderTemplate = Path.Combine(AppConfigManager.Settings.TemplateDirectory, "tourSummaryHeaderTemplate.html");
            var pathToTourSummaryEntryTemplate = Path.Combine(AppConfigManager.Settings.TemplateDirectory, "tourSummaryEntryTemplate.html");

            var htmlSourceHeaderTemplate = TourDao.ReadFromFile(pathToTourSummaryHeaderTemplate);
            var htmlSourceEntryTemplate = TourDao.ReadFromFile(pathToTourSummaryEntryTemplate);
            
            var tourDataHtml = CreateSummarizedToursHtml(htmlSourceEntryTemplate, toursToGenerateFrom);
            var fullHtml = htmlSourceHeaderTemplate + tourDataHtml;
        
            var absolutePath = Path.Combine(AppConfigManager.Settings.OutputDirectory, "Summarized_Report.pdf");

            TourDao.SaveToFile(absolutePath, fullHtml, true);
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