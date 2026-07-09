using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Test.Xunit;
using JTAGICEmkII;
using JTAGICEmkII.Adaptor;
using JTAGICEmkII.HostService;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;
using log4net;
using MyFramework;
using MyUsbDevice.JTAGICEmkII;
using Xunit;

namespace JTAGICEmkIITest
{

    public class HostServiceUsbLiveTest : HostServiceBaseTest
    {
        public HostServiceUsbLiveTest() : base()
        {
            _timeoutOccured = false;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

            base.Dispose(disposing);
        }

        [Fact]
        public void HostServiceLive_Constructor()
        {
            var test = CreateHostService();
            Assert.NotNull(test);
            Assert.True(true);
        }


        [Fact]
        public void HostServiceLive_shall_Initialise()
        {

            //--- Setup
            var hostService = CreateHostService();

            //--- Expectations

            //--- Action
            var result = hostService.Initialise();

            //--- Verification
            Assert.True(result);


        }


        [Fact]
        public void HostServiceLive_Calling_SignOff_shall_return_success()
        {

            //-- Setup
            var test = CreateHostService();
            test.TargetMcuState.GoStopped();

            //-- Expectation

            //-- Action
            var result = test.SignOff();

            //-- Verification
            CheckResult(result);
        }

        [Fact]
        public void HostServiceLive_Calling_SingOn_shall_return_success()
        {

            //-- Setup
            var test = CreateHostService();


            //-- Expectation
            var expectedResponse = new ResponseSignOn(SlaveResponseEnum.RSP_SIGN_ON)
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

            //-- Action
            var result = test.SignOn(out ResponseSignOn? response);

            //-- Verification
            CheckResult(result);
            Assert.NotNull(response);
            Assert.Equal(expectedResponse.CommunicationProtocolVersion, response.CommunicationProtocolVersion);
            Assert.Equal(expectedResponse.MasterMcuBootLoaderVersion, response.MasterMcuBootLoaderVersion);
            Assert.Equal(expectedResponse.MasterMcuHwVersion, response.MasterMcuHwVersion);
            Assert.Equal(expectedResponse.MasterMcuFirmwareVersionMajor, response.MasterMcuFirmwareVersionMajor);
            Assert.Equal(expectedResponse.MasterMcuFirmwareVersionMinor, response.MasterMcuFirmwareVersionMinor);
            Assert.Equal(expectedResponse.SlaveMcuBootLoaderVersion, response.SlaveMcuBootLoaderVersion);
            Assert.Equal(expectedResponse.SlaveMcuFirmwareVersionMajor, response.SlaveMcuFirmwareVersionMajor);
            Assert.Equal(expectedResponse.SlaveMcuFirmwareVersionMinor, response.SlaveMcuFirmwareVersionMinor);
            Assert.Equal(expectedResponse.SlaveMcuHwVersion, response.SlaveMcuHwVersion);
            Assert.Equal(expectedResponse.SerialNumber, response.SerialNumber);
            Assert.Equal(expectedResponse.DeviceId, response.DeviceId);
        }

        //[Fact]
        //public void HostServiceLive_Calling_SetParameter_shall_return_success()
        //{

        //    //-- Setup
        //    var test = CreateHostService();
        //    test.TargetMcuState.GoStopped();

        //    //-- Expectation

        //    //-- Action
        //    var result = test.SetParameter(0, 55);

        //    //-- Verification
        //    CheckResult(result);

        //}

        //[Fact]
        //public void HostServiceLive_Calling_SetParameter_ForAllParameters_shall_return_success()
        //{
        //    //-- Setup
        //    var test = CreateHostService();
        //    Parameters expectedParameters = BuildParametersForTest();
        //    test.TargetMcuState.GoStopped();
        //    //-- Expectation

        //    foreach (var param in expectedParameters.GetAllWrite())
        //    {
        //        //-- Action
        //        Logger.Debug($"Testing GetParameter for {param.Key} which has expected value {param.Value.Value} Size {param.Value.Size}. ");
        //        var result = test.SetParameter(param.Key, param.Value.Value);

