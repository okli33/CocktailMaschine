using Cocktailer.ViewModels.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cocktailer.Views.Configurations
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigurationsPage : ContentPage
    {
        ConfigurationsViewModel ViewModel => BindingContext as ConfigurationsViewModel;
        public ConfigurationsPage()
        {
            InitializeComponent();
        }
    }
}