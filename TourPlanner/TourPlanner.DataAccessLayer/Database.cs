using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

        public bool Modify(Tour modifiedTour)
        {
            try
            {
                _context.Update(modifiedTour);
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