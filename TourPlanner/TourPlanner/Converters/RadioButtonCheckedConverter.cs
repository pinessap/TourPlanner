using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static TourPlanner.Models.Tour;

namespace TourPlanner.Converters
{
    public class RadioButtonCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null || !(value is TransportTypes) || !(parameter is TransportTypes))
                return false;

            return (TransportTypes)value == (TransportTypes)parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked && isChecked && parameter is TransportTypes)
            {
                return parameter;
            }

            return Binding.DoNothing;
        }
    }
}
