using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.HostService
{
    internal class VisitorCommandSent : VisitorCommandBase
    {


        #region Constructors 

        internal VisitorCommandSent(Master.IMasterCommand command) : base()
        {
            this.Command = command ?? throw new ArgumentNullException(nameof(command));
        }
        #endregion


        #region Fields 

        #endregion


        #region Properties 
        public Master.IMasterCommand Command { get; }


        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        public override bool Visit(ActivitySignOn element)
        {
            return element.CommandSent();
        }

        public override bool Visit(ActivitySignOff element)
        {
            return element.CommandSent();
        }

        public override bool Visit(ActivityReset element)
        {
            return element.CommandSent();
        }

        public override bool Visit(ActivityClearEvents element)
        {
            return element.CommandSent();
        }

        public override bool Visit(ActivitySetDeviceDescriptor element)
        {
            return element.CommandSent();
        }
        public override bool Visit(ActivityGetAllParameter element)
        {
            return element.CommandSent();
        }

        public override bool Visit(ActivitySetAllParameter element)
        {
            return element.CommandSent();
        }
        #endregion

    }
}
