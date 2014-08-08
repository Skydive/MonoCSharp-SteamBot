
def OnLoaded(pcf):
    pcf.AddCommand("Broadcast", "Broadcast", 2, "Sends a message to all players. Args: Message");
    pcf.AddCommand("Say", "Say", 1, "Sends a message to a player. Args: filter, message, silent");
    pcf.AddCommand("Message", "Say", 1, "Sends a message to a player. Args: filter, message, silent");
    pcf.AddCommand("Spam", "Spam", 2, "Spams a message to all players. Args: filter, message, count, silent");
    
def Broadcast(instigator, args):
    if(args.Length >= 1):
        SteamBot = instigator.SteamInstance.SteamBotInstance;
        Friends = SteamBot.FriendContainers;
        for friend in Friends:
            instigator.SendMessage("Message sent to -> "+friend.GetPersonaName());
            friend.SendMessage(args[0]);
    else:
        instigator.SendMessage("One argument is required. (message)");
        
def Say(instigator, args):
    if(args.Length >= 2):
        SteamBot = instigator.SteamInstance.SteamBotInstance;
        Friends = SteamBot.FriendContainers;

        for friend in Friends:
            if(friend.GetPersonaName().lower().find(args[0].lower()) != -1):
                instigator.SendMessage("Message sent to -> "+friend.GetPersonaName());
                if(args.Length == 3 and args[2] == "silent"):
                    friend.SendMessage(args[1]);
                else:
                    friend.SendMessage("{}: {}".format(instigator.GetPersonaName(), args[1]));
    else:
        instigator.SendMessage("Two arguments are required. (name, message, optional silent)");
        
def Spam(instigator, args):
    if(args.Length >= 3 and args[2].isnumeric()):
        SteamBot = instigator.SteamInstance.SteamBotInstance;
        Friends = SteamBot.FriendContainers;

        for friend in Friends:
            if(friend.GetPersonaName().lower().find(args[0].lower()) != -1):
                instigator.SendMessage(args[2]+" messages sent to -> "+friend.GetPersonaName());
                if(args.Length == 4 and args[3] == "silent"):
                    for i in range(int(args[2])):
                        friend.SendMessage(args[1]);
                else:
                    for i in range(int(args[2])):
                        friend.SendMessage("{}: {}".format(instigator.GetPersonaName(), args[1]));
    else:
        instigator.SendMessage("Three arguments are required. (name, message, count, optional silent)");
