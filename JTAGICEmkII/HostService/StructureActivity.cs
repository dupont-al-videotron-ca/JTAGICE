using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII;

namespace JTAGICEmkII.HostService
{
    public class StructureActivity : IEnumerable<IActivityElement>,
        ICollection<IActivityElement>,
        IReadOnlyCollection<IActivityElement>
    {

        #region Constructors 
        internal StructureActivity(IHostDeviceService hostService)
        {
            _activities = new List<IActivityElement>();
            CurrentActivity = null;
            this._hostService = hostService ?? throw new ArgumentNullException(nameof(hostService));

        }
        #endregion


        #region Fields 

        private IActivityElement? _currentActivity;
        private List<IActivityElement> _activities;
        private readonly IHostDeviceService _hostService;

        #endregion


        #region Properties 

        public TargetState TargetMcuState
        {
            get
            {
                return this.HostService.TargetMcuState;
            }
        }

        public IActivityElement? CurrentActivity { get => this._currentActivity; set => this._currentActivity = value; }

        public IHostDeviceService HostService => this._hostService;

        public int Count => _activities.Count;

        public bool IsReadOnly => false;

        #endregion

        #region Public Methods 

        public IEnumerator<IActivityElement> GetEnumerator() => _activities.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        public void Add(IActivityElement item) => _activities.Add(item);
        public void Clear() => _activities.Clear();
        public bool Contains(IActivityElement item) => _activities.Contains(item);
        public void CopyTo(IActivityElement[] array, int arrayIndex) => _activities.CopyTo(array, arrayIndex);
        public bool Remove(IActivityElement item) => _activities.Remove(item);

        internal bool AcceptVisitor(IVisitorCommand visitor)
        {
            if (CurrentActivity is null)
                throw new InvalidOperationException("No current activity to accept visitor.");

            if (CurrentActivity is IActivityComElement)
            {
                return ((IActivityComElement)CurrentActivity).Accept(visitor);
            }

            return false;
        }

        internal bool AcceptVisitor(IVisitorActivity visitor)
        {
            if (CurrentActivity is null)
                throw new InvalidOperationException("No current activity to accept visitor.");

            return CurrentActivity.Accept(visitor);
        }

        internal bool CanSendCommand(Master.IMasterCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);
            return AcceptVisitor(new VisitorCanSendCommand(command));

        }

        internal bool CommandSent(Master.IMasterCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);
            return AcceptVisitor(new VisitorCommandSent(command));

        }

        internal bool OnReceivedResponse(Slave.ISlaveResponse response)
        {
            ArgumentNullException.ThrowIfNull(response);
            return AcceptVisitor(new VisitorOnReceivedResponse(response));
            
        }

        internal bool OnReceivedEvent(Slave.ISlaveResponse response)
        {
            IVisitorCommand v = new VisitorOnReceivedResponse(response);
            foreach (IActivityComElement activity in _activities.Where(a => a is IActivityComElement))
            {
                if (activity.Accept(v))
                    return true;
            }
            return false;   
        }

        internal bool ExitActivity(bool lastRequest)
        {
            if (CurrentActivity is null)
                throw new InvalidOperationException("No current activity to exit.");
            return CurrentActivity.Accept(new VisitorActivityExit(lastRequest));
        }

        internal bool EntryActivity(bool lastRequest)
        {
            if (CurrentActivity is null)
                throw new InvalidOperationException("No current activity to enter.");
            return CurrentActivity.Accept(new VisitorActivityEntry());
        }

        internal bool ActionActivity()
        {
            if (CurrentActivity is null)
                throw new InvalidOperationException("No current activity to action.");
            return CurrentActivity.Accept(new VisitorActivityAction());
        }

        internal void AddActivity(ActivityBase activity)
        {
            ArgumentNullException.ThrowIfNull(activity);
            _activities.Add(activity);
        }
        #endregion

    }
}
