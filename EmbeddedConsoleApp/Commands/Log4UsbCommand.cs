using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentArgs;
using FluentArgs.Help;
using log4net.Core;
using Log4UsbService;
using MyFramework.CommandArgs;

namespace EmbeddedConsoleApp.Commands
{
    internal class Log4UsbCommand : MyFramework.CommandArgs.CommandArgsBase<Log4UsbCommand>
    {

        #region Constructors 
        public Log4UsbCommand(CommandExecDelegate<Log4UsbCommand> commandExec) : base(_commandName, commandExec)
        {
            Create();
        }

        #endregion


        #region Fields 

        //private const string _commandName = "log4usb";
        private const string _commandName = "l";

        #endregion


        #region Properties 

        public Level Level { get; private set; } = Level.Off;


        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        /// <summary>
        /// Creates a new instance of the <see cref="Log4UsbCommand"/> with configured argument parsing.
        /// </summary>
        /// <returns>A new <see cref="Log4UsbCommand"/> instance configured with:
        /// <list type="bullet">
        /// <item><description>Help flag support via -h</description></item>
        /// <item><description>Optional level parameter (-l) with default value of <see cref="Level.Off"/></description></item>
        /// <item><description>Disallowed unused arguments validation</description></item>
        /// <item><description>Help and parsing error output directed to Console.Error</description></item>
        /// </list>
        /// </returns>
        protected override void Create()
        {

                /* 1) General parser configurations: The ordering does not matter */
            var parsable = ApplyCommonConfig(FluentArgsBuilder.New())

                /* 2) Parse parameters, list-parameters, flags and commands. Parsing is done in the defined ordering. */
                .Parameter<string>("-l", "-level")
                    .WithExamples(Level.Off.Name, Level.Debug.Name, Level.Info.Name, Level.Warn.Name, Level.Error.Name, Level.Fatal.Name, Level.All.Name)
                    .WithDescription($"The log level to use for embedded logging. Default is {Level.Off.Name}.")
                    .WithValidation(a => a != null, $"The log level must be one of the following: {Level.Off.Name}, {Level.Debug.Name}, {Level.Info.Name}, {Level.Warn.Name}, {Level.Error.Name}, {Level.Fatal.Name}, {Level.All.Name}.")
                    .IsOptionalWithDefault(Level.Off.Name)

                /* 3) Positional parameters. Parsing is done in the defined ordering. */
                .PositionalArgument<string>()
                    .WithValidation(a => a == _commandName)
                    //.WithExamples(_commandName)
                    .WithDescription($"Use '{_commandName}' to activate or disable embedded logging.")
                    .IsRequired()

                /* 4) Load remaining arguments */
                //.LoadRemainingArguments()


                /* 5) Callback to save args*/
                .Call(PositionArg => LevelArg => 
                {
                    SetLevel(LevelArg);
                })
                .Build();

            this.SetParsable(parsable);
        }

        private void SetLevel(string levelArg)
        {
            switch (levelArg.ToLower())
            {
                case "off":
                    this.Level = Level.Off;
                    break;
                case "debug":
                    this.Level = Level.Debug;
                    break;
                case "info":
                    this.Level = Level.Info;
                    break;
                case "warn":
                    this.Level = Level.Warn;
                    break;
                case "error":
                    this.Level = Level.Error;
                    break;
                case "fatal":
                    this.Level = Level.Fatal;
                    break;
                case "all":
                    this.Level = Level.All;
                    break;
                default:
                    throw new ArgumentException($"Invalid log level: {levelArg}");
            }
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
