using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentArgs;

namespace MyFramework.CommandArgs
{
    public interface ICommandArgs
    {
        string Name { get; }

        IParsable Parsable { get; }

        bool Parse(string[] args);

        Task<bool> ParseAsync(string[] args);
    }
}