        //        //-- Verification
        //        CheckResult(result);
        //    }

        //}

        //[Fact]
        //public void HostServiceLive_Calling_GetParameter_shall_return_success()
        //{
        //    //-- Setup
        //    var test = CreateHostService();
        //    var expectedParameters = new Parameters().GetAllRead();
        //    var expectedParameter = expectedParameters.First();
        //    test.TargetMcuState.GoStopped();
        //    //-- Expectation

        //    //-- Action
        //    var result = test.GetParameter(expectedParameter.Key, out uint param);

        //    //-- Verification
        //    CheckResult(result);
        //    Assert.Equal((uint)0xFE, param);
        //}

        //[Fact]
        //public void HostServiceLive_Calling_WriteMemory_shall_return_success()
        //{

        //    //-- Setup
        //    var test = CreateHostService();
        //    test.TargetMcuState.GoStopped();


        //    //-- Expectation

        //    //-- Action
        //    var result = test.WriteMemory(0, 0x12345, new byte[] { 0x01, 0x02, 0x03, 0x04 });

        //    //-- Verification
        //    CheckResult(result);

        //}

        //[Fact]
        //public void HostServiceLive_Calling_ReadMemory_shall_return_success()
        //{

        //    //-- Setup
        //    var test = CreateHostService();
        //    test.TargetMcuState.GoStopped();


        //    //-- Expectation
        //    byte[] expectedData = new byte[] { 0x00, 0x01, 0x02, 0x03 };

        //    //-- Action
        //    var result = test.ReadMemory(0, 0x12345, 4, out byte[] data);

        //    //-- Verification
        //    CheckResult(result);
        //    Assert.Equal(expectedData, data);

        //}

        //[Fact]
        //public void HostServiceLive_Calling_WritePc_shall_return_success()
        //{

        //    //-- Setup
        //    var test = CreateHostService();
        //    test.TargetMcuState.GoStopped();


        //    //-- Expectation

        //    //-- Action
        //    var result = test.WriteProgramCounter(0x12345);

        //    //-- Verification
        //    CheckResult(result);

        //}

        //[Fact]
        //public void HostServiceLive_Calling_ReadPc_shall_return_success()
        //{

        //    //-- Setup
        //    var test = CreateHostService();
        //    test.TargetMcuState.GoStopped();


        //    //-- Expectation
        //    ulong expectedPcValue = 0x12345678;

        //    //-- Action
        //    var result = test.ReadProgramCounter(out ulong pc);

        //    //-- Verification
        //    CheckResult(result);
        //    Assert.Equal(expectedPcValue, pc);

        //}

        //[Fact]
        //public void HostServiceLive_Calling_StartRunning_shall_return_success()
        //{

        //    //-- Setup
        //    var test = CreateHostService();
        //    test.TargetMcuState.GoStopped();


        //    //-- Expectation

        //    //-- Action
        //    var result = test.StartRunning();

        //    //-- Verification
        //    CheckResult(result);

        //}

        //[Fact]
        //public void HostServiceLive_Calling_SingleStepIntoAsm_shall_return_success()
        //{

        //    //-- Setup
        //    var test = CreateHostService();
        //    test.TargetMcuState.GoStopped();


        //    //-- Expectation

        //    //-- Action
        //    var result = test.SingleStepIntoAsm();

        //    //-- Verification
        //    CheckResult(result);

        //}

        //[Fact]
        //public void HostServiceLive_Calling_SingleStepOverAsm_shall_return_success()
        //{

        //    //-- Setup
        //    var test = CreateHostService();
        //    test.TargetMcuState.GoStopped();


        //    //-- Expectation

        //    //-- Action
        //    var result = test.SingleStepOverAsm();

        //    //-- Verification
        //    CheckResult(result);

        //}

        //[Fact]
        //public void HostServiceLive_Calling_SingleStepOutAsm_shall_return_success()
        //{

        //    //-- Setup
        //    var test = CreateHostService();
        //    test.TargetMcuState.GoStopped();


        //    //-- Expectation

        //    //-- Action
        //    var result = test.SingleStepOutAsm();

