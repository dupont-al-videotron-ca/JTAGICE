using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    internal sealed class ActivityGetAllParameter : ActivityProcessCommand
    {


        #region Constructors 
        public ActivityGetAllParameter(StructureActivity activityStructure, IActivityElement? parent) : base(activityStructure, parent!)
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
            return visitor.Visit(this);
        }

        public override bool CanSendCommand(IMasterCommand command)
        {
            switch(command.MessageId)
            {
                case MasterCommandEnum.CMND_GET_PARAMETER:
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

        #endregion


        #region Protected Methods 

        #endregion

        #region Private Methods 

        #endregion

        #region Private Classes / Enum 

        #endregion
    }
}
