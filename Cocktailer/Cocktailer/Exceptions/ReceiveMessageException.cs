using System;
using System.Collections.Generic;
using System.Text;

namespace Cocktailer.Exceptions
{
    public class ReceiveMessageException : Exception
    {
        public ReceiveMessageException(string message) : base(message)
        {

        }
        public ReceiveMessageException() { }
    }
}
