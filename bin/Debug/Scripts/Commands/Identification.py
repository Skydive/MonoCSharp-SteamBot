def OnLoaded(pcf):
    pcf.AddCommand("Status", "Status", 0, "Outputs data of player states. Args: Filter");
    pcf.AddCommand("SteamID", "SteamID", 0, "Outputs data of player IDs. Args: filter, message, silent");
   
def Status(instigator, args):
	SteamBot = instigator.SteamInstance.SteamBotInstance;
	Friends = SteamBot.FriendContainers;
	FilterList = [];
	for friend in Friends:
		if(args.Length == 1):
			if(friend.GetPersonaName().lower().find(args[0].lower()) != -1):
				FilterList.append(friend);
		else:
			FilterList.append(friend);
	PersonaNames = list();
	PersonaStates = list();
	for friend in FilterList:
		PersonaNames.append(friend.GetPersonaName());
		PersonaStates.append(friend.GetPersonaState().ToString());
	instigator.SendTable("Person", "State", List[str](PersonaNames), List[str](PersonaStates));
        
def SteamID(instigator, args):
	SteamBot = instigator.SteamInstance.SteamBotInstance;
	Friends = SteamBot.FriendContainers;
	FilterList = [];
	for friend in Friends:
		if(args.Length == 1):
			if(friend.GetPersonaName().lower().find(args[0].lower()) != -1):
				FilterList.append(friend);
		else:
			FilterList.append(friend);
	PersonaNames = list();
	PersonaStates = list();
	for friend in FilterList:
		PersonaNames.append(friend.GetPersonaName());
		PersonaStates.append(friend.SteamIDString);
	instigator.SendTable("Person", "State", List[str](PersonaNames), List[str](PersonaStates));
        
