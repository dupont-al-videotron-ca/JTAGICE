using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII;
using log4net;

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
            this._hostService = hostService ?? throw new ArgumentNullException(nameof(hostService));
            Logger = LogManager.GetLogger(this.GetType());
        }
        #endregion


        #region Fields 

        private List<IActivityElement> _activities;
        private readonly IHostDeviceService _hostService;

        public ILog Logger { get; }

        #endregion


        #region Properties 

        public TargetState TargetMcuState
        {
            get
            {
                return this.HostService.TargetMcuState;
            }
        }

        public IActivityElement? CurrentActivity { get; set; }

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

            if (CurrentActivity is IActivityElement)
            {
                return ((IActivityElement)CurrentActivity).Accept(visitor);
            }

            return false;
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
            var retval = CurrentActivity.Accept(new VisitorActivityExit(lastRequest));
            return retval;
        }

        internal bool EntryActivity()
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

        internal bool OnRequestCompleted(CommandRequest<Master.IMasterCommand, Slave.ISlaveResponse> request)
        {
            if (CurrentActivity is null)
                throw new InvalidOperationException("No current activity to action.");

            return CurrentActivity.Accept(new VisitorActivityRequestCompleted(request));
        }


        internal bool OnRequestTimeout(CommandRequest<Master.IMasterCommand, Slave.ISlaveResponse> request)
        {
            if (CurrentActivity is null)
                throw new InvalidOperationException("No current activity to action.");

            return CurrentActivity.Accept(new VisitorActivityRequestTimeout(request));
        }

        internal bool OnReceivedEvevnt(Slave.ISlaveResponse eventResponse)
        {
            if (CurrentActivity is null)
                throw new InvalidOperationException("No current activity to action.");

            return CurrentActivity.Accept(new VisitorActivityEventReceived(eventResponse));
        }

        internal void AddActivity(ActivityBase activity)
        {
            ArgumentNullException.ThrowIfNull(activity);
            _activities.Add(activity);
        }

        internal bool RunActivity(IActivityElement? activity)
        {
            try
            {
                if (activity is null)
                {
                    // TODO: end off activity!
                    return true;
                }

                if (!this.EntryActivity())
                {
                    throw new InvalidOperationException($"Activity {activity} entry failed.");
                }
                else if (!ReferenceEquals(activity, this.CurrentActivity))
                {
                    if(RunActivity(this.CurrentActivity))
                        this.CurrentActivity = activity;
                }

                if (!this.ActionActivity())
                {
                    throw new InvalidOperationException($"Activity {activity} action failed.");
                }
                else if (!ReferenceEquals(activity, this.CurrentActivity))
                {
                    if (RunActivity(this.CurrentActivity))
                        this.CurrentActivity = activity;
                }

                bool useless = true;
                if (!this.ExitActivity(useless))
                {
                    throw new InvalidOperationException($"Activity {activity} exit failed.");
                }
                else if (!ReferenceEquals(activity, this.CurrentActivity))
                {
                    if (RunActivity(this.CurrentActivity))
                        this.CurrentActivity = activity;
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    Logger.Error(ex.Message);
                }

                throw;
            }

            return true;
        }

        internal IActivityElement? Find<T>()
        {
            return this._activities.FirstOrDefault(n => n is T);
        }

        #endregion
    }
}
