using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentArgs;
using log4net.Core;

namespace MyFramework.CommandArgs
{
    public class CommandExit : CommandArgsBase<CommandExit>
    {


        #region Constructors 
        public CommandExit(CommandExecDelegate<CommandExit> commandExec) : base(_commandName, commandExec)
        {
            Create();
        }

        #endregion


        #region Fields 

        private const string _commandName = "Exit";


        #endregion


        #region Properties 

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

                /* 3) Positional parameters. Parsing is done in the defined ordering. */
                .PositionalArgument<string>()
                    .WithValidation(a => String.Equals(a, _commandName, StringComparison.CurrentCultureIgnoreCase))
                    .WithExamples(_commandName)
                    .WithDescription($"Exit application.")
                    .IsRequired()

                /* 4) Load remaining arguments */

                /* 5) Callback */
                .Call(pos =>
                {
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
