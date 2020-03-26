using Cocktailer.Models.Entries;
using Cocktailer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace Cocktailer.ViewModels
{
    public class BluetoothConnectionViewModel : BaseViewModel
    {
        ObservableCollection<BluetoothEntry> bluetoothEntries;
        public ObservableCollection<BluetoothEntry> BluetoothEntries {
            get => bluetoothEntries;
            set
            {
                bluetoothEntries = value;
                OnPropertyChanged();
            }
        }      

        readonly IBluetoothCommunicationService bluetoothCommunicationService;
        public BluetoothConnectionViewModel(INavService navService,
            IBluetoothCommunicationService bluetoothService) : base(navService)
        {
            bluetoothCommunicationService = bluetoothService;
            BluetoothEntries = new ObservableCollection<BluetoothEntry>();
        }

        public override async void Init()
        {
            BluetoothEntries = new ObservableCollection<BluetoothEntry>(
                await bluetoothCommunicationService.GetDevicesAsync());
        }
        public Command<BluetoothEntry> ConnectCommand => new Command<BluetoothEntry>(
            async entry => await bluetoothCommunicationService.Connect(entry.Name));

        public Command RefreshCommand => new Command(async () =>
            BluetoothEntries = new ObservableCollection<BluetoothEntry>(
                await bluetoothCommunicationService.GetDevicesAsync()));
    }
}
