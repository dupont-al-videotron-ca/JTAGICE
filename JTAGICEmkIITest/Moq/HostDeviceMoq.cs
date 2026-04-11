using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII;
using JTAGICEmkII.HostService;
using JTAGICEmkII.Slave;

namespace JTAGICEmkIITest.Moq
{
    public class HostDeviceMoq : IHostDeviceService
    {
        private ResponseSignOn? _signOnResponse = null;
        
        public ResponseSignOn SignOnResponse { get => _signOnResponse!; set => _signOnResponse = value; }
        public TargetState TargetMcuState { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool ClearBreakpoint(int Index, ulong Breakpoint) => throw new NotImplementedException();
        public bool ClearEvents() => throw new NotImplementedException();
        public bool EnterPrograming() => throw new NotImplementedException();
        public bool EraseDevice() => throw new NotImplementedException();
        public bool EraseMemory(int MemType, ulong Address, ulong Length) => throw new NotImplementedException();
        public bool GetAllParameter() => throw new NotImplementedException();
        public bool GetBreakpoint(int Index, ulong Breakpoint, int BreakpointType, int BrakpointMode) => throw new NotImplementedException();
        public bool GetParameter(int paramId, out uint value) => throw new NotImplementedException();
        public bool LeavePrograming() => throw new NotImplementedException();
        public bool ModifyAllParameter() => throw new NotImplementedException();
        public bool ReadMemory(int memType, ulong Address, ulong Length, out byte Values) => throw new NotImplementedException();
        public bool ReadProgramCount(out ulong ProgramCounter) => throw new NotImplementedException();
        public bool Reconnect() => throw new NotImplementedException();
        public bool Reset() => throw new NotImplementedException();
        public bool SetAllParameter() => throw new NotImplementedException();
        public bool SetBreakpoint(int index, ulong Breakpoint, int BreakpointType, int BrakpointMode) => throw new NotImplementedException();
        public bool SetDeviceDescriptor() => throw new NotImplementedException();
        public bool SetParameter(int paramId, uint value) => throw new NotImplementedException();
        public bool SignOff() => throw new NotImplementedException();
        public bool SignOn(out ResponseSignOn? response) => throw new NotImplementedException();
        public bool StartRunning() => throw new NotImplementedException();
        public bool StartRunningUntil(ulong Breakpoint) => throw new NotImplementedException();
        public bool StepIn(ulong ProgramCounter) => throw new NotImplementedException();
        public bool StopRunning() => throw new NotImplementedException();
        public bool VerifiyPrograming() => throw new NotImplementedException();
        public bool WriteMemory(int MemType, ulong Address, byte Values) => throw new NotImplementedException();
        public bool WriteProgramCount(ulong ProgramCounter) => throw new NotImplementedException();
        public bool WritePrograming(ulong Address, byte Values) => throw new NotImplementedException();
    }
}
