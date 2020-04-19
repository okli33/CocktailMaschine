using Cocktailer.ViewModels;
using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cocktailer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrinksPage : ContentPage
    {
        DrinksViewModel ViewModel => BindingContext as DrinksViewModel;
        public DrinksPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel != null)
                ViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (ViewModel != null)
                ViewModel.PropertyChanged -= OnViewModelPropertyChanged;
        }

        protected void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DrinkDetailViewModel.Entry))
            {
                //Maybe add update function
            }
        }
    }
}