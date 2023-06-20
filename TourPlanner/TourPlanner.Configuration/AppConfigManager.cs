using System.Configuration;
using TourPlanner.Logging;

namespace TourPlanner.Configuration;

public static class AppConfigManager
{
    public static AppConfigSettings Settings { get; } = new ();

    static AppConfigManager()
    {
        try
        {
            // Load configuration values from App.config
            Settings.DbConnection = ConfigurationManager.AppSettings["DbConnection"]!;
            Settings.TemplateDirectory = ConfigurationManager.AppSettings["TemplateDirectory"]!;
            Settings.PictureDirectory = ConfigurationManager.AppSettings["PictureDirectory"]!;
            Settings.OutputDirectory = ConfigurationManager.AppSettings["OutputDirectory"]!;
            Settings.LogfilePath = ConfigurationManager.AppSettings["LogfilePath"]!;
            Settings.LogLayout = ConfigurationManager.AppSettings["LogLayout"]!;
            Settings.MapQuestApiKey = ConfigurationManager.AppSettings["MapQuestApiKey"]!;
        }
        catch (NullReferenceException ex)
        {
            AppLogger.ThrowFatal("App.config error: Setting not found!", ex);
        }
        catch (Exception ex)
        {
            AppLogger.ThrowFatal("App.config error: (Probably) Syntax invalid!", ex);
        }
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
        configuration.AppSettings.Settings["TemplateDirectory"].Value = Settings.TemplateDirectory;
        configuration.AppSettings.Settings["PictureDirectory"].Value = Settings.PictureDirectory;
        configuration.AppSettings.Settings["OutputDirectory"].Value = Settings.OutputDirectory;
        configuration.AppSettings.Settings["LogfilePath"].Value = Settings.LogfilePath;
        configuration.AppSettings.Settings["LogLayout"].Value = Settings.LogLayout;
        configuration.AppSettings.Settings["MapQuestApiKey"].Value = Settings.MapQuestApiKey;

        configuration.Save(ConfigurationSaveMode.Full, true); 
        
        ConfigurationManager.RefreshSection(configuration.AppSettings.SectionInformation.Name);
    }
}
