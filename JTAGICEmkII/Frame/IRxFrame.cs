using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII
{
    public interface IRxFrame
    {
        event EventHandler<ResponseReceivedEventArgs>? ResponseReceived;
        event EventHandler<CommandReceivedEventArgs>? CommandReceived;
        event EventHandler? RxTimerExpired;
        void StartReceiving();
        void StopReceiving();

        bool IsReceiving { get; }

        IRxFrameAdaptor Adaptor { get; }

    }

}
