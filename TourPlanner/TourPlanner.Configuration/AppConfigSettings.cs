namespace TourPlanner.Configuration;

public class AppConfigSettings
{
     /// <summary>
     /// DBConnection string consisting of "Host = ...; Database = ...; Username = ...; Password = ..."
     /// </summary>
     public string DbConnection { get; set; }
     
     /// <summary>
     /// The absolute path to the directory where the report .html template files lie
     /// </summary>
     public string TemplateDirectory { get; set; }
     
     /// <summary>
     /// The absolute path to the directory where all application pictures are saved
     /// </summary>
     public string PictureDirectory { get; set; }
     
     /// <summary>
     /// The directory where all output is saved
     /// </summary>
     public string OutputDirectory { get; set; }
     
     /// <summary>
     /// The path to the logfile
     /// <example>C:\Downloads\main.log</example>
     /// </summary>
     public string LogfilePath { get; set; }
     
     /// <summary>
     /// The layout of log messages. Consult Log4Net documentation for syntax.
     /// </summary>
     public string LogLayout { get; set; }
}
