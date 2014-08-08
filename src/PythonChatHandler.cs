using System;
using System.Collections.Generic;

using Microsoft.Scripting.Hosting;
using IronPython.Hosting;

// TODO: Create a better method of chat handler creation. (Involve Classes in IronPython?)

public class PythonChatHandler : ICloneable
{
	public ResponseHandler ParentHandler;
	public static ScriptEngine Engine;
	public ScriptScope Scope;
	public ScriptSource Source;
	
	public string HandlerName;
	public int AccessLevel;

	public PythonChatHandler()
	{
		Scope = Engine.CreateScope();
	}
	
	public void LoadFile(string path)
	{
		Source = Engine.CreateScriptSourceFromFile(path);
		Console.WriteLine("Loading: "+path);
		try
		{
			Source.Compile();
			Source.Execute(Scope);
			HandlerName = Scope.GetVariable<string>("Handler"); 
			AccessLevel = Scope.GetVariable<int>("AccessLevel");
		}
		catch(Exception e)
		{
			Console.WriteLine(e.ToString());
			/*Func<PythonTuple> exc_info = Engine.Operations.GetMember<Func<PythonTuple>>(Engine.GetSysModule(), "exc_info");
			PythonTuple pt = exc_info();
			for(int i=0; i<pt.Count; i++)
			{
				TraceBack tb = (TraceBack)pt[i];
				Console.WriteLine(tb.ToString());
			}*/
		}
	}
	
	public void ExecuteMessaged(FriendContainer instigator, string message)
	{
		if(Scope.ContainsVariable("OnMessaged"))
		{
			Action<FriendContainer, string> Function = Scope.GetVariable<Action<FriendContainer, string>>("OnMessaged");
			Function(instigator, message);
		}
	}

	public void ExecuteAllMessaged(FriendContainer client, FriendContainer instigator, string message)
	{
		if(Scope.ContainsVariable("OnAllMessaged"))
		{
			Action<FriendContainer, FriendContainer, string> Function = Scope.GetVariable<Action<FriendContainer, FriendContainer, string>>("OnAllMessaged");
			Function(client, instigator, message);
		}
	}
	
	public dynamic GetVariable(string name)
	{
		return Scope.GetVariable(name);
	}

	public PythonChatHandler Clone()
	{
		PythonChatHandler Cloned = new PythonChatHandler();
		Cloned.LoadFile(Source.Path);
		return Cloned;
	}

	object ICloneable.Clone()
	{
		return Clone();
	}
}
