using Android.Bluetooth;
using Android.Util;
using Java.Lang;

namespace Cocktailer.Models.CommunicationManagement
{
    public class ConnectThread : Thread
    {
        const int STATE_NONE = 0;
        const int STATE_LISTEN = 1;
        const int STATE_CONNECTING = 2;
        const int STATE_CONNECTED = 3;

        BluetoothSocket socket;
        BluetoothDevice device;
        BluetoothConnectionManager manager;
        string socketType;

        public ConnectThread(BluetoothDevice device, BluetoothConnectionManager manager, bool secure)
        {
            this.device = device;
            this.manager = manager;
            BluetoothSocket tmp = null;
            socketType = secure ? "Secure" : "Insecure";

            try
            {
                if (secure)
                {
                    tmp = device.CreateRfcommSocketToServiceRecord(BluetoothConnectionManager.MY_UUID_SECURE);
                }
                else
                {
                    tmp = device.CreateInsecureRfcommSocketToServiceRecord(BluetoothConnectionManager.MY_UUID_INSECURE);
                }

            }
            catch (Java.IO.IOException e)
            {
                Log.Error(BluetoothConnectionManager.TAG, "create() failed", e);
            }
            socket = tmp;
            manager.state = STATE_CONNECTING;
        }

        public override void Run()
        {
            Name = $"ConnectThread_{socketType}";

            // Always cancel discovery because it will slow down connection
            manager.btAdapter.CancelDiscovery();

            // Make a connection to the BluetoothSocket
            try
            {
                // This is a blocking call and will only return on a
                // successful connection or an exception
                socket.Connect();
            }
            catch (Java.IO.IOException e)
            {
                // Close the socket
                try
                {
                    socket.Close();
                }
                catch (Java.IO.IOException e2)
                {
                    Log.Error(BluetoothConnectionManager.TAG, $"unable to close() {socketType} socket during connection failure.", e2);
                }

                // Start the service over to restart listening mode
                manager.ConnectionFailed();
                return;
            }

            // Reset the ConnectThread because we're done
            lock (this)
            {
                manager.connectThread = null;
            }

            // Start the connected thread
            manager.Connected(socket, device, socketType);
        }

        public void Cancel()
        {
            try
            {
                socket.Close();
            }
            catch (Java.IO.IOException e)
            {
                Log.Error(BluetoothConnectionManager.TAG, "close() of connect socket failed", e);
            }
        }

    }
}
