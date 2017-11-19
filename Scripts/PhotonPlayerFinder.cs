using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 玩家[Avatar] ik 管理器
/// </summary>
public class PhotonPlayerFinder
{
    static List<Transform>             playerList           = new List<Transform>();
    static List<Transform>             playerStandSpaceList = new List<Transform>();
    /// <summary>
    /// Index (玩家順位) / OwnerID
    /// </summary>
    static Dictionary<int , int>       playerIdDic          = new Dictionary<int , int>();
    /// <summary>
    /// Index (玩家是第幾順位) / PlayerStandPlace (玩家初始位置)
    /// </summary>
    static Dictionary<int , Transform> playerStandSpaceDic  = new Dictionary<int, Transform >();
    /// <summary>
    /// OwnerID / Avatar Instance
    /// </summary>
    static Dictionary<int , Transform> playerAvatarDic      = new Dictionary<int, Transform>();

    /// <summary>
    /// 註冊玩家初始位置，並初始化Dic
    /// </summary>
    /// <param name="StandSpaceList"></param>
    public static void RegPlayerStandSpaceList ( List<Transform> StandSpaceList )
    {
        playerStandSpaceList = StandSpaceList;
        for ( int i = 0 ; i < playerStandSpaceList.Count ; i++ )
        {
            playerStandSpaceDic[ i + 1 ] = playerStandSpaceList[ i ];
        }
    }

    /// <summary>
    /// 註冊最大玩家人數，並設定PlayerDic內容初始化
    /// </summary>
    /// <param name="defaultPlayerCount"></param>
    public static void InitPlayerDic ( int defaultPlayerCount )
    {
        for ( int i = 1 ; i < defaultPlayerCount + 1 ; i++ )
        {
            playerIdDic[ i ] = 0;
        }
    }

    /// <summary>
    /// [Sync] Client Do This
    /// </summary>
    /// <param name="PlayerIDs"></param>
    public static void SetPlayerIdDic ( Dictionary<int , int> _playerDic )
    {
        playerIdDic = _playerDic;
    }

    /// <summary>
    /// 註冊角色模型
    /// </summary>
    /// <param name="trans"></param>
    public static void RegPlayer ( int playerID , Transform trans )
    {
        Debug.Log( trans );
        Debug.Log( "RegPlayer Avatar : " + playerID );
        
        playerAvatarDic[ playerID ] = trans;
    }

    public static void UnRegPlayer ( int playerID )
    {
        playerAvatarDic.Remove(playerID);
        Debug.Log( "UnRegAvatar : " + playerID );
    }

    public static List<Transform> GetPlayerTransList ( )
    {
        //return playerDic.Values.ToList();
        return playerList;
    }

    public static List<int> GetPlayerIDList ( )
    {
        return playerIdDic.Keys.ToList();
    }

    /// <summary>
    /// 透過PlayerID取得Avatar實例
    /// </summary>
    /// <param name="playerID"></param>
    /// <returns></returns>
    public static Transform GetPlayerAvatarInstance(int playerID)
    {
        return playerAvatarDic[ playerID ];
    }

    /// <summary>
    /// 透過玩家順序編號，取得玩家初始點的位置
    /// </summary>
    /// <param name="index">玩家順序編號</param>
    /// <returns></returns>
    public static Transform GetPlayerInitTransform ( int playerID )
    {
        // *** 透過playerID去playerIdDic找Index後，再去playerStandSpaceDic拿到真正的玩家初始位置。 ***
        int _index = playerIdDic.FirstOrDefault( kvp => kvp.Value == playerID ).Key;
        Transform _trans = playerStandSpaceDic[ _index ];
        return _trans;
    }

    public static int GetPlayerId(int playerIndex)
    {
        return playerIdDic[ playerIndex ];
    }

    /// <summary>
    /// 透過玩家ID取得順位Index，最小順位為1
    /// </summary>
    /// <param name="playerID"></param>
    /// <returns></returns>
    public static int GetPlayerIndex(int playerID)
    {
        int id = playerIdDic.FirstOrDefault( kvp => kvp.Value == playerID ).Key;
        return id;
    }

    //public static int GetPlayerID ( Transform standPlace )
    //{
    //    return playerStandSpaceDic[ standPlace ];
    //}

    public static void PlayerConnected ( PhotonPlayer newPlayer )
    {
        int emptyIndex = playerIdDic.FirstOrDefault( kvp => kvp.Value == 0 ).Key;
        if ( emptyIndex == 0 )
            return;
        int playerID = newPlayer.ID;
        Debug.Log( "RegPlayer, ID : " + playerID );
        playerIdDic[ emptyIndex ] = playerID;

        PhotonPlayerHandler.instance.SyncPlayerIdDic( playerIdDic );
    }

    public static void PlayerDisconnected ( PhotonPlayer otherPlayer )
    {
        int playerID = otherPlayer.ID;
        int playerIndex = playerIdDic.FirstOrDefault( kvp => kvp.Value == playerID ).Key;
        if ( playerIndex == 0 )
            return;
        playerIdDic[ playerIndex ] = 0;
        PhotonPlayerHandler.instance.SyncPlayerIdDic( playerIdDic );
    }

    public static void ShowAllPlayerID ( )
    {
        Debug.Log( "-------玩家ID列表-------" );
        foreach ( var p in playerIdDic )
        {
            Debug.Log( p.Value );
        }
    }

    public static void ShowAllPlayerStandPlace ( )
    {
        Debug.Log( "-------玩家初始位置列表-------" );
        foreach ( var p in playerStandSpaceDic )
        {
            Debug.Log( p.Value );
        }
    }

}
