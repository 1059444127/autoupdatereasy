using System;
using AutoUpdaterEasy.Resources;

namespace AutoUpdaterEasy.Exceptions
{
    public class DnsNotResolveException : Exception
    {
        public DnsNotResolveException():base(Errors.DnsNotResolveException_Message)
        {
            
        }
    }
}