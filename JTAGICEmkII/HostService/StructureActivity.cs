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

        private readonly List<IActivityElement> _activities;
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

        public IActivityElement? CurrentActivity { get; internal set; }

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

            if (this.CurrentActivity is IActivityComElement)
            {
                return ((IActivityComElement)CurrentActivity).Accept(visitor);
            }

            return false;
        }

        internal bool AcceptVisitor(IVisitorActivity visitor)
        {
            if (CurrentActivity is null)
                throw new InvalidOperationException("No current activity to accept visitor.");

            if (this.CurrentActivity is not null)
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
            IVisitorActivity v = new VisitorActivityEventReceived(response);
            foreach (IActivityElement activity in _activities.Where(a => a is IActivityElement))
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

        internal bool OnRequestCompleted(CommandRequestBase<Master.IMasterCommand, Slave.ISlaveResponse> request)
        {
            if (CurrentActivity is null)
                throw new InvalidOperationException("No current activity to action.");

            return CurrentActivity.Accept(new VisitorActivityRequestCompleted(request));
        }


        internal bool OnRequestTimeout(CommandRequestBase<Master.IMasterCommand, Slave.ISlaveResponse> request)
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
                Logger.Debug($"Running activity: {activity?.GetType().Name ?? "null"}");
                if (activity is null)
                {
                    // end off activity!
                    return true;
                }

                if (!this.EntryActivity())
                {
                    throw new InvalidOperationException($"Activity {activity} entry failed.");
                }
                else if (!ReferenceEquals(activity.NextActivity, this.CurrentActivity))
                {
                    this.CurrentActivity = activity.NextActivity;
                    if (RunActivity(activity.NextActivity))
                        this.CurrentActivity = activity;
                }

                if (!this.ActionActivity())
                {
                    throw new InvalidOperationException($"Activity {activity} action failed.");
                }
                else if (!ReferenceEquals(activity.NextActivity, this.CurrentActivity))
                {
                    this.CurrentActivity = activity.NextActivity;
                    if (RunActivity(activity.NextActivity))
                        this.CurrentActivity = activity;
                }

                bool useless = true;
                if (!this.ExitActivity(useless))
                {
                    throw new InvalidOperationException($"Activity {activity} exit failed.");
                }
                else if (!ReferenceEquals(activity.NextActivity, this.CurrentActivity))
                {
                    this.CurrentActivity = activity.NextActivity;
                    if (RunActivity(activity.NextActivity))
                        this.CurrentActivity = activity;
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    Logger.Error(ex.Message);
                }
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
