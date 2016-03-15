using System;
using AutoUpdaterEasy.Resources;

namespace AutoUpdaterEasy.Exceptions
{
    public class ProtocolErrorException : Exception
    {
        public ProtocolErrorException():base(Errors.ProtocolErrorException_Message)
        {
            
        }
    }
}