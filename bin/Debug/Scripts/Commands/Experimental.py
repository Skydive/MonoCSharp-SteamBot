from math import *

def OnLoaded(pcf):
    pcf.AddCommand("Math", "Math", 0, "Does Math. Args: eval string");


def Math(instigator, args):
	if(args.Length >= 1):
		Output = "";
		try:
			Output = eval(str(args[0]));
		except:
			instigator.SendMessage(sys.exc_info()[0]);
			return;
		instigator.SendMessage("Output: "+str(Output));
	else:
		instigator.SendMessage("One argument is required. (eval string)");
        
