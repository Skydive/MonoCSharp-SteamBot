#define EXTERNAL_LOGIN

using System;
using System.IO;



public class MainClass
{

	void Start(string[] args)
	{
		#if EXTERNAL_LOGIN
            StreamReader file = File.OpenText("../../../SteamBotLoginData.txt");
            string[] Data = file.ReadToEnd().Split('\n');
            ExecuteSteamBot(Data[0], Data[1]);
            return;
        #endif
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


