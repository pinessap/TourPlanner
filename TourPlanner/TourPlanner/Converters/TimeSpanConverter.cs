using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TourPlanner.Converters
{
    public class TimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is TimeSpan timeSpan)
                {
                    return timeSpan.ToString();
                }

            }
            catch (Exception ex)
            {
                return TimeSpan.Zero.ToString();

            }
            return TimeSpan.Zero.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is string timeString)
                {
                    return TimeSpan.Parse(timeString);
                }

            }
            catch (Exception ex)
            {
                return TimeSpan.Zero;
            }
            
            return TimeSpan.Zero;
        }
        
    }
}
