using Cocktailer.Models.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cocktailer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditDrinkPage : ContentPage
    {
        public EditDrinkPage(DrinkEntry entry)
        {
            InitializeComponent();
            brand.Text = entry.Brand;
            name.Text = entry.Name;
            percentage.Text = entry.Percentage.ToString();
        }
    }
}