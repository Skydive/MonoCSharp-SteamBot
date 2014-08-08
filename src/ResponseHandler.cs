
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using System.Threading;


public class ResponseHandler
{
    public SteamBot SteamBotInstance;
	public LinkedList<PythonCommand> PythonCommands = new LinkedList<PythonCommand>();
	public LinkedList<PythonChatHandler> PythonChatHandlers = new LinkedList<PythonChatHandler>();
    public ResponseHandler()
    {
		PythonFile.Engine = IronPython.Hosting.Python.CreateEngine();
		PythonChatHandler.Engine = IronPython.Hosting.Python.CreateEngine();

		ICollection<string> paths = PythonFile.Engine.GetSearchPaths();
		paths.Add(Environment.CurrentDirectory+"\\Scripts\\Lib\\");
		PythonFile.Engine.SetSearchPaths(paths);
		PythonChatHandler.Engine.SetSearchPaths(paths);

		RegisterCommands();
		RegisterChatHandlers();
    }

    public void RegisterCommands()
    {
        PythonCommands.Clear();
        string[] files = Directory.GetFiles("./Scripts/Commands/", "*.py", SearchOption.AllDirectories);
        foreach(string path in files)
        {
			PythonFile command = new PythonFile();
            command.ParentHandler = this;
            command.LoadFile(path);
			command.ExecutePrecache();
        }
    }

    public void RawCommandIssued(FriendContainer instigator, string RawCommand)
    {
        string[] splitcommand = Regex.Split(RawCommand, "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*) (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

        string CommandName = splitcommand[0];

        List<string> parameters = new List<string>(splitcommand);
        parameters.RemoveAt(0);
        string[] CommandArgs = parameters.ToArray();
        if(CommandArgs.Length > 0)
        {
            for(int i=0; i<CommandArgs.Length; i++)
            {
				if (CommandArgs[i][0] == '\"' && CommandArgs[i][CommandArgs[i].Length-1] == '\"')
				{
					CommandArgs[i] = CommandArgs[i].Substring(1, CommandArgs[i].Length - 2);
				}
            }
        }
        CommandIssued(instigator, CommandName, CommandArgs);
    }

    public void CommandIssued(FriendContainer instigator, string commandname, string[] parameters)
    {
        bool bFound = false;
		RegisterCommands();
        foreach(PythonCommand command in PythonCommands)
        {
			Console.WriteLine(command.CommandName);
            if(command.CommandName.ToLower() == commandname.ToLower())
            {
                bFound = true;
                if(instigator.AccessLevel >= command.AccessLevel)
                {
					new Thread(new ThreadStart(() =>
					{
	                    try
						{
	                        command.Function(instigator, parameters);
	                    }
	                    catch(Exception e)
	                    {
	                        instigator.SendMessage(e.Source);
	                        instigator.SendMessage(e.Message);
	                        instigator.SendMessage(e.StackTrace);
	                    }
					})).Start();
                    break;
                }
                else
                {
                    instigator.SendMessage("You do not have permission to use this command!");
                }
            }
        }
        if(!bFound)
        {
            instigator.SendMessage("Unknown command!");
        }
    }

	public void RegisterChatHandlers()
	{
		PythonChatHandlers.Clear();
		string[] files = Directory.GetFiles("./Scripts/ChatHandlers/", "*.py", SearchOption.TopDirectoryOnly);
		foreach(string path in files)
		{
			PythonChatHandler command = new PythonChatHandler();
			command.ParentHandler = this;
			command.LoadFile(path);
			PythonChatHandlers.AddFirst(command);
		}
	}

	public void HandleMessage(FriendContainer sender, string message)
	{
		new Thread(new ThreadStart(() =>
		{
			foreach(FriendContainer reciever in SteamBotInstance.FriendContainers)
			{
				if(reciever.ChatHandler != null)
				{
					reciever.ChatHandler.ExecuteAllMessaged(reciever, sender, message);
				}
			}
			if(sender.ChatHandler != null)
			{
				sender.ChatHandler.ExecuteMessaged(sender, message);
		}
		})).Start();
	}
}

