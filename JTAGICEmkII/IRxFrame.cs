using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII
{
    internal interface IRxFrame
    {
        event EventHandler<MessageReceivedEventArgs>? MessageReceived;
        event EventHandler? RxTimerExpired;
        void StartReceiving();
        void StopReceiving();

        bool IsReceiving { get; }
    }
}
