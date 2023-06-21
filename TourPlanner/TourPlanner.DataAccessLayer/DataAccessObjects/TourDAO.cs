using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TourPlanner.Logging;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer.DataAccessObjects
{
    public class TourDao
    {
        private readonly IDataAccess _databaseAccess;
        private readonly IDataAccess _filesystemAccess;

        public TourDao()
        {
            _databaseAccess = Database.Instance; // Database is a singleton to ensure only a single connection exists
            _filesystemAccess = new FileSystem();
        }
        
        public List<Tour> GetTours()
        {
            try
            {
                return _databaseAccess.GetTours();
            }
            catch (NotImplementedException ex)
            {
                AppLogger.ThrowFatal("GetTours DAO error:" , ex);
                return null!;
            }
        }

        public void Add(Tour tourToAdd)
        {
            try
            {
                _databaseAccess.Add(tourToAdd);
            }
            catch (NotImplementedException ex)
            {
                AppLogger.ThrowFatal("ADD DAO error:", ex);
            }
        }

        public void Delete(Tour tourToDelete)
        {
            try
            {
                _databaseAccess.Delete(tourToDelete);
            }
            catch (NotImplementedException ex)
            {
                AppLogger.ThrowFatal("DELETE DAO error:", ex);
            }
        }

        public void Modify(Tour modifiedTour)
        {
            try
            {
                _databaseAccess.Modify(modifiedTour);
            }
            catch (NotImplementedException ex)
            {
                AppLogger.ThrowFatal("MODIFY DAO error:", ex);
            }
        }
        
        public List<string> GetModifiedProperties(Tour modifiedTour)
        {
            try
            {
                return _databaseAccess.GetModifiedProperties(modifiedTour).ToList();
            }
            catch (NotImplementedException ex)
            {
                AppLogger.ThrowFatal("GetModifiedProperties DAO error:", ex);
                return null!;
            }
        }
        
        public void SaveToFile(string absoluteFilePath, string fileContent, bool manualUserSave = false)
        {
            try
            {
                _filesystemAccess.SaveToFile(absoluteFilePath, fileContent, manualUserSave);
            }
            catch (NotImplementedException ex)
            {
                AppLogger.ThrowFatal("SaveToFile DAO error:", ex);
            }
        }
        
        public void SaveToFile(string absoluteFilePath, Stream fileContent, bool manualUserSave = false)
        {
            try
            {
                _filesystemAccess.SaveToFile(absoluteFilePath, fileContent, manualUserSave);
            }
            catch (NotImplementedException ex)
            {
                AppLogger.ThrowFatal("SaveToFile DAO error:", ex);
            }
        }

        public string ReadFromFile(string absoluteFilePath, bool manualUserSelectWhenNotFound = false)
        {
            try 
            {
                return _filesystemAccess.ReadFromFile(absoluteFilePath, manualUserSelectWhenNotFound);
            }
            catch (NotImplementedException ex)
            {
                AppLogger.ThrowFatal("ReadFromFile DAO error:", ex);
                return null!;
            }
        }
    }
}
