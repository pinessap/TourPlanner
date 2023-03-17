using System;
using System.Collections.Generic;
using System.Linq;
using TourPlanner.Models;
using Npgsql;
using TourPlanner.DataAccessLayer.Data;

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
        
        /// <summary>
        /// Get all tours from the database
        /// </summary>
        public List<Tour> GetTours()
        {
            // Get tours via linq syntax
            var tours = from tour in _context.Tours select tour;
            
            // This does the exact same as the above, just with another syntax
            // var tours = _context.Products.Where(p => true);
            
            // Convert IQueryable<Tour> to List<Tour>
            List<Tour> allTours = new();
            allTours.AddRange(tours);

            return allTours;
        }
        
        /// <summary>
        /// Adds given tour to the database
        /// </summary>
        /// <returns>True if successful, false on an error</returns>
        public bool Add(Tour tourToAdd)
        {
            try
            {
                _context.Add(tourToAdd);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // TODO: Write this to a log file (cuz the console logs don't actually work...)
                Console.WriteLine("ADD EXCEPTION: " + ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Removes the given tour from the database
        /// </summary>
        /// <returns>True if successful, false on an error</returns>
        public bool Delete(Tour tourToDelete)
        {
            try
            {
                _context.Remove(tourToDelete);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // TODO: Write this to a log file (cuz the console logs don't actually work...)
                Console.WriteLine("DELETE EXCEPTION: " + ex.Message);
                return false;
            }

            return true;
        }
    }
}