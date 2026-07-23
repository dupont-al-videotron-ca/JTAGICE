namespace MyFramework.CommandArgs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using log4net;
    using log4net.Repository.Hierarchy;

    /// <summary>
    /// 
    /// </summary>
    public sealed class Commands : ICommands
    {


        #region Constructors 

        public Commands()
        {
            _instance = this;
            new CommandHelp(this.ExecuteHelp);
            new CommandExit(this.ExecuteQuit);
            new CommandClrscr(this.ExecuteClearScreen);
        }

        #endregion


        #region Fields 
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, ICommandArgs> _registeredCommands = new Dictionary<string, ICommandArgs>();

        private static Commands? _instance;

        #endregion


        #region Properties 

        public static Commands Instance => _instance!;

        public ILog Logger => LogManager.GetLogger(typeof(Commands));

        public TextWriter Error => Console.Error;

        public IReadOnlyDictionary<string, ICommandArgs> CommandList => _registeredCommands;

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        /// <summary>
        /// Registers a command with the specified name and associated ICommandArgs instance.
        /// </summary>
        /// <param name="command"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void RegisterCommand(ICommandArgs command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            if (_registeredCommands.ContainsKey(command.Name.ToLower()))
                throw new ArgumentException($"Command with name '{command.Name}' is already registered.");

            Logger.Debug($"Registering command: {command.Name}");
            _registeredCommands.Add(command.Name.ToLower(), command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public ICommandArgs? FindCommand(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Command name cannot be null or whitespace.", nameof(name));
            CommandList.TryGetValue(name.ToLower(), out var command);
            return command;
        }

        #endregion


        #region Protected Methods 

        #endregion

        #region Private Methods 
        private bool ExecuteClearScreen(CommandClrscr cmd)
        {
            Console.Clear();
            return true;
        }

        private bool ExecuteQuit(CommandExit cmd)
        {
            Logger.Debug("Quitting application.");
            Environment.Exit(0);
            return true;
        }

        private bool ExecuteHelp(CommandHelp cmd)
        {
            if (cmd.All)
            {
                var commands = CommandList.Values.ToList();
                foreach (var command in commands)
                {
                    Console.WriteLine();
                    Console.WriteLine($"{command.Name}:");
                    command.Parsable.Parse(new string[] { "-h" });
                }
                return true;
            }

            else if (cmd.HelpOnHelp)
            {
                var cmdToHelp = FindCommand(cmd.Name);
                if (cmdToHelp != null)
                {
                    cmdToHelp.Parse(new string[] { "-h" });
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Private Classes / Enum 

        #endregion



    }

}
