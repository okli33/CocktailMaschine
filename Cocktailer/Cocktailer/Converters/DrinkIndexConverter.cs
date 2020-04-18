using Cocktailer.Models.Entries;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Cocktailer.Converters
{
    public class DrinkIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DrinkEntry)
                return ConvertDrinkToIndex((DrinkEntry)value);
            if (value is int)
                return ConvertIndexToDrinkEntry((int)value);
            return null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DrinkEntry)
                return ConvertDrinkToIndex((DrinkEntry)value);
            if (value is int)
                return ConvertIndexToDrinkEntry((int)value);
            return null;
        }

        private int ConvertDrinkToIndex(DrinkEntry value)
        {
            var entry = new DrinkEntry() { Brand = value.Brand, Name = value.Name, Percentage = value.Percentage };
            int index = DrinkList.AvailableDrinks.IndexOf(entry);
            return index;
        }
        private DrinkEntry ConvertIndexToDrinkEntry(int value)
        {
            DrinkEntry entry = DrinkList.AvailableDrinks[value];
            return entry;
        }
    }
}
