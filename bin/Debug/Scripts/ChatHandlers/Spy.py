
Handler = "Spy";
AccessLevel = 1;


Filter = "";
def OnMessaged(instigator, message):
    global Filter;
    if(message == "disable"):
        Filter = "";
    else:
        Filter = message.lower();
    instigator.SendMessage("Filter: '"+Filter+"'");
        
       
def OnAllMessaged(reciever, instigator, message):
    global Filter;
    if(reciever != instigator):
        if(Filter == "" or message.lower().find(Filter.lower()) != -1):
            reciever.SendMessage("{} -> {}".format(instigator.GetPersonaName(), message));
