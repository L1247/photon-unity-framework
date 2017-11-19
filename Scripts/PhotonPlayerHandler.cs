using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 管理玩家登入與跟PhotonPlayerFinder註冊
/// </summary>
public class PhotonPlayerHandler : Photon.PunBehaviour
{
    public static PhotonPlayerHandler instance;

    void Awake ( )
    {
        instance = this;
        DontDestroyOnLoad( this );
    }

    public override void OnJoinedRoom ( )
    {
        if ( PhotonNetwork.isMasterClient )
            PhotonPlayerFinder.PlayerConnected( PhotonNetwork.player );
    }

    public override void OnPhotonPlayerConnected ( PhotonPlayer newPlayer )
    {
        if ( PhotonNetwork.isMasterClient )
        {
            PhotonPlayerFinder.PlayerConnected( newPlayer );
        }
    }

    public override void OnPhotonPlayerDisconnected ( PhotonPlayer otherPlayer )
    {
        if ( PhotonNetwork.isMasterClient )
        {
            //print( otherPlayer );
            PhotonPlayerFinder.PlayerDisconnected( otherPlayer );
        }
    }

    public void SyncPlayerIdDic ( Dictionary<int , int> idDic )
    {
        photonView.RPC( "ReveicePlayerList" , PhotonTargets.Others , idDic );
    }

    [PunRPC]
    private void ReveicePlayerList ( Dictionary<int , int> idDic )
    {
        PhotonPlayerFinder.SetPlayerIdDic( idDic );
    }

    /// <summary>Called by Unity when the application is closed. Disconnects.</summary>
    protected void OnApplicationQuit ( )
    {
        PhotonNetwork.Disconnect();
    }

}
