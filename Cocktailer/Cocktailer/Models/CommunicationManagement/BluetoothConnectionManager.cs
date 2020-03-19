using Android.Bluetooth;
using Android.OS;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cocktailer.Models.CommunicationManagement
{
    public class BluetoothConnectionManager
    {
        const string TAG = "BluetoothConnectionManager";

        const string NAME_SECURE = "BluetoothChatSecure";
        const string NAME_INSECURE = "BluetoothChatInsecure";

        static UUID MY_UUID_SECURE = UUID.FromString("fa87c0d0-afac-11de-8a39-0800200c9a66");
        static UUID MY_UUID_INSECURE = UUID.FromString("8ce255c0-200a-11e0-ac64-0800200c9a66");

        BluetoothAdapter btAdapter;
        Handler handler;
        AcceptThread secureAcceptThread;
        AcceptThread insecureAcceptThread;
        ConnectThread connectThread;
        ConnectedThread connectedThread;
        int state;
        int newState;

        public const int STATE_NONE = 0;       // we're doing nothing
        public const int STATE_LISTEN = 1;     // now listening for incoming connections
        public const int STATE_CONNECTING = 2; // now initiating an outgoing connection
        public const int STATE_CONNECTED = 3;  // now connected to a remote device

    }
}
