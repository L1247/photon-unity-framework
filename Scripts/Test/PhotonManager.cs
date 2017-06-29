using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PhotonManager : Photon.PunBehaviour
{
    public Transform CanvasTrans;
    public Button ConnectBtn, CreateRoomBtn;
    void Start ( )
    {
        //InvokeRepeating( "UpdateStatus" , 0 , 1f );
        //Connect();
        for ( int i = 0 ; i < CanvasTrans.childCount ; i++ )
        {
            CanvasTrans.GetChild(i).gameObject.SetActive( false );
        }
        ConnectBtn.gameObject.SetActive( true );
        ConnectBtn.onClick.AddListener( Connect );
        CreateRoomBtn.onClick.AddListener( CreateRoom );
    }



    #region Utility Methods
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
        // 與 Photon Cloud/Server 建立起連線,可以用字串方式設定須要連線的遊戲版本
        PhotonNetwork.ConnectUsingSettings( "PUN_PhotonCloud_1.0" );
    }

    void CreateRoom ( )
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        PhotonNetwork.CreateRoom( PhotonNetwork.playerName , options , TypedLobby.Default );

        Debug.Log( "CreateGame" );
        CreateRoomBtn.gameObject.SetActive( false );
    }
    #endregion

    #region Photon Callbacks
    // For each valid game room, creates a join button.
    public override void OnReceivedRoomListUpdate ( )
    {

        print( PhotonNetwork.GetRoomList().Length );

        foreach ( RoomInfo room in PhotonNetwork.GetRoomList() )
        {
            print( room );
        }
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
    }

    // if we join (or create) a room, no need for the create button anymore;
    public override void OnJoinedRoom ( )
    {
        Debug.Log( "OnJoinedRoom" );
    }
    #endregion

}