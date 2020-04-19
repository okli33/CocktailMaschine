using Android.Bluetooth;
using Android.Util;
using Java.Lang;


namespace Cocktailer.Droid.Core.CommunicationManagement
{
    public class AcceptThread : Thread
    {
        const int STATE_NONE = 0;
        const int STATE_LISTEN = 1;
        const int STATE_CONNECTING = 2;
        const int STATE_CONNECTED = 3;

        BluetoothServerSocket serverSocket;
        string socketType;
        BluetoothConnectionManager manager;

        public AcceptThread(BluetoothConnectionManager manager, bool secure)
        {
            BluetoothServerSocket tmp = null;
            socketType = secure ? "Secure" : "Insecure";
            this.manager = manager;

            try
            {
                if (secure)
                {
                    tmp = manager.btAdapter.ListenUsingRfcommWithServiceRecord(BluetoothConnectionManager.NAME_SECURE, BluetoothConnectionManager.MY_UUID_SECURE);
                }
                else
                {
                    tmp = manager.btAdapter.ListenUsingInsecureRfcommWithServiceRecord(BluetoothConnectionManager.NAME_INSECURE, BluetoothConnectionManager.MY_UUID_INSECURE);
                }

            }
            catch (Java.IO.IOException e)
            {
                Log.Error(BluetoothConnectionManager.TAG, "listen() failed", e);
            }
            serverSocket = tmp;
            manager.state = STATE_LISTEN;
        }

        public override void Run()
        {
            Name = $"AcceptThread_{socketType}";
            BluetoothSocket socket = null;

            while (manager.GetState() != STATE_CONNECTED)
            {
                try
                {
                    socket = serverSocket.Accept();
                }
                catch (Java.IO.IOException e)
                {
                    Log.Error(BluetoothConnectionManager.TAG, "accept() failed", e);
                    break;
                }

                if (socket != null)
                {
                    lock (this)
                    {
                        switch (manager.GetState())
                        {
                            case STATE_LISTEN:
                            case STATE_CONNECTING:
                                // Situation normal. Start the connected thread.
                                manager.Connected(socket, socket.RemoteDevice, socketType);
                                break;
                            case STATE_NONE:
                            case STATE_CONNECTED:
                                try
                                {
                                    socket.Close();
                                }
                                catch (Java.IO.IOException e)
                                {
                                    Log.Error(BluetoothConnectionManager.TAG, "Could not close unwanted socket", e);
                                }
                                break;
                        }
                    }
                }
            }

        }

        public void Cancel()
        {
            try
            {
                serverSocket.Close();
            }
            catch (Java.IO.IOException e)
            {
                Log.Error(BluetoothConnectionManager.TAG, "close() of server failed", e);
            }
        }
    }
}