        //    //-- Verification
        //    CheckResult(result);

        //}


        //[Fact]
        //public void HostServiceLive_Calling_StopRunning_shall_return_success()
        //{

        //    //-- Setup
        //    var test = CreateHostService();
        //    test.TargetMcuState.GoRunning();


        //    //-- Expectation

        //    //-- Action
        //    var result = test.StopRunning();

        //    //-- Verification
        //    CheckResult(result);
        //}

        //[Fact]
        //public void HostServiceLive_Calling_Reset_shall_return_success()
        //{

        //    //-- Setup
        //    var test = CreateHostService();
        //    test.TargetMcuState.GoRunning();


        //    //-- Expectation

        //    //-- Action
        //    var result = test.Reset();

        //    //-- Verification
        //    CheckResult(result);
        //    Assert.True(test.TargetMcuState.IsStopped);

        //}

        //[Fact]
        //public void HostServiceLive_Calling_SetDeviceDescriptor_shall_return_success()
        //{

        //    //-- Setup
        //    var test = CreateHostService();
        //    test.TargetMcuState.GoStopped();


        //    //-- Expectation

        //    //-- Action
        //    var result = test.SetDeviceDescriptor();

        //    //-- Verification
        //    CheckResult(result);

        //}

        //[Fact]
        //public void HostServiceLive_Calling_EraseMemory_shall_return_success()
        //{

        //    //-- Setup
        //    var test = CreateHostService();
        //    test.TargetMcuState.GoStopped();

        //    //-- Expectation

        //    //-- Action
        //    var result = test.EraseMemory(0xB0, 0x0, 0x10000);

        //    //-- Verification
        //    CheckResult(result);

        //}

        //[Fact]
        //public void HostServiceLive_Calling_GetSync_shall_return_success()
        //{

        //    //-- Setup
        //    var test = CreateHostService();


        //    //-- Expectation

        //    //-- Action
        //    var result = test.GetSync();

        //    //-- Verification
        //    CheckResult(result);

        //}

        //[Fact]
        //public void HostServiceLive_Calling_SelfTest_shall_return_success()
        //{

        //    //-- Setup
        //    var test = CreateHostService();
        //    test.TargetMcuState.GoStopped();


        //    //-- Expectation

        //    //-- Action
        //    var result = test.SelfTest();

        //    //-- Verification
        //    CheckResult(result);

        //}
        //[Fact]
        //public void HostServiceLive_Calling_SetBreakpoint_shall_return_success()
        //{

        //    //-- Setup
        //    var test = CreateHostService();
        //    test.TargetMcuState.GoStopped();

        //    //-- Expectation

        //    //-- Action
        //    var result = test.SetBreakpoint(0, 0x0045, 0, 0x03);

        //    //-- Verification
        //    CheckResult(result);

        //}

        //[Fact]
        //public void HostServiceLive_Calling_GetBreakpoint_shall_return_success()
        //{

        //    //-- Setup
        //    var test = CreateHostService();
        //    test.TargetMcuState.GoStopped();

        //    //-- Expectation

        //    //-- Action
        //    var result = test.GetBreakpoint(0, out int breakpointType, out int brakpointMode, out ulong bp);

        //    //-- Verification
        //    CheckResult(result);
        //    Assert.Equal((ulong)0x12345678, bp);
        //    Assert.Equal((int)BreakpointTypeEnumS.BKPT_PRG_MEMORY, breakpointType);
        //    Assert.Equal((int)BreakpointModeEnumS.BKPT_MODE_PROGRAM, brakpointMode);  
        //}

        //[Fact]
        //public void HostServiceLive_Calling_EraseDevice_shall_return_success()
        //{

        //    //-- Setup
        //    var test = CreateHostService();
        //    test.TargetMcuState.GoStopped();


        //    //-- Expectation

        //    //-- Action
        //    var result = test.EraseDevice();

        //    //-- Verification
        //    CheckResult(result);

        //}


        //[Fact]
        //public void HostServiceLive_Calling_EnterPrograming_shall_return_success()
        //{

