using log4net;
using log4net.Config;

namespace TourPlanner.Logging;

public static class AppLogger
{
    private static readonly ILog Log = LogManager.GetLogger(typeof(AppLogger));

    /// <summary>
    /// Configuration should not need be called manually, but it doesn't work otherwise so it is called here
    /// </summary>
    static AppLogger()
    {
        XmlConfigurator.Configure();
    }

    /// <summary>
    /// Logs a debugging message. Use during development, does not get logged to file.
    /// </summary>
    public static void Debug(string message)
    {
        Log.Debug(message);
    }

    /// <summary>
    /// Logs an info message. Use to log status information.
    /// </summary>
    public static void Info(string message)
    {
        Log.Info(message);
    }
    
    /// <summary>
    /// Logs a warning message. Use when an error might happen in the future.
    /// </summary>
    public static void Warn(string message)
    {
        Log.Warn(message);
    }

    /// <summary>
    /// Logs an error message. Does NOT throw an error. Use when error happens, but application can continue running.
    /// </summary>
    public static void Error(string message)
    {
        Log.Error(message);
    }
    
    /// <summary>
    /// Logs an error message and throws the provided exception. Use when error happens, but application can continue running.
    /// </summary>
    public static void ThrowError(string message, Exception ex)
    {
        Log.Error(message, ex);
        throw ex;
    }
    
    /// <summary>
    /// Logs a fatal error message. Does NOT throw an error. Use when error crashes application.
    /// </summary>
    public static void Fatal(string message)
    {
        Log.Fatal(message);
    }
    
    /// <summary>
    /// Logs a fatal error message and throws the provided exception. Use when error crashes application.
    /// </summary>
    public static void ThrowFatal(string message, Exception ex)
    {
        Log.Fatal(message, ex);
        throw ex;
    }
}