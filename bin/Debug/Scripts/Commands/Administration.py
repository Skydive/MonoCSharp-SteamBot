
def OnLoaded(pcf):
        pcf.AddCommand("Login", "LoginCommand", 0, "Login Args: password");

UserPasses = dict();
UserPasses["azetyfmk"] = "Owner";
UserPasses["iloveskydive"] = "SuperAdmin";
UserPasses["qwerty"] = "Admin";

AccessLevels = dict();
AccessLevels["Owner"] = 3;
AccessLevels["SuperAdmin"] = 2;
AccessLevels["Admin"] = 1;

def LoginCommand(instigator, args):
	if(args.Length >= 1):
		bSuccess = False;
		for key in UserPasses.keys():
			if(args[0].lower() == key.lower()):
				instigator.SendMessage("You are now one of the "+UserPasses[key]+"s!");
				instigator.AccessLevel = AccessLevels[UserPasses[key]];
				bSuccess = True;
		if(not bSuccess):
			instigator.SendMessage("Invalid Password!");
	else:
		if(instigator.AccessLevel > 0):
			instigator.AccessLevel = 0;
			instigator.SendMessage("You've successfully logged out!");
		else:
			instigator.SendMessage("One argument is required. (password)");
        

