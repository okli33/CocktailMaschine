using System;
using System.Globalization;
using Xamarin.Forms;

namespace Cocktailer.Converters
{
    public class FloatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "0";
            float numAsFloat = (float)value;
            return numAsFloat.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;
            if (string.IsNullOrEmpty(strValue))
                strValue = "0";
            decimal resultFloat;
            if (decimal.TryParse(strValue, out resultFloat))
            {
                return resultFloat;
            }
            return 0;
        }
    }
}
