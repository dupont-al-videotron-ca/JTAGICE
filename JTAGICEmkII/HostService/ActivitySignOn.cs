using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    public sealed class ActivitySignOn : ActivityProcessCommand
    {


        #region Constructors 

        public ActivitySignOn(StructureActivity activityStructure) : 
            base(activityStructure, 
                MasterCommandEnum.CMND_GET_SIGN_ON, 
                SlaveResponseEnum.RSP_SIGN_ON)
        {
        }

        #endregion


        #region Public Methods 

        public override bool Accept(IVisitorCommand visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit(this);
        }

        public override bool OnReceivedResponse(ISlaveResponse response)
        {
            switch (response.ResponseId)
            {
                case SlaveResponseEnum.RSP_SIGN_ON:
                        return true;
                default:
                    return base.OnReceivedResponse(response);
            }
        }

        public override bool ActivityEntry()
        {
            if(!this.ActivityStructure.HostService.SignOn(out ResponseSignOn? response))
                return false;
            if (response is null)
                return false;

            this.ActivityStructure.HostService.SignOnResponse = response;
            return base.ActivityAction();
        }

        #endregion
    }
}
