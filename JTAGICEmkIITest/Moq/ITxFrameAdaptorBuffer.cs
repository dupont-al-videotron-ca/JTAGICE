
using JTAGICEmkII;

namespace JTAGICEmkIITest
{
    public interface ITxFrameAdaptorBuffer : ITxFrameAdaptor
    {
        byte[] Buffer { get; }
    }
}