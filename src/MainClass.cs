using System;
using System.IO;



public class MainClass
{

	void Start(string[] args)
	{
		if(args.Length >= 2) 
		{
            ExecuteSteamBot(args[0], args[1]);
		}
		else
		{
            Console.WriteLine("Enter Username: ");
            string TempUsername = Console.ReadLine();
            Console.WriteLine("Enter Password: ");
            string TempPassword = Console.ReadLine();
            ExecuteSteamBot(TempUsername, TempPassword);
		}

	}

	void ExecuteSteamBot(string username, string password)
	{
		SteamBot Bot = new SteamBot(username, password);
	}

	static void Main(string[] args)
	{
		MainClass main = new MainClass();
		main.Start(args);
	}
}


