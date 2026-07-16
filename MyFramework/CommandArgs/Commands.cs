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
    public static class Commands
    {


        #region Constructors 
        static Commands()
        {
            new CommandHelp();
            new CommandExit();
            new CommandClrscr();
        }

        #endregion


        #region Fields 
        /// <summary>
        /// 
        /// </summary>
        private static Dictionary<string, ICommandArgs> _commands = new Dictionary<string, ICommandArgs>();

        #endregion


        #region Properties 

        public static ILog Logger => LogManager.GetLogger(typeof(Commands));

        public static TextWriter Error => Console.Error;

        public static IReadOnlyDictionary<string, ICommandArgs> CommandList => _commands;

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
        public static void RegisterCommand(ICommandArgs command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            if (_commands.ContainsKey(command.Name))
                throw new ArgumentException($"Command with name '{command.Name}' is already registered.");

            Logger.Debug($"Registering command: {command.Name}");
            _commands.Add(command.Name.ToLower(), command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static ICommandArgs? FindCommand(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Command name cannot be null or whitespace.", nameof(name));
            _commands.TryGetValue(name.ToLower(), out var command);
            return command;
        }

        internal static void Quit()
        {
            Environment.Exit(0);
        }

        public static void RunConsole(string[] appArgs)
        {
            bool run = true;

            while (run)
            {
                Console.WriteLine();
                Console.Write("=>");
                string? line = Console.ReadLine();
                string[] lineArg = line!.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (lineArg.Length >= 1)
                {
                    if (_commands.ContainsKey(lineArg[0].ToLower()))
                    {
                        try
                        {
                            if (_commands[lineArg[0].ToLower()].Parse(lineArg))
                            {
                                Console.WriteLine($"Command '{lineArg[0]}' executed successfully.");
                            }
                        }
                        catch (TargetInvocationException ex)
                        {
                            if (ex.InnerException != null)
                            {
                                Console.WriteLine(ex.InnerException.Message);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Unknown command: {lineArg[0]}");
                    }
                }
            }
        }

        internal static void ClearScreen()
        {
            Console.Clear();
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
