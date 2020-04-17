using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cocktailer.Services
{
    public class AlertMessageService : IAlertMessageService
    {
        public async Task ShowAlertMessage(string message)
        {
            await Application.Current.MainPage.DisplayAlert("Achtung", message, "Verstanden");
        }

        public async Task ShowErrorMessage(string message)
        {
            await Application.Current.MainPage.DisplayAlert("Fehler", message, "Verstanden");
        }

        public async Task ShowSuccessMessage(string message)
        {
            await Application.Current.MainPage.DisplayAlert("Erfolg", message, "Prost");
        }
    }
}
