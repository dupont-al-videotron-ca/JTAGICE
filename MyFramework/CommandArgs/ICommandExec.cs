using System;
using System.Collections.Generic;
using System.Text;

namespace MyFramework.CommandArgs
{
    public delegate bool CommandExecDelegate<T>(T args)
        where T : class, ICommandArgs;

    public interface ICommandExec<T> where T : class, ICommandArgs 
    {
        CommandExecDelegate<T>  CommandExec { get; }
    } 
}
