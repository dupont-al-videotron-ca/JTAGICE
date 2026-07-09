namespace JTAGICEmkII
{
    public interface IRxFrameAdaptorTest: IRxFrameAdaptor
    {
        int RxTimeout { get; }
        void Attach(RxFrame rxFrame);
        bool WaitForTimeout { get; set; }
    }
}