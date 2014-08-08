import clr
from System import Environment

clr.AddReferenceToFileAndPath(Environment.CurrentDirectory+"\Scripts\ChatHandlers\ChatterBotAPI.dll")

import ChatterBotAPI
from System.Collections.Generic import List

Handler = "Cleverbot"
AccessLevel = 0;


Session = None;
def OnMessaged(instigator, message):
    global Session;
    if(Session == None):
        Factory = ChatterBotAPI.ChatterBotFactory();
        CleverBot = Factory.Create(ChatterBotAPI.ChatterBotType.CLEVERBOT);
        Session = CleverBot.CreateSession();
    Response = Session.Think(message);
    instigator.SendMessage(Response);
    
