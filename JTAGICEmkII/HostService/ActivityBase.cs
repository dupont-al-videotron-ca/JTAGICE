using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;
using log4net;

using log4net.Repository.Hierarchy;

namespace JTAGICEmkII.HostService
{
    public abstract class ActivityBase : IActivityElement
    {

        #region Constructors 

        internal ActivityBase(StructureActivity activityStructure)
        {
            Logger = LogManager.GetLogger(this.GetType());
            this.ActivityStructure = activityStructure ?? throw new ArgumentNullException(nameof(activityStructure));
            this.ActivityStructure.AddActivity(this);
            NextActivity = this;
        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 

        protected ILog Logger { get; }
        protected StructureActivity ActivityStructure { get; }

        public IActivityElement? NextActivity { get; internal set; }

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 
        public abstract bool Accept(IVisitorActivity visitor);

        public virtual bool ActivityExit(bool lastRequest)
        {
            Logger.Debug($"ActivityExit called.");
            return true;
        }

        public virtual bool ActivityAction()
        {
            Logger.Debug($"ActivityAction called.");
            return true;
        }

        public virtual bool ActivityEntry()
        {
            Logger.Debug($"ActivityEntry called.");
            return true;
        }

        public virtual bool EventReceived(ISlaveResponse responceEvent)
        {
            Logger.Debug($"EventReceived {responceEvent.ResponseId} called.");
            return false;
        }

        public virtual bool RequestTimeout(CommandRequestBase<IMasterCommand, ISlaveResponse> request)
        {
            Logger.Debug($"RequestTimeout called.");
            return true;
        }

        public virtual bool RequestCompleted(CommandRequestBase<IMasterCommand, ISlaveResponse> request)
        {
            Logger.Debug($"RequestCompleted called.");
            return true;
        }


        #endregion
    }
}
