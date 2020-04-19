using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Cocktailer.Converters
{
    public class IntArrayStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int[]))
                return "No int";
            var arr = (int[])value;
            return arr[0].ToString() + ", " + arr[1].ToString() ;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string))
                return new int[] { -1, -1 };
            try
            {
                var numInString = ((string)value).Split(',').Select(x => x.Trim()).ToList();
                return new int[] { int.Parse(numInString[0]), int.Parse(numInString[1])  };
            }
            catch
            {
                return new int[] { -1, -1 };
            }
            
        }
    }
}
