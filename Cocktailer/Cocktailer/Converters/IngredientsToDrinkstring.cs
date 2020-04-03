using Cocktailer.Models.DataManagement;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Cocktailer.Converters
{
    public class IngredientsToDrinkstring : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is List<Ingredient>))
                    return "";
                var ings = (List<Ingredient>)value;
                if (ings.Count >= 3)
                    return string.Join(", ", ings.Select(x => x.Drink.Brand + " " + x.Drink.Name).Take(3)) + ", ...";
                else if (ings.Count == 2)
                    return string.Join(", ", ings.Select(x => x.Drink.Brand + " " + x.Drink.Name).Take(2));

                return ings.Select(x => x.Drink.Brand + " " + x.Drink.Name).First();
            }
            catch
            {
                return "";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
