
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoConnect : MonoBehaviour
{
    // Use this for initialization
    void Start ( )
    {
        PhotonNetwork.ConnectUsingSettings( "DinoGen1.5" );
    }

    public void OnConnectedToMaster ( )
    {
        Debug.Log( "OnConnectedToMaster" );
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnJoinedLobby ( )
    {
        Debug.Log( "OnJoinedLobby" );
        PhotonNetwork.JoinRandomRoom();
    }
    public void OnJoinedRoom ( )
    {
        Debug.Log( "OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage" );
    }
    public virtual void OnPhotonRandomJoinFailed ( )
    {
        Debug.Log( "OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);" );
    }

    public void Update ( )
    {
        if ( Input.GetKeyDown(KeyCode.Space) )
        {
            Debug.Log(PhotonNetwork.playerList.Length);
        }
    }
}
