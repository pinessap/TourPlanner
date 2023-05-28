using TourPlanner.Models;

namespace TourPlanner.BusinessLayer.Reports;

public interface IReportGenerator
{
    /// <summary>
    /// Generates a PDF report from a given tour
    /// </summary>
    /// <param name="tourToGenerateReportFrom">Tour used to generate the report from</param>
    /// <returns></returns>
    bool GenerateSingleReport(Tour tourToGenerateReportFrom);
}