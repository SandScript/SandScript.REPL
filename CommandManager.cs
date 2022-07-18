using System.Reflection;

namespace SandScript.REPL;

public static class CommandManager
{
	public const string CommandPrefix = "#";
	
	private static readonly Dictionary<string, MethodBase> Commands = new();

	public static bool IsCommand( string cmd ) => Commands.ContainsKey( cmd );

	public static void RunCommand( string cmd, string[] args )
	{
		if ( !IsCommand( cmd ) )
			return;

		Commands[cmd].Invoke( null, new object?[] {args} );
	}

	public static void RegisterTypeCommands<T>() where T : class
	{
		var methods = typeof(T).GetMethods()
			.Where( m => m.GetCustomAttributes( typeof(ReplCommandAttribute), false ).Length > 0 );

		foreach ( var method in methods )
		{
			var attribute = (ReplCommandAttribute)Attribute.GetCustomAttribute( method, typeof(ReplCommandAttribute) )!;
			if ( Commands.ContainsKey( attribute.CommandName ) )
				throw new CommandExistsException( attribute.CommandName );
			
			Commands.Add( attribute.CommandName, method );
		}
	}
}
