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
            TargetMcuState = McuStateEnum.Unknown;
        }

        public McuStateEnum TargetMcuState { get; set; } = McuStateEnum.Unknown;

        public bool IsRunning => TargetMcuState == McuStateEnum.RUNNING;
        public bool IsStopped => TargetMcuState == McuStateEnum.STOPPED;
        public bool IsProgramming=> TargetMcuState == McuStateEnum.PROGRAMMING;

        public void GoRunning()
        {
            TargetMcuState = McuStateEnum.RUNNING;
        }
        public void GoStopped()
        {
            TargetMcuState = McuStateEnum.STOPPED;
        }
        public void GoProgramming()
        {
            TargetMcuState = McuStateEnum.PROGRAMMING;
        }
    }
}
