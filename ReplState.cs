namespace SandScript.REPL;

public class ReplState
{
	public bool ShouldQuit = false;
	public readonly Script Script = new();

	public ReplState()
	{
		Script.AddClassMethods<Program>();
		Script.AddClassVariables<Program>();
	}
}
