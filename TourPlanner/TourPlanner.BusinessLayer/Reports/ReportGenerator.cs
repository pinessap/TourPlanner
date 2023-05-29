using System;
using System.Collections.Generic;
using System.IO;
using iText.Html2pdf;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer.Reports;

// TODO: Replace all hardcoded relative paths in this file with one from a config file
public class ReportGenerator : IReportGenerator
{
    public bool GenerateSingleReport(Tour tourToGenerateReportFrom)
    {
        var htmlSource = ReadHtmlFile("tourTemplate");
        var htmlWithTourData = ReplaceHtmlPlaceholdersWithTourData(htmlSource, tourToGenerateReportFrom);
        var htmlWithAllData = htmlWithTourData + CreateTourLogsHtml(tourToGenerateReportFrom.Logs);
        
        SavePdfFromHtml(htmlWithAllData, "SingleReport");

        return true;
    }

    public bool GenerateSummarizedReport(List<Tour> toursToGenerateFrom)
    {
        var htmlHeader = ReadHtmlFile("tourSummaryHeaderTemplate");
        var tourDataHtml = CreateSummarizedToursHtml(toursToGenerateFrom);
        var fullHtml = htmlHeader + tourDataHtml;
        
        SavePdfFromHtml(fullHtml, "SummarizedReport");

        return true;
    }

    /// <summary>
    /// Reads contents from an html file
    /// </summary>
    /// <param name="filename">The filename of the file to read without file-extension</param>
    /// <returns>Html string</returns>
    private string ReadHtmlFile(string filename)
    {
        // TODO: Replace relative path with information from config file
        var relativePath = "..\\..\\..\\..\\TourPlanner.BusinessLayer\\Reports\\" + filename + ".html";
        var absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

        if (File.Exists(absolutePath))
        {
            return File.ReadAllText(absolutePath);
        }
            
        // TODO: Better error handling
        return "";
    }

    /// <summary>
    /// Replaces all {{PlaceHolderName}} placeholders inside html string
    /// </summary>
    /// <param name="htmlStringWithPlaceholders">Html string containing placeholders in the following format: {{TourModelPropertyName}}</param>
    /// <param name="tourWithData">Tour which contains the data-values we want to add to the html.</param>
    /// <returns>Html string with replaced placeholders</returns>
    private string ReplaceHtmlPlaceholdersWithTourData(string htmlStringWithPlaceholders, Tour tourWithData)
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

    /// <summary>
    /// Creates an html string with a given list of tourLogs
    /// </summary>
    /// <returns>Html string with all tour log information</returns>
    private string CreateTourLogsHtml(List<TourLog> tourLogs)
    {
        var tourLogTemplateHtml = ReadHtmlFile("tourLogTemplate");
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

    private string CreateSummarizedToursHtml(List<Tour> tours)
    {
        var allToursHtml = "";
        var tourHtmlWithPlaceholders = ReadHtmlFile("tourSummaryEntryTemplate");

        foreach (var tour in tours)
        {
            allToursHtml += ReplaceHtmlPlaceholdersWithTourData(tourHtmlWithPlaceholders, tour);
        }

        return allToursHtml;
    }

    /// <summary>
    /// Saves html string as pdf file
    /// </summary>
    /// <param name="htmlContent">Html string which gets converted to pdf format</param>
    /// <param name="fileName">Name of pdf file without file-extension</param>
    private void SavePdfFromHtml(string htmlContent, string fileName)
    {
        // TODO: Replace relative path with information from config file
        var relativePath = "Reports\\" + fileName + ".pdf";
        var absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            
        // Create the directory if it does not already exist
        var directoryPath = Path.GetDirectoryName(absolutePath);
        Directory.CreateDirectory(directoryPath!);
        
        using var pdfDest = File.Open(absolutePath, FileMode.Create);
        var converterProperties = new ConverterProperties();
        converterProperties.SetBaseUri("..\\..\\..\\..\\TourPlanner.BusinessLayer\\Reports\\");
        HtmlConverter.ConvertToPdf(htmlContent, pdfDest, converterProperties);
    }
}