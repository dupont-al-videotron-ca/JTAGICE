using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentArgs;
using log4net.Core;

namespace MyFramework.CommandArgs
{
    internal class CommandClrscr : CommandArgsBase
    {

        #region Constructors 

        #endregion


        #region Fields 

        private const string _commandName = "Clrscr";

        public CommandClrscr() : base(_commandName)
        {
            Create();
        }

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

                /* 3) Positional parameters. Parsing is done in the defined ordering. */
                .PositionalArgument<string>()
                    .WithValidation(a => String.Equals(a, _commandName, StringComparison.CurrentCultureIgnoreCase))
                    .WithExamples(_commandName)
                    .WithDescription($"Clear screen.")
                    .IsRequired()

                /* 4) Load remaining arguments */

                /* 5) Callback */
                .Call(pos => 
                {
                    Commands.ClearScreen();
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
