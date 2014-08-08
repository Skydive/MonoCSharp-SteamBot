using System;
using System.IO;

using SteamKit2;

public class Steam
{
    public SteamBot SteamBotInstance;
    public SteamClient Client;
    string UserN, Pass, Auth;

    private CallbackManager CallbackManagerInstance;
    // Lots of Handlers
    public SteamApps Apps;
    public SteamCloud Cloud;
    public SteamFriends Friends;
    public SteamGameCoordinator GameCoordinator;
    public SteamGameServer GameServer;
    public SteamMasterServer MasterServer;
    public SteamScreenshots Screenshots;
    public SteamUserStats Stats;
    public SteamTrading Trading;
    public SteamUser User;
    public SteamWorkshop Workshop;

    bool bActive;

	public void Login(string username, string password)
    {
        UserN = username;
        Pass = password;
        bActive = true;
        Client = new SteamClient();
        CallbackManagerInstance = new CallbackManager(Client);

        SetupHandlers();
        SetupCallBacks();

        Console.WriteLine("[ALERT] Connecting to Steam...");
		Client.Connect();
        MainLoop();
    }

    private void MainLoop()
    {
        while(bActive)
        {
            CallbackManagerInstance.RunWaitCallbacks(TimeSpan.FromSeconds (1));
        }
    }

    private void SetupHandlers()
    {
        Apps = Client.GetHandler<SteamApps>();
        Cloud = Client.GetHandler<SteamCloud>();
        Friends = Client.GetHandler<SteamFriends>();
        GameCoordinator = Client.GetHandler<SteamGameCoordinator>();
        GameServer = Client.GetHandler<SteamGameServer>();
        MasterServer = Client.GetHandler<SteamMasterServer>();
        Screenshots = Client.GetHandler<SteamScreenshots>();
        Stats = Client.GetHandler<SteamUserStats>();
        Trading = Client.GetHandler<SteamTrading>();
        User = Client.GetHandler<SteamUser>();
        Workshop = Client.GetHandler<SteamWorkshop>();
    }

    private void SetupCallBacks()
    {
        // On Connected
        new Callback<SteamUser.AccountInfoCallback>(UserAccountInfo, CallbackManagerInstance);

        // Friends!
        new Callback<SteamFriends.FriendAddedCallback>(FriendAdded, CallbackManagerInstance);
        new Callback<SteamFriends.FriendsListCallback>(FriendsList, CallbackManagerInstance);
        new Callback<SteamFriends.PersonaStateCallback>(FriendsPersonaState, CallbackManagerInstance);
        new Callback<SteamFriends.FriendMsgCallback>(FriendsMsg, CallbackManagerInstance);

        // Connection
        new Callback<SteamClient.ConnectedCallback>(ClientConnected, CallbackManagerInstance);
        new Callback<SteamClient.DisconnectedCallback>(ClientDisconnected, CallbackManagerInstance);
        new Callback<SteamUser.LoggedOnCallback>(UserLoggedOn, CallbackManagerInstance);
        new Callback<SteamUser.LoggedOffCallback>(UserLoggedOff, CallbackManagerInstance);
        new JobCallback<SteamUser.UpdateMachineAuthCallback>(UserMachineAuth, CallbackManagerInstance);
    }

    void UserAccountInfo(SteamUser.AccountInfoCallback callback)
    {
        Friends.SetPersonaState(EPersonaState.Online);
    }

    void FriendAdded(SteamFriends.FriendAddedCallback callback)
    {
        Friends.SendChatMessage(callback.SteamID, EChatEntryType.ChatMsg, "Hello! New friend.");
        SteamBotInstance.GenerateFriend(callback.SteamID);
    }

    void FriendsList(SteamFriends.FriendsListCallback callback)
    {
        int length = Friends.GetFriendCount();
        for (int i=0; i<length; i++)
        {
            SteamID friend = Friends.GetFriendByIndex(i);
            SteamBotInstance.GenerateFriend(friend);
        }
        foreach (var friend in callback.FriendList)
        {
            if (friend.Relationship == EFriendRelationship.RequestRecipient)
            {
                Friends.AddFriend(friend.SteamID);
            }
            //steamFriends.SendChatMessage(friend.SteamID, EChatEntryType.ChatMsg, "BROADCAST: I am Online! Peasants!");
        }
    }
    void FriendsPersonaState(SteamFriends.PersonaStateCallback callback)
    {
        SteamBotInstance.FriendStateChanged(callback);
    }
    void FriendsMsg(SteamFriends.FriendMsgCallback callback)
    {
        SteamBotInstance.FriendMessaged(callback);
    }
 





   

    #region Connection Handlers
    void ClientConnected(SteamClient.ConnectedCallback callback)
    {
        if (callback.Result != EResult.OK)
        {
            Console.WriteLine(String.Format("[ERROR] Unable to connect to Steam: {0}", callback.Result));
            bActive = false;
            return;
        }
        Console.WriteLine(String.Format("[ALERT] Connected to Steam! Logging in as '{0}'", UserN));
        User.LogOn(new SteamUser.LogOnDetails
        {
            Username = UserN,
            Password = Pass,
            AuthCode = null,
            SentryFileHash = null
        });

    }

    void ClientDisconnected(SteamClient.DisconnectedCallback callback)
    {
        Console.WriteLine("[Alert] Disconnected from Steam. Reconnecting...");
        Client.Connect();
    }

    void UserLoggedOn(SteamUser.LoggedOnCallback callback)
    {
        if (callback.Result == EResult.AccountLogonDenied)
        {
            Console.WriteLine("[Alert] This account is SteamGuard protected!");
            Console.Write("Please enter the auth code sent to the email at {0}: ", callback.EmailDomain);

            Auth = Console.ReadLine();
            return;
        }
        else if (callback.Result != EResult.OK)
        {
            Console.WriteLine(String.Format("[ERROR] Unable to logon to Steam: {0} / {1}", callback.Result, callback.ExtendedResult));
            bActive = false;
            return;
        }
        Console.WriteLine(String.Format("[ALERT] Login Success!"));
    }

    void UserLoggedOff(SteamUser.LoggedOffCallback callback)
    {
        Console.WriteLine(String.Format("[ALERT] Logged off!"));
    }

    void UserMachineAuth(SteamUser.UpdateMachineAuthCallback callback, JobID jobId)
    {
        Console.WriteLine(String.Format("[ALERT] Updating sentryfile..."));

        byte[] sentryHash = CryptoHelper.SHAHash(callback.Data);

        // write out our sentry file
        // ideally we'd want to write to the filename specified in the callback
        // but then this sample would require more code to find the correct sentry file to read during logon
        // for the sake of simplicity, we'll just use "sentry.bin"
        File.WriteAllBytes("sentry.bin", callback.Data);

        // inform the steam servers that we're accepting this sentry file
        User.SendMachineAuthResponse(new SteamUser.MachineAuthDetails
        {
            JobID = jobId,

            FileName = callback.FileName,

            BytesWritten = callback.BytesToWrite,
            FileSize = callback.Data.Length,
            Offset = callback.Offset,

            Result = EResult.OK,
            LastError = 0,

            OneTimePassword = callback.OneTimePassword,

            SentryFileHash = sentryHash,
        });

        Console.WriteLine(String.Format("Done!"));
    }
    #endregion
}


