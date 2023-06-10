using System.Configuration;

namespace TourPlanner.Configuration;

public static class AppConfigManager
{
    public static AppConfigSettings Settings { get; } = new AppConfigSettings();

    static AppConfigManager()
    {
        // TODO: Add proper exceptions
        // Load configuration values from App.config
        Settings.DbConnection = ConfigurationManager.AppSettings["DbConnection"] ?? throw new Exception();
        Settings.ProgramDirectory = ConfigurationManager.AppSettings["ProgramDirectory"] ?? throw new Exception();
        Settings.OutputDirectory = ConfigurationManager.AppSettings["OutputDirectory"] ?? throw new Exception();
    }
    
    /// <summary>
    /// Saves settings during runtime.
    /// Be aware that this does not reflect changes in the App.config file located in TourPlanner.Configuration but rather the copy that the IDE creates on building.
    /// <example>
    /// <para>Update the DbConnection string during runtime and save it in the App.config file:</para>
    /// <code>
    /// AppConfigManager.Settings.DbConnection = "aNewConnectionString";
    /// AppConfigManager.SaveSettings();
    /// </code>
    /// </example>
    /// </summary>
    public static void SaveSettings()
    {
        var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        
        configuration.AppSettings.Settings["DbConnection"].Value = Settings.DbConnection;
        configuration.AppSettings.Settings["ProgramDirectory"].Value = Settings.ProgramDirectory;
        configuration.AppSettings.Settings["OutputDirectory"].Value = Settings.OutputDirectory;

        configuration.Save(ConfigurationSaveMode.Full, true); 
        
        ConfigurationManager.RefreshSection(configuration.AppSettings.SectionInformation.Name);
    }
}
