using Android.Bluetooth;
using Android.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cocktailer.Models.CommunicationManagement
{
    public class BluetoothConnector
    {
        BluetoothAdapter bluetoothAdapter;
        BluetoothConnectionManager manager;

        void ConnectDevice(Intent data, bool secure)
        {
            var address = data.Extras.GetString(DeviceListActivity.EXTRA_DEVICE_ADDRESS);
            var device = bluetoothAdapter.GetRemoteDevice(address);
            manager.Connect(device, secure);
        }
    }
}
