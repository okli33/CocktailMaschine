using Cocktailer.Models.Entries;
using System;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace Cocktailer.Converters
{
    public class DrinkStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DrinkEntry))
                return "";
            var drink = (DrinkEntry)value;
            return drink.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string))
                return null;
            try
            {
                var strlist = ((string)value).Split('/').ToList();
                string brand = strlist[0];
                strlist = strlist[1].Replace("%", "").Split(',').ToList();
                string name = strlist[0];
                string perc = strlist[1];
                return new DrinkEntry()
                {
                    Name = name,
                    Brand = brand,
                    Percentage = double.Parse(perc)
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
