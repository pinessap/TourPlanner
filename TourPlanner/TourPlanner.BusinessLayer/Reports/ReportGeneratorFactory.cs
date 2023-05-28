using System;

namespace TourPlanner.BusinessLayer.Reports;

public static class ReportGeneratorFactory
{
    private static readonly Lazy<IReportGenerator> Lazy = new(() => new ReportGenerator());
    public static IReportGenerator Instance => Lazy.Value;
}