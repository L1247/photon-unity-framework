﻿using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 管理玩家登入與跟PhotonPlayerFinder註冊
/// </summary>
public class PhotonPlayerHandler : Photon.PunBehaviour
{
    public static PhotonPlayerHandler instance;

    [SerializeField]
    [ReadOnly]
    private string ScriptDescription = "管理玩家登入與跟PhotonPlayerFinder註冊";
    void Awake ( )
    {
        instance = this;
        DontDestroyOnLoad( this );
    }

    /// <summary>
    /// Master Client 玩家登入註冊
    /// </summary>
    public override void OnJoinedRoom ( )
    {
        if ( PhotonNetwork.isMasterClient )
            PhotonPlayerFinder.PlayerConnected( PhotonNetwork.player );
    }

    /// <summary>
    /// Other Client 玩家登入註冊
    /// </summary>
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

    /// <summary>
    /// 同步所有ID給其他Client做使用
    /// </summary>
    /// <param name="idDic"></param>
    public void SyncPlayerIdDic ( Dictionary<int , int> idDic )
    {
        photonView.RPC( "ReveicePlayerList" , PhotonTargets.Others , idDic );
    }

    [PunRPC]
    private void ReveicePlayerList ( Dictionary<int , int> idDic )
    {
        //print( "Rec Count : " + idDic.Count );
        PhotonPlayerFinder.SetPlayerIdDic( idDic );
    }

    /// <summary>Called by Unity when the application is closed. Disconnects.</summary>
    protected void OnApplicationQuit ( )
    {
        PhotonNetwork.Disconnect();
    }

}
