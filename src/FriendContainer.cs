using System;
using System.Collections.Generic;
using SteamKit2;

public class FriendContainer
{
    public static string AvatarPrefix = "http://cdn.akamai.steamstatic.com/steamcommunity/public/images/avatars/";
    public static string AvatarSuffix = "_full.jpg";

	public Steam SteamInstance;
    public int AccessLevel = 0;
    public string SteamIDString;
    public SteamID ID;

	public PythonChatHandler ChatHandler;

    public string GetPersonaName()
    {
        if(SteamInstance != null && ID != null)
        {
            return SteamInstance.Friends.GetFriendPersonaName(ID);
        }
        return "NULL";
    }
    public EPersonaState GetPersonaState()
    {
        if(SteamInstance != null && ID != null)
        {
            return SteamInstance.Friends.GetFriendPersonaState(ID);
        }
        return EPersonaState.Offline;
    }
    public string GetAvatarLink()
    {
        if(SteamInstance != null && ID != null)
        {
            string SHA1 = BitConverter.ToString(SteamInstance.Friends.GetFriendAvatar(ID)).Replace("-","").ToLower();
            string PreURL = SHA1.Substring(1, 2);
            return AvatarPrefix + PreURL + "/" + SHA1 + AvatarSuffix;
        }
        return "NULL";
    }
    public void SendMessage(string message)
    {
        if (SteamInstance != null && ID != null)
        {
            SteamInstance.Friends.SendChatMessage(ID, EChatEntryType.ChatMsg, message);
        }
    }

	//TODO: Somehow fix SendTable()
	public void SendTable(string keyname, string valuename, List<string> k, List<string> v)
	{
		double FakeWidth = 1;
		string[] keys = k.ToArray();
		string[] values = v.ToArray();
		string Data = String.Format("{0} -> {1}\n", keyname, valuename);
		for(int i=0; i<keys.Length; i++)
		{
			Data += String.Format("{0} -> {1}\n", keys[i], values[i]);
		}


        SendMessage(".\n" + Data);
    }
}


