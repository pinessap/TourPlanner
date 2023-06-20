using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TourPlanner.Converters
{
    class ZeroDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float distance)
            {
                if (distance != 0)
                {
                    // Check if the decimal portion is zero
                    if (Math.Abs(distance % 1) > float.Epsilon)
                    {
                        // Display decimal points if the decimal portion is not zero
                        return string.Format("{0:N2} km", distance);
                    }
                    else
                    {
                        // Do not display decimal points if the decimal portion is zero
                        return $"{distance:N0} km";
                    }
                }
                else
                {
                    // Do not display decimal points if the distance is zero
                    return $"{distance:N0}";
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
