using System;
using System.Collections.Generic;
using SteamKit2;

public class SteamBot
{
	Steam SteamInstance;
    public ResponseHandler ResponseHandlerInstance;
    string Username, Password;
    public LinkedList<FriendContainer> FriendContainers = new LinkedList<FriendContainer>();

	public SteamBot(string username, string password)
	{
        ResponseHandlerInstance = new ResponseHandler();
        ResponseHandlerInstance.SteamBotInstance = this;
        SteamInstance = new Steam();
        SteamInstance.SteamBotInstance = this;
        Username = username;
        Password = password;

        SteamInstance.Login(username, password);
	}

    public void FriendStateChanged(SteamFriends.PersonaStateCallback callback){}

    public void FriendMessaged(SteamFriends.FriendMsgCallback callback)
    {
        if (callback.EntryType == EChatEntryType.ChatMsg)
        {
            Console.WriteLine(SteamInstance.Friends.GetFriendPersonaName(callback.Sender) + ": " + callback.Message);
            FriendContainer Sender = FindFriend(callback.Sender);
            if (callback.Message.StartsWith("!"))
            {
                ResponseHandlerInstance.RawCommandIssued(Sender, callback.Message.Substring(1));
                return;
            }

			ResponseHandlerInstance.HandleMessage(Sender, callback.Message);
        }
    }

    public FriendContainer FindFriend(SteamID ID)
    {
        FriendContainer foundfriend = null;
        foreach (FriendContainer friend in FriendContainers)
        {
            if (friend.SteamIDString == ID.ToString())
            {
                foundfriend = friend;
            }
        }
        return foundfriend;
    }

    public void GenerateFriend(SteamID ID)
    {
        bool bExists = false;
        foreach (FriendContainer friend in FriendContainers)
        {
            if (friend.ID == null)
            {
                FriendContainers.Remove(friend);
            }
            if (friend.ID == ID)
            {
                bExists = true;
            }
        }

        if (!bExists)
        {
            Console.WriteLine("Generated Friend: " + ID.ToString());
            FriendContainer Friend = new FriendContainer();
            Friend.SteamInstance = SteamInstance;
            Friend.SteamIDString = ID.ToString();
            Friend.ID = ID;
            FriendContainers.AddFirst(Friend);
        }
    }
}

