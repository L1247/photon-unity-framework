using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;

public class PhotonManager : Photon.PunBehaviour
{
    public Canvas m_Canvas;
    public Transform ShowBtns, RoomListBtn;
    public Button ConnectBtn, CreateRoomBtn, LeaveRoomBtn, ShowRoomListBtn;
    public GameObject BtnPrefab;
    public Transform btn;
    void Start ( )
    {
        PhotonNetwork.player.NickName = "Player " + Random.Range( 0 , 101 );
        //InvokeRepeating( "UpdateStatus" , 0 , 1f );
        //Connect();
        for ( int i = 0 ; i < ShowBtns.childCount ; i++ )
        {
            ShowBtns.GetChild( i ).gameObject.SetActive( false );
        }
        ConnectBtn.gameObject.SetActive( true );
        ShowRoomListBtn.gameObject.SetActive( true );
        ConnectBtn.onClick.AddListener( Connect );
        CreateRoomBtn.onClick.AddListener( CreateRoom );
        LeaveRoomBtn.onClick.AddListener( LeaveRoom );
        ShowRoomListBtn.onClick.AddListener( ShowRoomList );
    }

    #region Utility Methods
    private void DestroyRoomButton ( )
    {
        for ( int i = 0 ; i < RoomListBtn.childCount ; i++ )
        {
            Destroy( RoomListBtn.GetChild( i ).gameObject );
        }
    }

    private void ShowRoomButton ( )
    {
        RoomInfo[ ] roomInfoList = PhotonNetwork.GetRoomList();
        for ( int i = 0 ; i < roomInfoList.Length ; i++ )
        {
            RoomInfo room = roomInfoList[ i ];
            if ( !room.IsOpen )
                continue;
            print( room );
            string RoomName = room.Name;
            StringBuilder sb = new StringBuilder( RoomName );
            sb.Append( " , " );
            sb.Append( room.PlayerCount );
            sb.Append( "/" );
            sb.Append( room.MaxPlayers );
            Transform roomButton = Instantiate( BtnPrefab ).transform;
            roomButton.transform.SetParent( RoomListBtn );
            roomButton.localPosition = new Vector3( 0 , 135 - ( i + 1 ) * 35 , 0 );
            roomButton.name = RoomName;
            roomButton.GetComponentInChildren<Text>().text = sb.ToString();
            roomButton.GetComponent<Button>().onClick.AddListener( ButtonClick );
            roomButton.GetComponent<RoomJoiner>().RoomName = RoomName;
        }
    }

    void ButtonClick ( )
    {
    }

    void UpdateStatus ( )
    {
        string status = PhotonNetwork.connectionStateDetailed.ToString();
        int ping = PhotonNetwork.GetPing();
        Debug.Log( status + ", " + ping + "ms" );
        print( "GetRoomList : " + PhotonNetwork.GetRoomList().Length );
        print( "GetInsideLobby : " + PhotonNetwork.insideLobby );
    }
    public void GetList ( )
    {
        print( PhotonNetwork.GetRoomList().Length );
    }


    public void GetInsideLobby ( )
    {
        print( PhotonNetwork.insideLobby );
    }
    #endregion

    #region Button Methods
    void Connect ( )
    {
        print( "Connect" );
        // 與 Photon Cloud/Server 建立起連線,可以用字串方式設定須要連線的遊戲版本
        PhotonNetwork.ConnectUsingSettings( "DinoGen2.0" );
    }

    void CreateRoom ( )
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        PhotonNetwork.CreateRoom( PhotonNetwork.player.NickName , options , TypedLobby.Default );

        Debug.Log( "CreateGame" );
        CreateRoomBtn.gameObject.SetActive( false );
    }

    void LeaveRoom ( )
    {
        PhotonNetwork.LeaveRoom();
        LeaveRoomBtn.gameObject.SetActive( false );
    }

    void ShowRoomList ( )
    {
        Debug.Log( "ShowRoomList" );
        RoomInfo[ ] RoomList = PhotonNetwork.GetRoomList();
        if ( RoomList.Length > 0 )
        {
            for ( int i = 0 ; i < RoomList.Length ; i++ )
            {
                Debug.Log( RoomList[ i ] );
            }
        }
    }
    #endregion

    #region Photon Callbacks
    // For each valid game room, creates a join button.
    public override void OnReceivedRoomListUpdate ( )
    {
        DestroyRoomButton();
        ShowRoomButton();
    }

    // When connected to Photon, enable nickname editing (too)
    public override void OnConnectedToMaster ( )
    {
        PhotonNetwork.JoinLobby();
        Debug.Log( "OnConnectedToMaster" );
        ConnectBtn.gameObject.SetActive( false );
        CreateRoomBtn.gameObject.SetActive( true );
    }

    // When connected to Photon Lobby, disable nickname editing and messages text, enables room list
    public override void OnJoinedLobby ( )
    {
        Debug.Log( "OnJoinedLobby" );
    }

    // (masterClient only) enables start race button
    public override void OnCreatedRoom ( )
    {
        Debug.Log( "OnCreateRoom" );
        print( "GetRoomList : " + PhotonNetwork.GetRoomList().Length );
        DestroyRoomButton();
    }

    // if we join (or create) a room, no need for the create button anymore;
    public override void OnJoinedRoom ( )
    {
        Debug.Log( "OnJoinedRoom" );
        LeaveRoomBtn.gameObject.SetActive( true );
        DestroyRoomButton();
    }

    public override void OnLeftRoom ( )
    {
        Debug.Log( "OnLeftRoom" );
        ShowRoomButton();
    }
    #endregion

}