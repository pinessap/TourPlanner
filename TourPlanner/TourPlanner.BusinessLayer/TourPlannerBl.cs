using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TourPlanner.BusinessLayer.Workers;
using TourPlanner.Configuration;
using TourPlanner.DataAccessLayer.DataAccessObjects;
using TourPlanner.Logging;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    internal class TourPlannerBl : ITourPlannerBl
    {
        private readonly TourWorker _tourWorker = new ();
        private readonly JsonDataWorker _jsonDataWorker = new ();
        private readonly ReportGenerationWorker _reportGenerationWorker = new ();

        public List<Tour> GetTours()
        {
            return _tourWorker.GetTours();
        }
        
        public List<Tour> Search(string searchValue, bool caseSensitive = false)
        {
            return _tourWorker.Search(searchValue, caseSensitive);
        }
        
        public async Task Add(Tour tourToAdd)
        {
            await _tourWorker.Add(tourToAdd);
        }
        
        public void Delete(Tour tourToDelete)
        {
            _tourWorker.Delete(tourToDelete);

        }
        
        public async Task Modify(Tour modifiedTour)
        {
            await _tourWorker.Modify(modifiedTour);
        }

        public void Export(List<Tour> toursToExport)
        {
            _jsonDataWorker.Export(toursToExport);
        }

        public void ImportOverride()
        {
            _jsonDataWorker.ImportOverride();
        }

        public void ImportAppend()
        {
            _jsonDataWorker.ImportAppend();
        }

        public void GenerateSingleReport(Tour tourToGenerateReportFrom)
        {
            _reportGenerationWorker.GenerateSingleReport(tourToGenerateReportFrom);
        }
        
        public void GenerateSummarizedReport(List<Tour> toursToGenerateFrom)
        {
            _reportGenerationWorker.GenerateSummarizedReport(toursToGenerateFrom);
        }
    }
}
