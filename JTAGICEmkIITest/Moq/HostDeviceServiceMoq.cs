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
    public class HostDeviceServiceMoq : IHostDeviceService
    {

        public HostDeviceServiceMoq()
        {
            TargetMcuState = new TargetState();
        }

        private uint _breakpoint;
        private ResponseSignOn? _signOnResponse = null;
        private uint _programCounter;

        public event EventHandler<RequestEventArgs>? RequestCompleted;
        public event EventHandler<EventReceivedEventArgs>? EventReceived;
        public event EventHandler<RequestEventArgs>? RequestTimeout;
        public event EventHandler<ResponseReceivedEventArgs>? ResponseReceived;
        public event EventHandler<CommandReceivedEventArgs>? CommandReceived;

        public ResponseSignOn SignOnResponse { get => _signOnResponse!; set => _signOnResponse = value; }
        public TargetState TargetMcuState { get; set; }

        public bool ForceSuccess { get; set; } = true;

        public ICommandResult ClearBreakpoint(int Index, ulong Breakpoint)
        {
            _breakpoint = 0;
            return (CommandResult)ForceSuccess;
        }
        public ICommandResult ClearEvents() => (CommandResult)ForceSuccess;
        public ICommandResult EnterPrograming() => (CommandResult)ForceSuccess;
        public ICommandResult EraseDevice() => (CommandResult)ForceSuccess;
        public ICommandResult EraseMemory(int MemType, ulong Address, ulong Length) => (CommandResult)ForceSuccess;
        public ICommandResult GetAllParameter() => (CommandResult)ForceSuccess;
        public ICommandResult GetBreakpoint(int Index, int BreakpointType, int BrakpointMode, out ulong breakpoint)
        {
            breakpoint = _breakpoint;
            return (CommandResult)ForceSuccess;
        }
        public ICommandResult GetParameter(int paramId, out uint value)
        {
            value = 0;
            return (CommandResult)ForceSuccess;
        }

        public ICommandResult LeavePrograming() => (CommandResult) ForceSuccess;
        public bool ModifyAllParameter() => ForceSuccess;
        public ICommandResult ReadMemory(int memType, ulong Address, ulong Length, out byte Value)
        {
            Value = 0xAA;
            return (CommandResult)ForceSuccess;
        }
        public ICommandResult ReadProgramCounter(out ulong ProgramCounter)
        {
            ProgramCounter = _programCounter;
            return (CommandResult)ForceSuccess;
        }
        public ICommandResult Reconnect() => (CommandResult) ForceSuccess;
        public ICommandResult Reset() => (CommandResult)ForceSuccess;
        public ICommandResult SetAllParameter() => (CommandResult)ForceSuccess;
        public ICommandResult SetBreakpoint(int index, ulong Breakpoint, int BreakpointType, int BrakpointMode)
        {
            _programCounter = (uint)Breakpoint;
            return (CommandResult)ForceSuccess;
        }
        public ICommandResult SetDeviceDescriptor() => (CommandResult)ForceSuccess;
        public ICommandResult SetParameter(int paramId, uint value) => (CommandResult)ForceSuccess;
        public ICommandResult SignOff() => (CommandResult) ForceSuccess;
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
        public ICommandResult StartRunning() => (CommandResult) ForceSuccess;
        public ICommandResult StartRunningUntil(ulong Breakpoint) => (CommandResult) ForceSuccess;
        public ICommandResult StepIn(ulong ProgramCounter) => (CommandResult) ForceSuccess;
        public ICommandResult StopRunning() => (CommandResult) ForceSuccess;
        public ICommandResult VerifiyPrograming() => (CommandResult) ForceSuccess;
        public ICommandResult WriteMemory(int MemType, ulong Address, byte Values) => (CommandResult) ForceSuccess;
        public ICommandResult WriteProgramCounter(ulong ProgramCounter) => (CommandResult) ForceSuccess;
        public ICommandResult WritePrograming(ulong Address, byte Values) => (CommandResult) ForceSuccess;
        public ICommandResult GetSync() => (CommandResult) ForceSuccess;
        public ICommandResult WriteMemory(int MemType, ulong Address, byte[] Values) => (CommandResult) ForceSuccess;
        public ICommandResult ReadMemory(int memType, ulong Address, ulong Length, out byte[] Values)
        {
            Values = new byte[Length];
            for (ulong i = 0; i < Length; i++)
            {
                Values[i] = (byte)i;
            }

            return (CommandResult)ForceSuccess;
        }
        public ICommandResult SelfTest() => (CommandResult) ForceSuccess;
        public ICommandResult SpiCommand(byte[] send, out byte read)
        {
            read = 0x55;
            return (CommandResult)ForceSuccess;
        }
    }
}
