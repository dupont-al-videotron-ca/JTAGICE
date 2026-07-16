using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentArgs;
using FluentArgs.Help;
namespace MyFramework.CommandArgs
{
    /// <summary>
    /// Base class for command arguments.
    /// </summary>
    public abstract class CommandArgsBase : ICommandArgs
    {

        #region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandArgsBase"/> class.
        /// </summary>
        /// <param name="parsable">Arguments parser.</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected CommandArgsBase(string cmdName, IParsable parsable)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(cmdName);
            ArgumentNullException.ThrowIfNull(parsable);
            this.Name = cmdName;
            this.Parsable = null!;

            SetParsable(parsable);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandArgsBase"/> class.
        /// </summary>
        /// <param name="parsable">Arguments parser.</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected CommandArgsBase(string cmdName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(cmdName);
            this.Name = cmdName;
            this.Parsable = null!;

        }
        #endregion


        #region Fields 

        #endregion


        #region Properties 


        public string Name { get; }

        public IParsable Parsable { get; private set; }

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        protected abstract void Create();

        protected void SetParsable(IParsable parsable)
        {
            this.Parsable = parsable ?? throw new ArgumentNullException(nameof(parsable));
            Commands.RegisterCommand(this);
        }

        protected IInitialFluentArgsBuilder ApplyCommonConfig(IInitialFluentArgsBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            builder
                /* 1) General parser configurations: The ordering does not matter */
                .DefaultConfigsWithAppDescription("toto")
                .RegisterHelpPrinter(new SimpleHelpPrinter(MyFramework.CommandArgs.Commands.Error))
                .RegisterParsingErrorPrinter(new SimpleParsingErrorPrinter(MyFramework.CommandArgs.Commands.Error));

            return builder;
        }

        public bool Parse(string[] args)
        {
            if (Parsable != null)
                return this.Parsable.Parse(args);
            else return false;
        }

        public async Task<bool> ParseAsync(string[] args)
        {
            if (args == null) throw new ArgumentNullException("args");

            if (Parsable != null)
            {
                await this.Parsable.ParseAsync(args);
                return true;
            }
            else
                return false;
        }

        #endregion


        #region Protected Methods 

        #endregion

        #region Private Methods 

        #endregion

        #region Private Classes / Enum 

        #endregion
    }
}
