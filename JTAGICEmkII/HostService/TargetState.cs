using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    public class TargetState
    {

        public TargetState() 
        { 
            State = McuStateEnum.Unknown;
        }

        public McuStateEnum State { get; set; } = McuStateEnum.Unknown;

        public bool IsRunning => State == McuStateEnum.RUNNING;
        public bool IsStopped => State == McuStateEnum.STOPPED;
        public bool IsProgramming=> State == McuStateEnum.PROGRAMMING;

        public void GoRunning()
        {
            State = McuStateEnum.RUNNING;
        }
        public void GoStopped()
        {
            State = McuStateEnum.STOPPED;
        }
        public void GoProgramming()
        {
            State = McuStateEnum.PROGRAMMING;
        }

        public void ResetState()
        {
            State = McuStateEnum.Unknown;
        }   
    }
}
