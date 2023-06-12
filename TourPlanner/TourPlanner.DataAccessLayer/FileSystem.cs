using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using iText.Html2pdf;
using Microsoft.Win32;
using TourPlanner.Configuration;
using TourPlanner.Logging;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    internal class FileSystem : IDataAccess //for import and export of tour data (file format of your choice) ?
    {
        public List<Tour> GetTours()
        {
            throw new NotImplementedException();
        }

        public void Add(Tour tourToAdd)
        {
            throw new NotImplementedException();
        }

        public void Delete(Tour tourToDelete)
        {
            throw new NotImplementedException();
        }

        public void Modify(Tour modifiedTour)
        {
            throw new NotImplementedException();
        }

        public void SaveToFile(string absoluteFilePath, string fileContent, bool manualUserSave = false)
        {
            try
            {
                var directoryPath = Path.GetDirectoryName(absoluteFilePath);

                // Create the directory if it does not already exist
                Directory.CreateDirectory(directoryPath!);

                // If manual saving is enabled, open a dialogbox and let user save file himself
                if (manualUserSave)
                {
                    SaveFile(absoluteFilePath, fileContent, SaveFileWithExplorer(absoluteFilePath));
                }
                else
                {
                    SaveFile(absoluteFilePath, fileContent);
                }
            }
            catch (Exception ex)
            {
                AppLogger.ThrowError("SaveToFile error:", ex);
            }
        }

        public string ReadFromFile(string absoluteFilePath, bool manualUserSelectWhenNotFound = false)
        {
            if (File.Exists(absoluteFilePath))
            {
                return File.ReadAllText(absoluteFilePath);
            }
            
            // If user should not select file manually, throw error when not found
            if(!manualUserSelectWhenNotFound) AppLogger.ThrowError("ReadFromFile error: File \"" + absoluteFilePath + "\" not found!", new FileNotFoundException());

            // Otherwise, open dialogbox and let user select file himself
            return SelectFileFromExplorer(absoluteFilePath);
        }

        /// <summary>
        /// Opens explorer and lets user save file manually
        /// </summary>
        /// <returns>Stream pointing to file user wants to create</returns>
        private FileStream? SaveFileWithExplorer(string absoluteFilePath)
        {
            AppLogger.Info("Opening file explorer for manual save");
            
            var directoryPath = Path.GetDirectoryName(absoluteFilePath);
            var fileName = Path.GetFileName(absoluteFilePath);
            var fileExtension = Path.GetExtension(absoluteFilePath);
            
            var saveFileDialog = new SaveFileDialog
            {
                Filter = $"Files (*{fileExtension})|*{fileExtension}",
                FileName = fileName,
                InitialDirectory = directoryPath
            };

            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                return (FileStream)saveFileDialog.OpenFile();
            }

            AppLogger.Warn("Empty filename when saving. Skipping manual save.");
            return null;
        }
        
        /// <summary>
        /// Opens explorer and lets user select file to read manually
        /// </summary>
        /// <returns>Contents of read file</returns>
        private string SelectFileFromExplorer(string absoluteFilePath)
        {
            var directoryPath = Path.GetDirectoryName(absoluteFilePath);
            var fileExtension = Path.GetExtension(absoluteFilePath);
            
            var openFileDialog = new OpenFileDialog
            {
                Filter = $"Files (*{fileExtension})|*{fileExtension}",
                InitialDirectory = directoryPath
            };

            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != "")
            {
                using var fs = (FileStream)openFileDialog.OpenFile();

                // Read the file contents using the FileStream
                var fileBytes = new byte[fs.Length];
                fs.Read(fileBytes, 0, (int)fs.Length);

                // Convert the file bytes to a string assuming it's a text file
                var fileContents = Encoding.UTF8.GetString(fileBytes);

                AppLogger.Info("\"" + openFileDialog.FileName + "\" manually read.");

                return fileContents;
            }
            else
            {
                AppLogger.ThrowError("Empty filename when reading!", new FileNotFoundException());
                return null!;
            }
        }

        /// <summary>
        /// Saves the fileContent into the given FileStream.
        /// </summary>
        /// <param name="absoluteFilePath">Absolute path to file, used as location if no FileStream is provided</param>
        /// <param name="fileContent">Content of file to save</param>
        /// <param name="fs">A fileStream that points to the file that gets saved. If null, absoluteFilePath is used as fileLocation.</param>
        private void SaveFile(string absoluteFilePath, string fileContent, FileStream? fs = null)
        {
            fs ??= new FileStream(absoluteFilePath, FileMode.Create);

            // If file to save is a pdf, a special conversion function to save needs to be called
            if (Path.GetExtension(absoluteFilePath) == ".pdf")
            {
                var converterProperties = new ConverterProperties();
                converterProperties.SetBaseUri(AppConfigManager.Settings.PictureDirectory);
                HtmlConverter.ConvertToPdf(fileContent, fs, converterProperties);
            }
            // Otherwise a normal file with text content gets created
            else
            {
                using var writer = new StreamWriter(fs);
                writer.Write(fileContent);
                writer.Close();
            }
            
            AppLogger.Info("\"" + fs.Name + "\" saved.");
            
            fs.Close();
        }
    }
}
