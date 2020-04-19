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
    public partial class ConfigurationDetailPage : ContentPage
    {
        ConfigurationDetailViewModel ViewModel => BindingContext
            as ConfigurationDetailViewModel;
        public ConfigurationDetailPage()
        {
            InitializeComponent();
        }
    }
}