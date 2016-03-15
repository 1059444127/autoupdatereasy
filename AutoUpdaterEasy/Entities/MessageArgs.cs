using System;

namespace AutoUpdaterEasy.Entities
{
    public class MessageArgs : EventArgs
    {        
        public MessageArgs(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}
