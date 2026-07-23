namespace MyFramework.CommandArgs
{
    public interface ICommands
    {
        IReadOnlyDictionary<string, ICommandArgs> CommandList { get; }
        TextWriter Error { get; }

        ICommandArgs? FindCommand(string name);
        void RegisterCommand(ICommandArgs command);
    }
}