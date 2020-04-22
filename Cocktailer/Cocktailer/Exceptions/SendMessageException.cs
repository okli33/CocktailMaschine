using System;
using System.Collections.Generic;
using System.Text;

namespace Cocktailer.Exceptions
{
    public class SendMessageException : Exception
    {
        public SendMessageException(string message) : base(message)
        {

        }
        public SendMessageException() { }
    }
}
