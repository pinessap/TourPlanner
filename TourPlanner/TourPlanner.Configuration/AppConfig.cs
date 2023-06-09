using System;
using System.IO;
using Newtonsoft.Json;

namespace TourPlanner.Configuration;

public class AppConfig
{
    private static readonly Lazy<AppConfig> Lazy = new(() => new AppConfig(true));
    public static AppConfig Instance => Lazy.Value;
    
    /// <summary>
    /// DBConnection string consisting of "Host = ...; Database = ...; Username = ...; Password = ..."
    /// </summary>
    public string DbConnection { get; set; }
    
    /// <summary>
    /// The absolute path to the directory where the program files lie (aka the .sln file)
    /// </summary>
    public string ProgramDirectory { get; set; }
    
    /// <summary>
    /// The directory where all output is saved
    /// </summary>
    public string OutputDirectory { get; set; }

    /// <summary>
    /// The relative path to the config.json file, relative to where the .exe file of this program gets executed from (= AppDomain.CurrentDomain.BaseDirectory)
    /// </summary>
    private string RelativePathToConfig = "..\\..\\..\\..\\TourPlanner.Configuration\\config.json";

    /// <summary>
    /// When AppConfig gets first accessed, it reads the config.json file and sets its properties using this constructor
    /// </summary>
    /// <param name="overloadHacks">An unused parameter that is only here so another overloaded constructor can exist that does not call ReadFromJson</param>
    private AppConfig(bool overloadHacks)
    {
        ReadFromJson();
    }

    /// <summary>
    /// A parameterless constructor must exist due to the JsonConvert.DeserializeObject function call in the ReadFromJson function.
    /// If not, the DeserializeObject function instantiates another AppConfig class which calls the ReadFromJson function again, creating an infinite loop!
    /// </summary>
    private AppConfig() { }
    
    /// <summary>
    /// Saves changes in this AppConfig instance to the config.json file
    /// </summary>
    public void SaveChangedValues()
    {
        WriteToJson();
    }
    
    /// <summary>
    /// Reads contents from config.json and saves them into AppConfig properties
    /// </summary>
    private void ReadFromJson()
    {
        var absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, RelativePathToConfig);

        if (File.Exists(absolutePath))
        {
            var jsonString = File.ReadAllText(absolutePath);

            try
            {
                var configObject = JsonConvert.DeserializeObject<AppConfig>(jsonString)!;

                // Copy values from json to this object
                DbConnection = configObject.DbConnection;
                ProgramDirectory = configObject.ProgramDirectory;
                OutputDirectory = configObject.OutputDirectory;
            }
            catch (Exception ex)
            {
                // TODO: Throw config parsing error (aka config is invalid)
            }
        }
        else
        {
            // TODO: Throw config reading error (aka config doesnt exist)
        }
    }

    /// <summary>
    /// Overwrites contents from config.json file with AppConfig properties
    /// </summary>
    private void WriteToJson()
    {
        var absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, RelativePathToConfig);
        
        var jsonString = JsonConvert.SerializeObject(this);
        
        // If File does not exist, create it, otherwise overwrite it
        using var writer = File.CreateText(absolutePath);

        writer.WriteLine(jsonString);
        
        writer.Close();
    }
}