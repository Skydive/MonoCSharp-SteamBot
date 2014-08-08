from System.Net import WebClient

def ParseXML(xml, key):
    KeyStrings = xml.split("<"+key+">");
    for i in range(len(KeyStrings)):
        FindEnd = KeyStrings[i].find("</"+key+">");
        if FindEnd > -1:
            KeyStrings[i] = KeyStrings[i][0:FindEnd];
        else:
            KeyStrings[i] = "";
    KeyStrings = [i for i in KeyStrings if i != '']
    return KeyStrings;

def OnLoaded(pcf):
    pcf.AddCommand("Translate", "Translate", 0, "Outputs translated message. Args: source, target, text");

def Translate(instigator, args):
    if(args.Length >= 3):
        XMLResponse = WebClient().DownloadString(QueryPath(args[2].replace(" ", "%20"), args[0].replace(" ", "%20"), args[1].replace(" ", "%20")));
        Output = ''.join(ParseXML(XMLResponse,"trans"));
        	
        instigator.SendMessage("Output: "+Output);
    else:
        instigator.SendMessage("Three arguments are required. (source, target, text)");
        
