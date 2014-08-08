def OnLoaded(pcf):
    pcf.AddCommand("ChatHandler", "ChatHandler", 0, "Sets the chat handler. args: name");
    
def ChatHandler(instigator, args):
    if(args.Length >= 1):
        SteamBot = instigator.SteamInstance.SteamBotInstance;
        SteamBot.ResponseHandlerInstance.RegisterChatHandlers();
	bSuccess = False;
        for handler in SteamBot.ResponseHandlerInstance.PythonChatHandlers:
            if(args[0].lower() == handler.HandlerName.lower()):
                instigator.SendMessage("ChatHandler: "+handler.HandlerName);
                instigator.ChatHandler = handler.Clone();
                bSuccess = True;
                break;
	if(not bSuccess):
            instigator.SendMessage("Invalid ChatHandler!");
    else:
        instigator.ChatHandler = None;
        instigator.SendMessage("You've successfully disabled your chat handler!");        
