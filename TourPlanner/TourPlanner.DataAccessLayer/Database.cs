using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TourPlanner.Models;
using Npgsql;
using TourPlanner.DataAccessLayer.Data;
using TourPlanner.Logging;

namespace TourPlanner.DataAccessLayer
{
    internal sealed class Database : IDataAccess
    {
        private static readonly Lazy<Database> Lazy = new(() => new Database());
        public static Database Instance => Lazy.Value;

        /// <summary>
        /// The context containing our entire DB
        /// </summary>
        private readonly TourPlannerContext _context;

        private Database()
        {
            _context = new TourPlannerContext();
        }

        ~Database()
        {
            _context.Dispose();
        }
        
        public List<Tour> GetTours()
        {
            // Get tours via linq syntax
            // Note the Include statement here, that makes sure the TourLogs are loaded too
            // If this is removed, the TourLogs list is not populated and remains empty!
            var tours = _context.Tours.Include(tour => tour.Logs).ToList();

            // Convert IQueryable<Tour> to List<Tour>
            List<Tour> allTours = new();
            allTours.AddRange(tours);

            return allTours;
        }
        
        public void Add(Tour tourToAdd)
        {
            try
            {
                _context.Add(tourToAdd);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                AppLogger.ThrowError("ADD error:", ex);
            }
        }
        
        public void Delete(Tour tourToDelete)
        {
            try
            {
                _context.Remove(tourToDelete);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                AppLogger.ThrowError("DELETE error:", ex);
            }
        }

        public void Modify(Tour modifiedTour)
        {
            try
            {
                _context.Update(modifiedTour);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                AppLogger.ThrowError("MODIFY error:", ex);
            }
        }

        public IEnumerable<string> GetModifiedProperties(Tour modifiedTour)
        {
            // Get the EntityEntry for the entity
            var entityEntry = _context.Entry(modifiedTour);
                
            // Get the modified properties
            var modifiedProperties = entityEntry.Properties
                .Where(p => p.IsModified)
                .Select(p => p.Metadata.Name);

            return modifiedProperties;
        }

        public void SaveToFile(string absoluteFilePath, string fileContent, bool manualUserSave = false)
        {
            throw new NotImplementedException();
        }

        public void SaveToFile(string absoluteFilePath, Stream fileContent, bool manualUserSave = false)
        {
            throw new NotImplementedException();
        }

        public string ReadFromFile(string absoluteFilePath, bool manualUserSelectWhenNotFound = false)
        {
            throw new NotImplementedException();
        }
    }
}