using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII;
using JTAGICEmkII.HostService;
using JTAGICEmkII.Slave;
using log4net.Repository.Hierarchy;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Xunit;

namespace JTAGICEmkIITest.Moq
{
    public class HostDeviceMoq : IHostDeviceService
    {
        public HostDeviceMoq()
        {
            TargetMcuState = new TargetState();
            Assert.Equal(McuStateEnum.Unknown,TargetMcuState.State);
        }

        private ResponseSignOn? _signOnResponse = null;

        public ResponseSignOn SignOnResponse { get => _signOnResponse!; set => _signOnResponse = value; }
        public TargetState TargetMcuState { get; set; }

        public bool ForceSuccess { get; set; } = true;

        public bool ClearBreakpoint(int Index, ulong Breakpoint) => throw new NotImplementedException();
        public bool ClearEvents() => ForceSuccess;
        public bool EnterPrograming() => throw new NotImplementedException();
        public bool EraseDevice() => throw new NotImplementedException();
        public bool EraseMemory(int MemType, ulong Address, ulong Length) => throw new NotImplementedException();
        public bool GetAllParameter() => ForceSuccess;
        public bool GetBreakpoint(int Index, ulong Breakpoint, int BreakpointType, int BrakpointMode) => throw new NotImplementedException();
        public bool GetParameter(int paramId, out uint value) => throw new NotImplementedException();
        public bool LeavePrograming() => throw new NotImplementedException();
        public bool ModifyAllParameter() => throw new NotImplementedException();
        public bool ReadMemory(int memType, ulong Address, ulong Length, out byte Values) => throw new NotImplementedException();
        public bool ReadProgramCount(out ulong ProgramCounter) => throw new NotImplementedException();
        public bool Reconnect() => throw new NotImplementedException();
        public bool Reset() => ForceSuccess;
        public bool SetAllParameter() => ForceSuccess;
        public bool SetBreakpoint(int index, ulong Breakpoint, int BreakpointType, int BrakpointMode) => throw new NotImplementedException();
        public bool SetDeviceDescriptor() => ForceSuccess;
        public bool SetParameter(int paramId, uint value) => throw new NotImplementedException();
        public bool SignOff() => throw new NotImplementedException();
        public bool SignOn(out ResponseSignOn? response)
        {
            response = new ResponseSignOn(SlaveResponseEnum.RSP_SIGN_ON)
            {
                CommunicationProtocolVersion = 1,
                MasterMcuBootLoaderVersion = 1,
                MasterMcuHwVersion = 1,
                MasterMcuFirmwareVersionMajor = 1,
                MasterMcuFirmwareVersionMinor = 0,
                SlaveMcuBootLoaderVersion = 1,
                SlaveMcuFirmwareVersionMajor = 1,
                SlaveMcuFirmwareVersionMinor = 0,
                SlaveMcuHwVersion = 1,
                SerialNumber = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 },
                DeviceId = new byte[] { 0x10, 0x20, 0x30, 0x40 }
            };
            return ForceSuccess;
        }
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
