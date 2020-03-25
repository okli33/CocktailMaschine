using Android.Bluetooth;
using Android.OS;
using Java.Util;
using System.Runtime.CompilerServices;

namespace Cocktailer.Droid.Core.CommunicationManagement
{
    public class BluetoothConnectionManager
    {
        public const string TAG = "BluetoothConnectionManager";

        public const string NAME_SECURE = "BluetoothConnectSecure";
        public const string NAME_INSECURE = "BluetoothConnectInsecure";

        public static UUID MY_UUID_SECURE = UUID.FromString("fa87c0d0-afac-11de-8a39-0800200c9a66");
        public static UUID MY_UUID_INSECURE = UUID.FromString("8ce255c0-200a-11e0-ac64-0800200c9a66");

        const int STATE_NONE = 0;
        const int STATE_LISTEN = 1;
        const int STATE_CONNECTING = 2;
        const int STATE_CONNECTED = 3;

        public BluetoothAdapter btAdapter;
        public Handler handler;
        public AcceptThread secureAcceptThread;
        public AcceptThread insecureAcceptThread;
        public ConnectThread connectThread;
        public ConnectedThread connectedThread;
        public int state;
        public int newState;

        public BluetoothConnectionManager(Handler handler)
        {
            btAdapter = BluetoothAdapter.DefaultAdapter;
            state = STATE_NONE;
            newState = state;
            this.handler = handler;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        void UpdateUserInterfaceTitle()
        {
            state = GetState();
            newState = state;
            handler.ObtainMessage(Constants.MESSAGE_STATE_CHANGE, newState, -1).SendToTarget();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int GetState()
        {
            return state;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Start()
        {
            if (connectThread != null)
            {
                connectThread.Cancel();
                connectThread = null;
            }

            if (connectedThread != null)
            {
                connectedThread.Cancel();
                connectedThread = null;
            }

            if (secureAcceptThread == null)
            {
                secureAcceptThread = new AcceptThread(this, true);
                secureAcceptThread.Start();
            }
            if (insecureAcceptThread == null)
            {
                insecureAcceptThread = new AcceptThread(this, false);
                insecureAcceptThread.Start();
            }

            UpdateUserInterfaceTitle();
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Connect(BluetoothDevice device, bool secure)
        {
            if (state == STATE_CONNECTING)
            {
                if (connectThread != null)
                {
                    connectThread.Cancel();
                    connectThread = null;
                }
            }

            // Cancel any thread currently running a connection
            if (connectedThread != null)
            {
                connectedThread.Cancel();
                connectedThread = null;
            }

            // Start the thread to connect with the given device
            connectThread = new ConnectThread(device, this, secure);
            connectThread.Start();

            UpdateUserInterfaceTitle();
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Connected(BluetoothSocket socket, BluetoothDevice device, string socketType)
        {
            // Cancel the thread that completed the connection
            if (connectThread != null)
            {
                connectThread.Cancel();
                connectThread = null;
            }

            // Cancel any thread currently running a connection
            if (connectedThread != null)
            {
                connectedThread.Cancel();
                connectedThread = null;
            }


            if (secureAcceptThread != null)
            {
                secureAcceptThread.Cancel();
                secureAcceptThread = null;
            }

            if (insecureAcceptThread != null)
            {
                insecureAcceptThread.Cancel();
                insecureAcceptThread = null;
            }

            // Start the thread to manage the connection and perform transmissions
            connectedThread = new ConnectedThread(socket, this, socketType);
            connectedThread.Start();

            // Send the name of the connected device back to the UI Activity
            var msg = handler.ObtainMessage(Constants.MESSAGE_DEVICE_NAME);
            Bundle bundle = new Bundle();
            bundle.PutString(Constants.DEVICE_NAME, device.Name);
            msg.Data = bundle;
            handler.SendMessage(msg);

            UpdateUserInterfaceTitle();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Stop()
        {
            if (connectThread != null)
            {
                connectThread.Cancel();
                connectThread = null;
            }

            if (connectedThread != null)
            {
                connectedThread.Cancel();
                connectedThread = null;
            }

            if (secureAcceptThread != null)
            {
                secureAcceptThread.Cancel();
                secureAcceptThread = null;
            }

            if (insecureAcceptThread != null)
            {
                insecureAcceptThread.Cancel();
                insecureAcceptThread = null;
            }

            state = STATE_NONE;
            UpdateUserInterfaceTitle();
        }


        public void Write(byte[] @out)
        {
            // Create temporary object
            ConnectedThread r;
            // Synchronize a copy of the ConnectedThread
            lock (this)
            {
                if (state != STATE_CONNECTED)
                {
                    return;
                }
                r = connectedThread;
            }
            // Perform the write unsynchronized
            r.Write(@out);
        }

        public void ConnectionFailed()
        {
            state = STATE_LISTEN;

            var msg = handler.ObtainMessage(Constants.MESSAGE_TOAST);
            var bundle = new Bundle();
            bundle.PutString(Constants.TOAST, "Unable to connect device");
            msg.Data = bundle;
            handler.SendMessage(msg);
            Start();
        }

        public void ConnectionLost()
        {
            var msg = handler.ObtainMessage(Constants.MESSAGE_TOAST);
            var bundle = new Bundle();
            bundle.PutString(Constants.TOAST, "Unable to connect device.");
            msg.Data = bundle;
            handler.SendMessage(msg);

            state = STATE_NONE;
            UpdateUserInterfaceTitle();
            this.Start();
        }
    }
}
