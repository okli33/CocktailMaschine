
using Cocktailer.Services;
using Cocktailer.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cocktailer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewDrinkPage : ContentPage
    {
        NewDrinkViewModel ViewModel => BindingContext as NewDrinkViewModel;
        public NewDrinkPage()
        {
            InitializeComponent();
            BindingContextChanged += Page_BindingContextChanged;
            BindingContext = new NewDrinkViewModel(DependencyService.Get<INavService>());
        }

        void Page_BindingContextChanged(object sender, EventArgs e)
        {
            ViewModel.ErrorsChanged += ViewModel_ErrorsChanged;
        }

        void ViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            var propHasErrors = (ViewModel.GetErrors(e.PropertyName) as List<string>)?.Any() == true;
            switch (e.PropertyName)
            {
                case nameof(ViewModel.Name):
                    name.LabelColor = propHasErrors ? Color.Red : Color.Black;
                    break;
                case nameof(ViewModel.Percentage):
                    percentage.LabelColor = propHasErrors ? Color.Red : Color.Black;
                    break;
                default:
                    break;
            }
        }
    }
}