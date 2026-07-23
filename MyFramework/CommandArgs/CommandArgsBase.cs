using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using FluentArgs;
using FluentArgs.Help;
namespace MyFramework.CommandArgs
{
    /// <summary>
    /// Base class for command arguments.
    /// </summary>
    public abstract class CommandArgsBase<T> : ICommandArgs<T> where T : class, ICommandArgs<T>
    {

        #region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandArgsBase"/> class.
        /// </summary>
        /// <param name="parsable">Arguments parser.</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected CommandArgsBase(string cmdName, IParsable parsable, CommandExecDelegate<T> cmdExecution)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(cmdName);
            ArgumentNullException.ThrowIfNull(parsable);
            ArgumentNullException.ThrowIfNull(cmdExecution);
            this.Name = cmdName;

            SetParsable(parsable);
            CommandExec = cmdExecution;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandArgsBase"/> class.
        /// </summary>
        /// <param name="parsable">Arguments parser.</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected CommandArgsBase(string cmdName, CommandExecDelegate<T> cmdExecution)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(cmdName);
            ArgumentNullException.ThrowIfNull(cmdExecution);
            this.Name = cmdName;

            CommandExec = cmdExecution;
        }

        #endregion


        #region Fields 
        

        #endregion


        #region Properties 


        public string Name { get; }

        public IParsable Parsable { get; private set; } = null!;

        public CommandExecDelegate<T> CommandExec { get; private set; } = null!;

        #endregion

        #region Delegates / Events 

        #endregion


        #region Public Methods 

        protected abstract void Create();

        protected void SetParsable(IParsable parsable)
        {
            this.Parsable = parsable ?? throw new ArgumentNullException(nameof(parsable));
            Commands.Instance.RegisterCommand(this);
        }

        protected IInitialFluentArgsBuilder ApplyCommonConfig(IInitialFluentArgsBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder
                /* 1) General parser configurations: The ordering does not matter */
                .DefaultConfigs()
                .RegisterHelpPrinter(new SimpleHelpPrinter(Commands.Instance.Error))
                .RegisterParsingErrorPrinter(new SimpleParsingErrorPrinter(Commands.Instance.Error));

            return builder;
        }

        public bool Execute(string[] args)
        {
            if (CommandExec != null && Parsable != null && this.Parsable.Parse(args))
            {
                return CommandExec((this as T)!);
            }
            else return false;
        }

        public async Task<bool> ParseAsync(string[] args)
        {
            if (args == null) throw new ArgumentNullException("args");

            if (Parsable != null)
            {
                var result = await this.Parsable.ParseAsync(args);
                return result;
            }
            else
                return false;
        }

        public bool Parse(string[] args)
        {
            if (Parsable != null)
                return this.Parsable.Parse(args);
            else return false;
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
