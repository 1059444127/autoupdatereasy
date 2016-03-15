using System;
using AutoUpdaterEasy.Resources;

namespace AutoUpdaterEasy.Exceptions
{
    public class ConnectionFailureException : Exception
    {
        public ConnectionFailureException():base(Errors.ConnectionFailureException_Message)
        {
            
        }
    }
}