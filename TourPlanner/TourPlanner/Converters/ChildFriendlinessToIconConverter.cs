using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TourPlanner.Converters
{
    public class ChildFriendlinessToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int friendliness = 5;
            if (value != null)
            {
                friendliness = (int)value;
            }
            
            if (friendliness <= 3)
                return "\uf119;";
            else if (friendliness is <= 6 and > 3)
                return "\uf11a;";
            else if (friendliness is <= 9 and > 6)
                return "\uf118";
            else if (friendliness == 10)
                return "\uf59a";


            return "\uf11a;"; // Default value
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
    
}
