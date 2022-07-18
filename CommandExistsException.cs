namespace SandScript.REPL;

public sealed class CommandExistsException : Exception
{
	public readonly string Command;

	public CommandExistsException( string command ) : base( "The command \"" + command + "\" already exists" )
	{
		Command = command;
	}
}
