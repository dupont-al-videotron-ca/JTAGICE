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
        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 

        protected ILog Logger { get; }
        protected StructureActivity ActivityStructure { get; }

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 
        public abstract bool Accept(IVisitorActivity visitor);

        public virtual bool ActivityExit(bool lastRequest)
        {
            Logger.Debug($"{this.GetType()} Executing activity exit called.");
            return true;
        }

        public virtual bool ActivityAction()
        {
            Logger.Debug($"{this.GetType()} Executing activity action called.");
            return true;
        }

        public virtual bool ActivityEntry()
        {
            Logger.Debug($"{this.GetType()} Executing activity entry called.");
            return true;
        }

        public virtual bool EventReceived(ISlaveResponse responceEvent)
        {
            Logger.Debug($"{this.GetType()} Event received {responceEvent.ResponseId}.");
            return true;
        }

        public virtual bool RequestTimeout(CommandRequest<IMasterCommand, ISlaveResponse> request)
        {
            Logger.Debug($"{this.GetType()} timout.");
            return true;
        }

        public virtual bool RequestCompleted(CommandRequest<IMasterCommand, ISlaveResponse> request)
        {
            Logger.Debug($"{this.GetType()} Request completed.");
            return true;
        }


        #endregion
    }
}
