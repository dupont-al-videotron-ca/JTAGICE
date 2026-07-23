using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentArgs;
using log4net.Core;

namespace MyFramework.CommandArgs
{
    public class CommandHelp : CommandArgsBase<CommandHelp>
    {
        #region Constructors 
        public CommandHelp(CommandExecDelegate<CommandHelp> commandExec) : base(_commandName, commandExec)
        {
            Create();
        }


        #endregion


        #region Fields 

        private const string _commandName = "Help";

        #endregion


        #region Properties 

        public bool All { get; private set; } = false;
        public bool HelpOnHelp { get; private set; } = true;

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        #endregion


        #region Protected Methods 

        protected override void Create()
        {

            /* 1) General parser configurations: The ordering does not matter */
            var parsable = ApplyCommonConfig(FluentArgsBuilder.New())

                /* 2) Parse parameters, list-parameters, flags and commands. Parsing is done in the defined ordering. */
                .Flag("-all")
                    .WithDescription($"Display help of all commands.")
                .Flag("-h", "-help")
                    .WithDescription($"Display help on help command.")

                /* 3) Positional parameters. Parsing is done in the defined ordering. */
                .PositionalArgument<string>()
                    .WithValidation(a => String.Equals(a,_commandName,StringComparison.CurrentCultureIgnoreCase))
                    .WithExamples(_commandName, _commandName + " -all", _commandName + " -h", _commandName + " -help")
                    .WithDescription($"Display help on command.")
                    .IsRequired()

                /* 4) Load remaining arguments */

                /* 5) Callback */
                .Call(pos => hoh => all => 
                {
                    All = all;
                    HelpOnHelp = hoh;
                })
                .Build();

            this.SetParsable(parsable);
        }

        #endregion

        #region Private Methods 

        #endregion

        #region Private Classes / Enum 

        #endregion

    }
}
