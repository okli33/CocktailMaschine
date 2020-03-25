using System;
using System.Runtime.Serialization;

namespace Cocktailer.Droid.Core.CommunicationManagement
{
    [Serializable]
    internal class BluetoothNotCoupledException : Exception
    {
        public BluetoothNotCoupledException()
        { 
        }

        public BluetoothNotCoupledException(string message) : base(message)
        {
        }

        public BluetoothNotCoupledException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BluetoothNotCoupledException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}