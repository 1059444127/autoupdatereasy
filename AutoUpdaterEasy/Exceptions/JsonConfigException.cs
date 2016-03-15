using System;
using AutoUpdaterEasy.Resources;

namespace AutoUpdaterEasy.Exceptions
{
    public class JsonConfigException : Exception
    {
        public JsonConfigException():base(Errors.ProtocolErrorException_Message)
        {
            
        }
    }
}