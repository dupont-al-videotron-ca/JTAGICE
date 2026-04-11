using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII
{
    internal interface ITxFrame
    {
        int BuildAndSendFrameCommand(Master.Command command);
        Task<int> BuildAndSendFrameCommandAsync(Master.Command command, CancellationToken cancellationToken);

        ITxFrameAdaptor Adaptor { get; }

    }
}
