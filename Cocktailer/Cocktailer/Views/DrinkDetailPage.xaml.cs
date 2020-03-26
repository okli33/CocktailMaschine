using Cocktailer.Models.Entries;
using Cocktailer.Services;
using Cocktailer.ViewModels;
using System;
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
            BindingContext = new DrinkDetailViewModel(DependencyService.Get<INavService>());
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