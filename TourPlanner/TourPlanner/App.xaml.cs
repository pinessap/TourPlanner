using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.Logging;

namespace TourPlanner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Log application start
            AppLogger.Info("------------------ START APPLICATION TRIGGERED ------------------");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            
            // Log application exit
            AppLogger.Info("------------------ CLOSE APPLICATION TRIGGERED ------------------");
        }
    }
}
