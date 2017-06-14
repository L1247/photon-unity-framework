using UnityEngine;

public class PhotonPlayerHandler : Photon.PunBehaviour
{
    public static PhotonPlayerHandler instance;

    void Awake ( )
    {
        instance = this;
        DontDestroyOnLoad( this );
    }

    public override void OnPhotonPlayerConnected ( PhotonPlayer newPlayer )
    {
        if ( PhotonNetwork.isMasterClient )
        {
            //print( newPlayer.ID );
            PlayerFinder.PlayerConnected( newPlayer );
        }
    }

    public override void OnPhotonPlayerDisconnected ( PhotonPlayer otherPlayer )
    {
        if ( PhotonNetwork.isMasterClient )
        {
            //print( otherPlayer );
            PlayerFinder.PlayerDisconnected( otherPlayer );
        }
    }

    public void SyncPlayerStandSpaceList ( int[ ] standPlaceList )
    {
        photonView.RPC( "ReveicePlayerStandData" , PhotonTargets.Others , standPlaceList );
    }

    [PunRPC]
    private void ReveicePlayerStandData ( int[ ] standPlaceList )
    {
        //foreach ( var item in standPlaceList )
        //{
        //    Debug.Log( item );
        //}
        PlayerFinder.SetPlayerStandSpaceDic( standPlaceList );
    }

    /// <summary>Called by Unity when the application is closed. Disconnects.</summary>
    protected void OnApplicationQuit ( )
    {
        PhotonNetwork.Disconnect();
    }

}
