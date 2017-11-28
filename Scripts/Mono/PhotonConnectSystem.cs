using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;

using System.Text.RegularExpressions;
using ExitGames.Demos.DemoAnimator;
using System;

/// <summary>
/// 登入系統
/// </summary>
public class PhotonConnectSystem : Photon.PunBehaviour/*, iFlow*/
{

    public void StartFlow ( )
    {
        UI_IP_Input.SetActive( true );
    }

    #region Public Variables

    public bool ShowDebug;

    [Tooltip("The Ui Text to inform the user about the connection progress")]
    public Text feedbackText;

    [Tooltip("The maximum number of players per room")]
    public byte maxPlayersPerRoom = 4;

    [Tooltip("The UI Loader Anime")]
    public LoaderAnime loaderAnime;

    public GameObject UI_IP_Input;

    #endregion

    #region Private Variables
    /// <summary>
    /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon, 
    /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
    /// Typically this is used for the OnConnectedToMaster() callback.
    /// </summary>
    bool isConnecting;

    /// <summary>
    /// This client's version number. Users are separated from each other by gameversion (which allows you to make breaking changes).
    /// </summary>
    string _gameVersion = "1";
    string _ConncectIP;
    private InputField InputField_IP;
    private Button Btn_Confirm , Btn_UseConnectedIP;

    //public event EventHandler<srGameEventArgs> EventUpdated;

    #endregion

    #region MonoBehaviour CallBacks

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake ( )
    {
        if ( loaderAnime == null )
        {
            Debug.LogError( "<Color=Red><b>Missing</b></Color> loaderAnime Reference." , this );
        }

        // #NotImportant
        // Force LogLevel
        PhotonNetwork.logLevel = PhotonLogLevel.ErrorsOnly;

        // #Critical
        // we don't join the lobby. There is no need to join a lobby to get the list of rooms.
        PhotonNetwork.autoJoinLobby = false;

        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.automaticallySyncScene = true;

        UI_IP_Input.SetActive( false );
        InputField_IP = UI_IP_Input.transform.Find( "InputField_IP" ).GetComponent<InputField>();
        Btn_Confirm = UI_IP_Input.transform.Find( "Button_Confirm" ).GetComponent<Button>();
        Btn_UseConnectedIP = UI_IP_Input.transform.Find( "Button_UseConnectedIP" ).GetComponent<Button>();

        if ( Btn_Confirm )
            Btn_Confirm.onClick.AddListener( delegate {
                SetButtonVisible( false );
                Connect();
            } );

        string connnectIPstr = PlayerPrefs.GetString( "ConnectedIP" );
        if ( connnectIPstr != string.Empty )
        {
            if ( Btn_UseConnectedIP )
            {
                Text Text_UseConnectedIP = Btn_UseConnectedIP.GetComponentInChildren<Text>();
                if ( Text_UseConnectedIP )
                {
                    _ConncectIP = PlayerPrefs.GetString( "ConnectedIP" );
                    Text_UseConnectedIP.text = string.Format( Text_UseConnectedIP.text , _ConncectIP );
                    if ( Btn_UseConnectedIP )
                        Btn_UseConnectedIP.onClick.AddListener( delegate {
                            SetButtonVisible( false );
                            Connect( _ConncectIP );
                        } );
                }
            }
        }
        else
        {
            Btn_UseConnectedIP.gameObject.SetActive( false );
        }
    }



    void Start ( )
    {

    }



    #endregion


    #region Public Methods


