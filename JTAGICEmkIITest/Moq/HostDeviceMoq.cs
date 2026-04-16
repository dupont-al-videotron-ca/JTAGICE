#pragma warning disable CS0067
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

        public event EventHandler<RequestEventArgs>? RequestCompleted;
        public event EventHandler<EventReceivedEventArgs>? EventReceived;
        public event EventHandler<RequestEventArgs>? RequestTimeout;
        public event EventHandler<ResponseReceivedEventArgs>? ResponseReceived;
        public event EventHandler<CommandReceivedEventArgs>? CommandReceived;

        public ResponseSignOn SignOnResponse { get => _signOnResponse!; set => _signOnResponse = value; }
        public TargetState TargetMcuState { get; set; }

        public bool ForceSuccess { get; set; } = true;

        public ICommandResult ClearBreakpoint(int Index, ulong Breakpoint) => throw new NotImplementedException();
        public ICommandResult ClearEvents() => (CommandResult)ForceSuccess;
        public ICommandResult EnterPrograming() => throw new NotImplementedException();
        public ICommandResult EraseDevice() => throw new NotImplementedException();
        public ICommandResult EraseMemory(int MemType, ulong Address, ulong Length) => throw new NotImplementedException();
        public ICommandResult GetAllParameter() => (CommandResult)ForceSuccess;
        public ICommandResult GetBreakpoint(int Index, ulong Breakpoint, int BreakpointType, int BrakpointMode) => throw new NotImplementedException();
        public ICommandResult GetParameter(int paramId, out uint value)
        {
            value = 0;
            return (CommandResult)ForceSuccess;
        }
         
        public ICommandResult LeavePrograming() => throw new NotImplementedException();
        public bool ModifyAllParameter() => ForceSuccess;
        public ICommandResult ReadMemory(int memType, ulong Address, ulong Length, out byte Values) => throw new NotImplementedException();
        public ICommandResult ReadProgramCount(out ulong ProgramCounter) => throw new NotImplementedException();
        public ICommandResult Reconnect() => throw new NotImplementedException();
        public ICommandResult Reset() => (CommandResult)ForceSuccess;
        public ICommandResult SetAllParameter() => (CommandResult)ForceSuccess;
        public ICommandResult SetBreakpoint(int index, ulong Breakpoint, int BreakpointType, int BrakpointMode) => throw new NotImplementedException();
        public ICommandResult SetDeviceDescriptor() => (CommandResult)ForceSuccess;
        public ICommandResult SetParameter(int paramId, uint value) => (CommandResult)ForceSuccess;
        public ICommandResult SignOff() => throw new NotImplementedException();
        public ICommandResult SignOn(out ResponseSignOn? response)
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
            return (CommandResult)ForceSuccess;
        }
        public ICommandResult StartRunning() => throw new NotImplementedException();
        public ICommandResult StartRunningUntil(ulong Breakpoint) => throw new NotImplementedException();
        public ICommandResult StepIn(ulong ProgramCounter) => throw new NotImplementedException();
        public ICommandResult StopRunning() => throw new NotImplementedException();
        public ICommandResult VerifiyPrograming() => throw new NotImplementedException();
        public ICommandResult WriteMemory(int MemType, ulong Address, byte Values) => throw new NotImplementedException();
        public ICommandResult WriteProgramCount(ulong ProgramCounter) => throw new NotImplementedException();
        public ICommandResult WritePrograming(ulong Address, byte Values) => throw new NotImplementedException();
        public ICommandResult GetSync() => throw new NotImplementedException();
        public ICommandResult WriteMemory(int MemType, ulong Address, byte[] Values) => throw new NotImplementedException();
        public ICommandResult ReadMemory(int memType, ulong Address, ulong Length, out byte[] Values) => throw new NotImplementedException();
    }
}
