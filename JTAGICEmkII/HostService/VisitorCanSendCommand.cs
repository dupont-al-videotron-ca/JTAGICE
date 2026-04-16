using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.HostService
{
    internal class VisitorCanSendCommand : VisitorCommandBase
    {


        #region Constructors 

        public VisitorCanSendCommand(Master.IMasterCommand command) :
            base()
        {
            Command = command ?? throw new ArgumentNullException(nameof(command));

        }
        #endregion


        #region Fields 

        #endregion


        #region Properties 
        public Master.IMasterCommand Command { get; }

        public override bool Visit(ActivitySignOn element)
        {
            return element.CanSendCommand(Command);
        }
        public override bool Visit(ActivitySignOff element)
        {
            return element.CanSendCommand(Command);
        }
        public override bool Visit(ActivityReset element)
        {
            return element.CanSendCommand(Command);
        }

        public override bool Visit(ActivityClearEvents element)
        {
            return element.CanSendCommand(Command);
        }

        public override bool Visit(ActivitySetDeviceDescriptor element)
        {
            return element.CanSendCommand(Command);
        }

        public override bool Visit(ActivityGetParameter element)
        {
            return element.CanSendCommand(Command);
        }

        public override bool Visit(ActivitySetParameter element)
        {
            return element.CanSendCommand(Command);
        }

        #endregion

    }
}