        //    //-- Setup
        //    var test = CreateHostService();
        //    test.TargetMcuState.GoStopped();


        //    //-- Expectation

        //    //-- Action
        //    var result = test.EnterPrograming();

        //    //-- Verification
        //    CheckResult(result);

        //}


        //[Fact]
        //public void HostServiceLive_Calling_LeavePrograming_shall_return_success()
        //{

        //    //-- Setup
        //    var test = CreateHostService();
        //    test.TargetMcuState.GoProgramming();


        //    //-- Expectation

        //    //-- Action
        //    var result = test.LeavePrograming();

        //    //-- Verification
        //    CheckResult(result);
        //    Assert.True(test.TargetMcuState.IsProgramming);

        //}

        //[Fact]
        //public void HostServiceLive_Calling_ClearBreakpont_shall_return_success()
        //{

        //    //-- Setup
        //    var test = CreateHostService();
        //    test.TargetMcuState.GoStopped();


        //    //-- Expectation

        //    //-- Action
        //    var result = test.ClearBreakpoint(0, 0x03);

        //    //-- Verification
        //    CheckResult(result);

        //}

        //[Fact]
        //public void HostServiceLive_Calling_StartRunningUntil_shall_return_success()
        //{

        //    //-- Setup
        //    var test = CreateHostService();
        //    test.TargetMcuState.GoStopped();


        //    //-- Expectation

        //    //-- Action
        //    var result = test.StartRunningUntil(0x0123456);

        //    //-- Verification
        //    CheckResult(result);
        //}

        //[Fact]
        //public void HostServiceLive_Calling_SpiCommand_shall_return_success()
        //{

        //    //-- Setup
        //    var test = CreateHostService();
        //    test.TargetMcuState.GoStopped();

        //    byte[] spi = new byte[] { 0x01, 0x02, 0x03, 0x04 };


        //    //-- Expectation

        //    //-- Action
        //    var result = test.SpiCommand(spi, out byte data);

        //    //-- Verification
        //    CheckResult(result);

        //}

        //[Fact]
        //public void HostServiceLive_Calling_ClearEvents_shall_return_success()
        //{

        //    //-- Setup
        //    var test = CreateHostService();
        //    test.TargetMcuState.GoStopped();


        //    //-- Expectation

        //    //-- Action
        //    var result = test.ClearEvents();

        //    //-- Verification
        //    CheckResult(result);

        //}

        //[Fact]
        //public void HostServiceLive_Calling_RestoreTarget_shall_return_success()
        //{

        //    //-- Setup
        //    var test = CreateHostService();


        //    //-- Expectation

        //    //-- Action
        //    var result = test.RestoreTarget();

        //    //-- Verification
        //    CheckResult(result);
        //    Assert.False(test.TargetMcuState.IsRunning);
        //    Assert.False(test.TargetMcuState.IsStopped);
        //    Assert.False(test.TargetMcuState.IsProgramming);

        //}

        protected override HostDeviceService CreateHostService(int timeout = -1, bool callInit = true)
        {
            JTAGICEmkIIDevice jTAGICEmkIIDevice = new JTAGICEmkIIDevice();
            Assert.True(jTAGICEmkIIDevice.Initialize());

            var jTAGICEInterface = jTAGICEmkIIDevice.JTAGICEInterface;


            var rxadapt = new RxUsbAdaptor(jTAGICEInterface.InPipe!);
            var txadapt = new TxUsbAdaptor(jTAGICEInterface.OutPipe!);
            _rxFrame = new RxFrame(rxadapt);
            _rxFrame.ResponseReceived += RxFrame_ResponseReceived;
            _rxFrame.CommandReceived += RxFrame_CommandReceived;
            _rxFrame.RxTimerExpired += RxFrame_RxTimerExpired;

            _txFrame = new TxFrame(txadapt);

            var hostService = new HostDeviceService(_rxFrame, _txFrame);
            _activityStructure = hostService.ActivityStructure;
            _hostService = hostService;

            if (callInit)
            {
                Assert.True(hostService.Initialise());

            }
            return hostService;
        }

    }
}
