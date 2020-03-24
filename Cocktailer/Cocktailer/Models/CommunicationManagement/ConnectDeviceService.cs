using Android.App;
using Android.Bluetooth;
using Android.Content;
using System.Collections.Generic;

namespace Cocktailer.Models.CommunicationManagement
{
    public class ConnectDeviceService : Activity
    {
        string arduinoName = "";

        BluetoothAdapter btAdapter;
        public static List<string> newDevices;
        List<string> coupledDevices;
        DeviceDiscoveredReceiver receiver;
        public string DEVICE_ADDRESS;
        public ConnectDeviceService()
        {
            newDevices = new List<string>();
            coupledDevices = new List<string>();
            receiver = new DeviceDiscoveredReceiver();
            var filter = new IntentFilter(BluetoothDevice.ActionFound);
            RegisterReceiver(receiver, filter);

            // Register for broadcasts when discovery has finished
            filter = new IntentFilter(BluetoothAdapter.ActionDiscoveryFinished);
            RegisterReceiver(receiver, filter);

            // Get the local Bluetooth adapter
            btAdapter = BluetoothAdapter.DefaultAdapter;
            // Get a set of currently paired devices
            var pairedDevices = btAdapter.BondedDevices;

            // If there are paired devices, add each on to the ArrayAdapter
            if (pairedDevices.Count > 0)
            {
                foreach (var device in pairedDevices)
                {
                    coupledDevices.Add(device.Name + "\n" + device.Address);
                }
                if (coupledDevices.Contains(arduinoName))
                {
                    DEVICE_ADDRESS = arduinoName;
                }
                else
                {
                    DEVICE_ADDRESS = "NOT_FOUND";
                }
            }
        }
        public void DoDiscovery()
        {
            // If we're already discovering, stop it
            if (btAdapter.IsDiscovering)
            {
                btAdapter.CancelDiscovery();
            }

            // Request discover from BluetoothAdapter
            var x = btAdapter.StartDiscovery();
        }
    }

    public class DeviceDiscoveredReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            string action = intent.Action;

            // When discovery finds a device
            if (action == BluetoothDevice.ActionFound)
            {
                // Get the BluetoothDevice object from the Intent
                BluetoothDevice device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                // If it's already paired, skip it, because it's been listed already
                if (device.BondState != Bond.Bonded)
                {
                    ConnectDeviceService.newDevices.Add(device.Name + "\n" + device.Address);
                }
                // When discovery is finished, change the Activity title
            }
        }
    }
}
