namespace TourPlanner.Configuration;

public class AppConfigSettings
{
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
}
