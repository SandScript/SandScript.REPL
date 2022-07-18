namespace SandScript.REPL;

public class Program
{
	[ScriptVariable( "_version", CanRead = true, CanWrite = false )]
	public static string Version => "1.0.0";

	private static ReplState _state = new();

	public static void Main( string[] args )
	{
		CommandManager.RegisterTypeCommands<Program>();

		Console.WriteLine( "SandScript REPL " + Version );
		do
		{
			Console.Write( "> " );
			var input = Console.ReadLine();

			if ( string.IsNullOrWhiteSpace( input ) )
				continue;

			if ( input.StartsWith( CommandManager.CommandPrefix ) )
			{
				var firstSpace = input.IndexOf( ' ' );
				var cmd = input.Substring( 1, firstSpace == -1 ? input.Length - 1 : firstSpace - 1 );
				if ( !CommandManager.IsCommand( cmd ) )
				{
					Console.WriteLine( "Unrecognized command \"" + cmd + "\"" );
					continue;
				}

				var cmdArgs = input.Split( ' ' ).Skip( 1 ).ToArray();
				CommandManager.RunCommand( cmd, cmdArgs );
				continue;
			}

			var returnValue = _state.Script.Execute( input, out var diagnostics );
			if ( diagnostics.Errors.Count == 0 && returnValue is not null )
				Console.WriteLine( returnValue.Value?.ToString() );

			Console.ForegroundColor = ConsoleColor.Red;
			foreach ( var error in diagnostics.Errors )
				Console.WriteLine( error );

			Console.ForegroundColor = ConsoleColor.Yellow;
			foreach ( var warning in diagnostics.Warnings )
				Console.WriteLine( warning );

			Console.ForegroundColor = ConsoleColor.Blue;
			foreach ( var info in diagnostics.Informationals )
				Console.WriteLine( info );
		
			Console.ResetColor();
		} while ( !_state.ShouldQuit );
	}

	[ScriptMethod( "print" )]
	public static void Print( Script script, ScriptValue value ) =>
		Console.WriteLine( value.Value?.ToString() );

	[ReplCommand( "quit" )]
	public static void QuitCommand( string[] args )
	{
		_state.ShouldQuit = true;
		Console.WriteLine( "Bye!" );
	}

	[ReplCommand( "reset" )]
	public static void ResetCommand( string[] args )
	{
		_state = new ReplState();
		Console.WriteLine( "Done" );
	}
}
