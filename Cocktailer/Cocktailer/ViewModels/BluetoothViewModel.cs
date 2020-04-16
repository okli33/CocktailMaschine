using Cocktailer.Models.Entries;
using Cocktailer.Services;
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cocktailer.ViewModels
{
    public class BluetoothViewModel : BaseViewModel
    {
        private string messageToSend;
        public string MessageToSend
        {
            get => messageToSend;
            set
            {
                messageToSend = value;
                OnPropertyChanged();
            }
        }
        private string receivedMessage;
        public string ReceivedMessage
        {
            get => receivedMessage;
            set
            {
                receivedMessage = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<BluetoothEntry> foundDevices;
        public ObservableCollection<BluetoothEntry> FoundDevices
        {
            get => foundDevices;
            set
            {
                foundDevices = value;
                OnPropertyChanged();
            }
        }
        IBluetoothCommunicationService btService;
        public BluetoothViewModel(INavService navService, IBluetoothCommunicationService bluetooth) : base(navService)
        {
            btService = bluetooth;
        }

        public override void Init()
        {
            btService.Init();
            ReceivedMessage = "noch nichts empfangen";
        }

        public Command WriteMessageCommand => new Command(async () => await SendMessage());
        

        private async Task SendMessage()
        {
            try
            {
                await btService.Write(MessageToSend);
            }
            catch (Exception ex)
            {
                await Application.Current
                    .MainPage.DisplayAlert("Fehler", ex.Message, "OK");
            }
        }

        public Command ReceiveMessageCommand => new Command(async () => await ReceiveMessage());
        private async Task ReceiveMessage()
        {
            try
            {
                ReceivedMessage = Encoding.ASCII.GetString(await btService.Read());
            }
            catch (Exception ex)
            {
                ReceivedMessage = "Fehler beim Empfangen";
                await Application.Current.MainPage
                    .DisplayAlert("Fehler", ex.Message, "OK");
            }
        }
    }
}
