using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    internal sealed class ActivitySignOn : ActivityProcessCommand
    {


        #region Constructors 
        public ActivitySignOn(StructureActivity activityStructure, IActivityElement? parent) : base(activityStructure, parent!)
        {
        }
        #endregion


        #region Fields 

        #endregion


        #region Properties 

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 
        public override bool Accept(IVisitorCommand visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit(this);
        }

        public override bool CanSendCommand(IMasterCommand command)
        {
            switch(command.MessageId)
            {
                case MasterCommandEnum.CMND_GET_SIGN_ON:
                    return true;
                default:
                    return false;
            }
        }

        public override bool OnReceivedResponse(ISlaveResponse response)
        {
            _nextIndex = -1;
            switch (response.ResponseId)
            {
                case SlaveResponseEnum.RSP_SIGN_ON:
                    _nextIndex = 0;
                    return true;
                default:
                    return base.OnReceivedResponse(response);
            }
        }

        public override bool ActivityEntry()
        {
            Logger.Debug($"{this.GetType()} Executing activity entry called.");
            if(!this.ActivityStructure.HostService.SignOn(out ResponseSignOn? response))
                return false;
            if (response is null)
                return false;

            this.ActivityStructure.HostService.SignOnResponse = response;
            return true;
        }

        #endregion


        #region Protected Methods 

        #endregion

        #region Private Methods 

        #endregion

        #region Private Classes / Enum 

        #endregion
    }
}
