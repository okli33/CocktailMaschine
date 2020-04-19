using Android.Bluetooth;
using Android.Content;

namespace Cocktailer.Droid.Core.CommunicationManagement
{
    public class BluetoothConnector
    {
        BluetoothAdapter bluetoothAdapter;
        BluetoothConnectionManager manager;
        public BluetoothConnector(BluetoothConnectionManager man, BluetoothAdapter adapter)
        {
            manager = man;
            if (adapter == null)
            {
                bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            }
            else
            {
                bluetoothAdapter = adapter;
            }
        }
        void ConnectDevice(Intent data, bool secure)
        {
            var address = new ConnectDeviceService().DEVICE_ADDRESS;
            if (address == "NOT_FOUND")
            {
                throw new BluetoothNotCoupledException("Could not find CocktailMachine");
            }
            var device = bluetoothAdapter.GetRemoteDevice(address);
            manager.Connect(device, secure);
        }
    }
}