    /// <summary>
    /// Start the connection process. 
    /// - If already connected, we attempt joining a random room
    /// - if not yet connected, Connect this application instance to Photon Cloud Network
    /// </summary>
    public void Connect ( string _IP = "" )
    {
        string UserInputText = InputField_IP.text.Trim();
        string _ConnectIP = string.Empty;

        if ( _IP != "" )
            _ConnectIP = _IP;
        else
            _ConnectIP = UserInputText;
        //PhotonNetwork.PhotonServerSettings.ServerAddress = Common.ReadTextFileConfig("PhotonServerIPAddress");
        PhotonNetwork.PhotonServerSettings.ServerAddress = _ConnectIP;
        string Name = string.Empty;
        Name = /*Common.ReadTextFileConfig( "PCName" );*/ string.Empty;


        // 取得本機名稱
        string strHostName = Dns.GetHostName();

        // 取得本機的 IpHostEntry 類別實體
        IPHostEntry iphostentry = Dns.GetHostEntry( strHostName );


        foreach ( IPAddress ipaddress in iphostentry.AddressList )
        {
            if ( ipaddress.AddressFamily == AddressFamily.InterNetwork )
            {
                Name += /*" " + */ipaddress.ToString();
                break;
            }
        }

        PhotonNetwork.playerName = Name;

        // we want to make sure the log is clear everytime we connect, we might have several failed attempted if connection failed.
        feedbackText.text = "";

        isConnecting = true;
        // start the loader animation for visual effect.
        if ( loaderAnime != null )
        {
            loaderAnime.StartLoaderAnimation();
        }

        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if ( PhotonNetwork.connected )
        {
            LogFeedback( "Joining Room..." );
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnPhotonRandomJoinFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {

            LogFeedback( "Connecting..." + PhotonNetwork.PhotonServerSettings.ServerAddress );

            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings( _gameVersion );
        }
    }


    /// <summary>
    /// Logs the feedback in the UI view for the player, as opposed to inside the Unity Editor for the developer.
    /// </summary>
    /// <param name="message">Message.</param>
    public void LogFeedback ( string message )
    {
        // we do not assume there is a feedbackText defined.
        if ( feedbackText == null )
        {
            return;
        }

        // add new messages as a new line and at the bottom of the log.
        feedbackText.text += System.Environment.NewLine + message;
    }

    #endregion


    #region Photon.PunBehaviour CallBacks

    /// <summary>
    /// Called after the connection to the master is established and authenticated but only when PhotonNetwork.autoJoinLobby is false.
    /// </summary>
    public override void OnConnectedToMaster ( )
    {

        //Debug.Log( "Region:" + PhotonNetwork.networkingPeer.CloudRegion );

        // we don't want to do anything if we are not attempting to join a room. 
        // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
        // we don't want to do anything.
        if ( isConnecting )
        {
            LogFeedback( "OnConnectedToMaster: Next -> try to Join Random Room" );
            if ( ShowDebug )
            {
                //Debug.Log( "DemoAnimator/Launcher: OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.\n Calling: PhotonNetwork.JoinRandomRoom(); Operation will fail if no room found" );
            }

            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnPhotonRandomJoinFailed()
            PhotonNetwork.JoinRandomRoom();
        }
    }

    /// <summary>
    /// Called when a JoinRandom() call failed. The parameter provides ErrorCode and message.
    /// </summary>
    /// <remarks>
    /// Most likely all rooms are full or no rooms are available. <br/>
    /// </remarks>
    /// <param name="codeAndMsg">codeAndMsg[0] is short ErrorCode. codeAndMsg[1] is string debug msg.</param>
    public override void OnPhotonRandomJoinFailed ( object[] codeAndMsg )
    {
        LogFeedback( "<Color=Red>OnPhotonRandomJoinFailed</Color>: Next -> Create a new Room" );
        if ( ShowDebug )
        {
            //Debug.Log( "DemoAnimator/Launcher:OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);" );
        }
        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.

        PhotonNetwork.CreateRoom( null , new RoomOptions() { MaxPlayers = this.maxPlayersPerRoom } , null );
    }


    /// <summary>
    /// Called after disconnecting from the Photon server.
    /// </summary>
    /// <remarks>
    /// In some cases, other callbacks are called before OnDisconnectedFromPhoton is called.
    /// Examples: OnConnectionFail() and OnFailedToConnectToPhoton().
    /// </remarks>
    public override void OnDisconnectedFromPhoton ( )
    {
        LogFeedback( "<Color=Red>OnDisconnectedFromPhoton</Color>" );
        Debug.LogError( "DemoAnimator/Launcher:Disconnected" );

        SetButtonVisible( true );

        // #Critical: we failed to connect or got disconnected. There is not much we can do. Typically, a UI system should be in place to let the user attemp to connect again.
        if ( loaderAnime )
            loaderAnime.StopLoaderAnimation();

        isConnecting = false;
    }

    /// <summary>
    /// Called when entering a room (by creating or joining it). Called on all clients (including the Master Client).
    /// </summary>
    /// <remarks>
    /// This method is commonly used to instantiate player characters.
    /// If a match has to be started "actively", you can call an [PunRPC](@ref PhotonView.RPC) triggered by a user's button-press or a timer.
    ///
    /// When this is called, you can usually already access the existing players in the room via PhotonNetwork.playerList.
    /// Also, all custom properties should be already available as Room.customProperties. Check Room.playerCount to find out if
    /// enough players are in the room to start playing.
    /// </remarks>
    public override void OnJoinedRoom ( )
    {
        LogFeedback( "<Color=Green>OnJoinedRoom</Color> with " + PhotonNetwork.room.playerCount + " Player(s)" );
        //Debug.Log( "DemoAnimator/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage" );

        // #Critical: We only load if we are the first player, else we rely on  PhotonNetwork.automaticallySyncScene to sync our instance scene.
        //LoadSelectMenuLevel();
        //if ( PhotonNetwork.room.playerCount == 1 )
        //{
        //}
        //if ( EventUpdated != null )
        //    EventUpdated( this , new srGameEventArgs( "Connected" ) );
    }

    public override void OnPhotonPlayerConnected ( PhotonPlayer other )
    {
        //if ( PhotonNetwork.room.playerCount == 1 )
        //{
        //    LoadGameMainLevel();
        //}
    }

    #endregion
    public void LoadSelectMenuLevel ( )
    {
        // #Critical
        // Load the Room Level. 
        //if ( PhotonNetwork.isMasterClient )
        //{
        //    PhotonNetwork.LoadLevel( GameEM.GameScene.SelectMenu.ToString() );
        //}
    }

    private void SetButtonVisible ( bool bShow )
    {
        if ( Btn_Confirm )
            Btn_Confirm.gameObject.SetActive( bShow );
        if ( Btn_UseConnectedIP && String.IsNullOrEmpty( _ConncectIP ) == false )
            Btn_UseConnectedIP.gameObject.SetActive( bShow );
    }

}
