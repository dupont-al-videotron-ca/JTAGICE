using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    public sealed class TargetProgramming : ActivityBaseComp
    {
        public TargetProgramming(StructureActivity activityStructure) : this(activityStructure, null!)
        {
        }

        public TargetProgramming(StructureActivity activityStructure, IActivityElement? parent) : base(activityStructure, parent)
        {
        }

        public override bool Accept(IVisitorActivity visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit(this);
        }

        public ProgrammingOptions? PrgOption { get; set; }

        public override bool ActivityAction()
        {
            if(PrgOption == null)
            {
                Logger.Error("Invalid programming options: null.");
                throw new InvalidOperationException("Programming options cannot be null.");
            }

            if (!this.ActivityStructure.TargetMcuState.IsProgramming)
            {
                Logger.Debug("Entering programming mode...");
                if (!this.ActivityStructure.HostService.EnterPrograming().IsSuccess)
                {
                    GoStoppedFail();
                    PrgOption.ProgrammingResult = ProgrammingResultEnum.PR_MODE_FAIL;
                    Logger.Debug($"Programming mode failed: {PrgOption.ToString()}.");
                    return true;
                }
                else if (!this.ActivityStructure.TargetMcuState.IsProgramming)
                {
                    PrgOption.ProgrammingResult = ProgrammingResultEnum.PR_MODE_FAIL;
                    GoStoppedFail();
                    Logger.Debug($"Programming mode failed: {PrgOption.ToString()}.");
                    return true;
                }
            }

            if (this.PrgOption.IsEraseDestination)
            {
                UInt64 currentAddress = PrgOption.Address;
                while (currentAddress < PrgOption.Address + PrgOption.ByteCount)
                {
                    if (!this.ActivityStructure.HostService.EraseMemory((byte)PrgOption.MemoryType, currentAddress, PrgOption.PageSize).IsSuccess)
                    {
                        PrgOption.ProgrammingResult = ProgrammingResultEnum.PR_ERASE_FAIL;
                        PrgOption.ResultErrorAddress = currentAddress;
                        GoStoppedFail();
                        Logger.Debug($"Erase memory failed: {PrgOption.ToString()}.");
                        return true;
                    }
                    currentAddress += (UInt64)PrgOption.PageSize;
                }
            }

            if (this.PrgOption.IsProgrammDestination)
            {
                UInt64 currentAddress = PrgOption.Address;
                while (currentAddress < PrgOption.Address + PrgOption.ByteCount)
                {
                    byte[] buffer = PrgOption.Data.Skip((int)(currentAddress - PrgOption.Address)).Take((int)PrgOption.PageSize).ToArray();
                    if (!this.ActivityStructure.HostService.WriteMemory((byte)PrgOption.MemoryType, currentAddress, buffer).IsSuccess)
                    {
                        PrgOption.ProgrammingResult = ProgrammingResultEnum.PR_PROGRAM_FAIL;
                        PrgOption.ResultErrorAddress = currentAddress;
                        Logger.Debug($"Programming memory failed: {PrgOption.ToString()}.");
                        GoStoppedFail();
                        return true;
                    }
                    currentAddress += (UInt64)buffer.Length;
                }
            }

            if (this.PrgOption.IsVerifyDestination || this.PrgOption.IsProgrammDestination)
            {
                UInt64 currentAddress = PrgOption.Address;
                while (currentAddress < PrgOption.Address + PrgOption.ByteCount)
                {
                    var srcBuffer = PrgOption.Data.Skip((int)(currentAddress - PrgOption.Address)).Take((int)PrgOption.PageSize).ToArray();
                    if (!this.ActivityStructure.HostService.ReadMemory((byte)PrgOption.MemoryType, currentAddress, PrgOption.PageSize, out var readBuffer).IsSuccess)
                    {
                        PrgOption.ProgrammingResult = ProgrammingResultEnum.PR_VERIFY_FAIL;
                        PrgOption.ResultErrorAddress = currentAddress;
                        Logger.Debug($"Programming verify failed: {PrgOption.ToString()}.");
                        GoStoppedFail();
                        return true;
                    }
                    else
                    {
                        for (int i = 0; i < readBuffer.Length; i++)
                        {
                            if(readBuffer[i] != srcBuffer[+i])
                            {
                                PrgOption.ProgrammingResult = ProgrammingResultEnum.PR_VERIFY_FAIL;
                                PrgOption.ResultErrorAddress = currentAddress + (UInt64)i;
                                PrgOption.ExpectedData = srcBuffer[i];
                                PrgOption.ReadData = readBuffer[i];

                                Logger.Info($"Programming verify error: {PrgOption.ToString()}.");
                                GoLeaveFail();
                                return true;
                            }
                        }
                    }

                    currentAddress += (UInt64)PrgOption.PageSize;
                }
            }

            PrgOption.ProgrammingResult = ProgrammingResultEnum.PR_SUCCESS;
            Logger.Debug($"Programming successful: {PrgOption.ToString()}.");
            this.ActivityStructure.HostService.LeavePrograming();
            NextActivity = this.Find<TargetStopped>();
            return base.ActivityAction();
        }

        public override bool ActivityEntry()
        {
            this.ActivityStructure.TargetMcuState.GoProgramming();
            return base.ActivityEntry();
        }

        public override bool ActivityExit(bool lastRequest) => base.ActivityExit(lastRequest);

        private void GoStoppedFail()
        {
            this.ActivityStructure.HostService.GetSync();
            NextActivity = this.Find<TargetStopped>();
        }

        private void GoLeaveFail()
        {
            this.ActivityStructure.HostService.LeavePrograming();
            NextActivity = this.Find<TargetStopped>();
        }
    }
}
