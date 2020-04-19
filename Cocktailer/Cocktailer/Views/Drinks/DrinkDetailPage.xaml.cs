using Cocktailer.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cocktailer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrinkDetailPage : ContentPage
    {
        DrinkDetailViewModel ViewModel => BindingContext as DrinkDetailViewModel;
        public DrinkDetailPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel != null)
                ViewModel.PropertyChanged += OnViewModelPropertyChangedAsync;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (ViewModel != null)
                ViewModel.PropertyChanged -= OnViewModelPropertyChangedAsync;
        }
        
        protected void OnViewModelPropertyChangedAsync(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DrinkDetailViewModel.Entry))
            {
             
            }
        }
    }
}