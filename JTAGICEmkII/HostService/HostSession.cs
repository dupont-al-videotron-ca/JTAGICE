using System.Threading;

namespace JTAGICEmkII.HostService
{
    public class HostSession : ActivityBaseComp
    {
        #region Constructors 
        public HostSession(StructureActivity activityStructure) : this(activityStructure, null)
        {
        }

        public HostSession(StructureActivity activityStructure, IActivityElement? parent) : base(activityStructure, parent)
        {
            _waitHandle = new ManualResetEvent(false);
            _targetConnecting = new TargetConnecting(activityStructure);

            _targetConnected = new TargetConnected(activityStructure, _targetConnecting);
            _targetConnecting.AddNext(_targetConnected);

            _targetDisconnecting = new TargetDisonnecting(activityStructure, _targetConnected);
            _targetConnected.AddNext(_targetDisconnecting);

        }

        #endregion


        #region Fields 

        private TargetConnecting _targetConnecting;
        private TargetConnected _targetConnected;
        private TargetDisonnecting _targetDisconnecting;
        private ManualResetEvent _waitHandle;

        #endregion


        #region Properties 
        public bool IsSessionActive { get; internal set; }

        #endregion



        #region Public Methods 
        public override bool Accept(IVisitorActivity visitor)
        {
            return visitor.Visit(this);
        }

        public override bool ActivityEntry()
        {
            _waitHandle.Reset();
            IsSessionActive = true;
            this.NextActivity = this._targetConnecting;
            return base.ActivityEntry();
        }

        public override bool ActivityExit(bool lastRequest)
        {
            _waitHandle.Set();
            IsSessionActive = false;
            this.NextActivity = null;
            return true;
        }

        internal bool WaitEndSession()
        {
            return _waitHandle.WaitOne(TimeSpan.FromSeconds(60));
        }


        #endregion

    }
}
