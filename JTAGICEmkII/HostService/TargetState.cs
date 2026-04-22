using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Slave;
using log4net;

namespace JTAGICEmkII.HostService
{
    [DebuggerDisplay("State={State}")]
    public class TargetState
    {


        #region Constructors 

        public TargetState()
        {
            State = McuStateEnum.Unknown;
            Logger = LogManager.GetLogger(this.GetType());
        }

        #endregion


        #region Properties 
        protected readonly ILog Logger;

        public McuStateEnum State { get; private set; } 

        public bool IsRunning => State == McuStateEnum.RUNNING;
        public bool IsStopped => State == McuStateEnum.STOPPED;
        public bool IsProgramming => State == McuStateEnum.PROGRAMMING;
        public bool IsReset => State == McuStateEnum.Unknown;

        #endregion

        #region Public Methods 

        public void GoRunning()
        {
            Logger.Debug("Transitioning to RUNNING state.");
            State = McuStateEnum.RUNNING;
        }
        public void GoStopped()
        {
            Logger.Debug("Transitioning to STOPPED state.");
            State = McuStateEnum.STOPPED;
        }
        public void GoProgramming()
        {
            Logger.Debug("Transitioning to PROGRAMMING state.");
            State = McuStateEnum.PROGRAMMING;
        }

        public void ResetState()
        {
            Logger.Debug("Resetting state to UNKNOWN.");    
            State = McuStateEnum.Unknown;
        }
        #endregion

    }
}
