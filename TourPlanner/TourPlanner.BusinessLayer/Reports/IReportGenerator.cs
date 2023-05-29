using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer.Reports;

public interface IReportGenerator
{
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