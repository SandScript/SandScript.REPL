namespace SandScript.REPL;

[AttributeUsage( AttributeTargets.Method, AllowMultiple = true )]
public class ReplCommandAttribute : Attribute
{
	public readonly string CommandName;

	public ReplCommandAttribute( string commandName )
	{
		CommandName = commandName;
	}
}
