using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////
// PlayerIndex : 由1開始
// PlayerID    : 由1開始
//////////////////////////////////////////////////////////////////////////

/// <summary>
/// 玩家[Avatar] ik 管理器
/// </summary>
public class PhotonPlayerFinder
{
    static List<Transform>             playerList                     = new List<Transform>();
    static List<Transform>             playerStandSpaceList           = new List<Transform>();
    /// <summary>
    /// Index (玩家順位)從1開始 / OwnerID
    /// </summary>
    static Dictionary<int , int>       playerIdDic                    = new Dictionary<int , int>();
    /// <summary>
    /// Index (玩家是第幾順位) / PlayerStandPlace (玩家初始位置)
    /// </summary>
    static Dictionary<int , Transform> playerStandSpaceDic            = new Dictionary<int, Transform >();
    /// <summary>
    /// OwnerID / Avatar Instance
    /// </summary>
    static Dictionary<int , Transform> playerAvatarDic                = new Dictionary<int, Transform>();
    /// <summary>
    /// PlayerIndex / AvatarController GameObject PhotonViewID(ViewID)
    /// </summary>
    static Dictionary<int , int> avatarHpcontrollerInstanceDic = new Dictionary<int, int>();


    static bool init;
    /// <summary>
    /// 設定最大玩家人數
    /// </summary>
    public readonly static int MaxPlayerCount = 4;
    static GameObject AvatarHPcontroller;

    #region Player Register、Initialization
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
        if ( init == true )
            return;
        for ( int i = 1 ; i < defaultPlayerCount + 1 ; i++ )
        {
            playerIdDic[ i ] = 0;
        }
        init = true;
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
    /// [Sync] Client Do This
    /// </summary>
    /// <param name="PlayerIDs"></param>
    public static void SetAvatarHpGoDic ( Dictionary<int , int> _avatarPvIDDic )
    {
        avatarHpcontrollerInstanceDic = _avatarPvIDDic;
    }

    /// <summary>
    /// 註冊角色模型
    /// </summary>
    /// <param name="trans"></param>
    public static void RegPlayer ( int playerID , Transform trans )
    {
        //Debug.Log( trans );

        playerAvatarDic[ playerID ] = trans;
        playerList = playerAvatarDic.Values.ToList();
    }

    public static void UnRegPlayer ( int playerID )
    {
        playerAvatarDic.Remove( playerID );
        playerList = playerAvatarDic.Values.ToList();
        //Debug.Log( "UnRegAvatar : " + playerID );
    }
    #endregion


    #region Public Get Methods
    public static List<Transform> GetPlayerTransList ( )
    {
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
    public static Transform GetPlayerAvatarInstance ( int playerID )
    {
        if ( playerID <= 0 )
        {
            Debug.LogErrorFormat( "No Found PlayerID : {0}" , playerID );
            return null;
        }
        return playerAvatarDic[ playerID ];
    }

    /// <summary>
    /// 取得目前Client的角色實例(Avatar Player)
    /// </summary>
    /// <returns></returns>
    public static Transform GetOwnerPlayerAvatarInstance ( )
    {
        return GetPlayerAvatarInstance( PhotonNetwork.player.ID );
    }


    /// <summary>
    /// 透過玩家順序編號，取得玩家初始點的位置
    /// </summary>
    /// <param name="index">玩家順序編號</param>
    /// <returns></returns>
    public static Transform GetPlayerInitTransform ( int playerID )
    {
        Transform _trans = null;
        // *** 透過playerID去playerIdDic找Index後，再去playerStandSpaceDic拿到真正的玩家初始位置。 ***
        int _index = playerIdDic.FirstOrDefault( kvp => kvp.Value == playerID ).Key;
        if ( _index > 0 )
            _trans = playerStandSpaceDic[ _index ];
        return _trans;
    }

    public static int GetPlayerId ( int playerIndex )
    {
        return playerIdDic[ playerIndex ];
    }

    /// <summary>
    /// 透過玩家ID取得順位Index，最小順位為1
    /// </summary>
    /// <param name="playerID"></param>
    /// <returns></returns>
    public static int GetPlayerIndex ( int playerID )
    {
        int id = playerIdDic.FirstOrDefault( kvp => kvp.Value == playerID ).Key;
        return id;
    }

    /// <summary>
    /// 回傳AvatarHpControllerGameObject
    /// </summary>
    /// <param name="playerIndex">玩家順序編號</param>
    /// <returns></returns>
    public static GameObject GetAvatarHpControllerGo ( int playerIndex )
    {
        GameObject result = null;
        int viewID = 0;
        avatarHpcontrollerInstanceDic.TryGetValue( playerIndex , out viewID );
        //Debug.LogFormat( "ViewID : {0}" , viewID );
        PhotonView pv = PhotonView.Find( viewID );
        if ( pv )
            result = pv.gameObject;
        return result;
    }

    /// <summary>
    /// 取得AvatarHpController的GameObject陣列
    /// </summary>
    /// <returns></returns>
    public static List<GameObject> GetAvatarHpControllerGoList ( )
    {
        List<GameObject> goList = new List<GameObject>();
        List <int > idList = avatarHpcontrollerInstanceDic.Values.ToList();
        for ( int i = 0 ; i < idList.Count ; i++ )
        {
            int viewId = idList[i];
            PhotonView pv = PhotonView.Find(viewId);
            if ( pv )
            {
                goList.Add( pv.gameObject );
            }
        }
        return goList;
    }

    #endregion

    #region Handle Players Connection
    /// <summary>
    /// 每個玩家登入遊戲後呼叫，Master才會呼叫
    /// </summary>
    /// <param name="player"></param>
    public static void PlayerConnected ( PhotonPlayer player )
    {
        if ( init == false )
            InitPlayerDic( MaxPlayerCount );
        int emptyIndex = playerIdDic.FirstOrDefault( kvp => kvp.Value == 0 ).Key;
        int playerID = player.ID;
        Debug.Log( "RegPlayer, ID : " + playerID );
        playerIdDic[ emptyIndex ] = playerID;

        // *** 產生AvatarHpcontroller ***
        if ( AvatarHPcontroller == null )
            AvatarHPcontroller = ResourcesManager.LoadAndCacheReource( "AvatarHPcontroller" ) as GameObject;
        GameObject avatarHpcontroller = PhotonNetwork.Instantiate(
            "AvatarHPcontroller" , Vector3.zero , Quaternion.identity , 0);
        PhotonView pv = avatarHpcontroller.GetComponent<PhotonView>();
        int viewID = pv.viewID;
        avatarHpcontroller.name = string.Format( "AvatarHPcontroller [{0}]" , emptyIndex );
        avatarHpcontrollerInstanceDic.Add( emptyIndex , viewID );
        PhotonPlayerHandler.instance.SyncPlayerIdDic( playerIdDic , avatarHpcontrollerInstanceDic );
    }

    public static void PlayerDisconnected ( PhotonPlayer player )
    {
        int playerID = player.ID;
        int playerIndex = playerIdDic.FirstOrDefault( kvp => kvp.Value == playerID ).Key;
        playerIdDic[ playerIndex ] = 0;
        if ( avatarHpcontrollerInstanceDic.ContainsKey( playerIndex ) )
        {
            int viewID = avatarHpcontrollerInstanceDic[playerIndex];
            PhotonView pv = PhotonView.Find(viewID);
            GameObject go = pv.gameObject;
            PhotonNetwork.Destroy( go );
            avatarHpcontrollerInstanceDic.Remove( playerIndex );
        }

        // *** Sync ***
        PhotonPlayerHandler.instance.SyncPlayerIdDic( playerIdDic , avatarHpcontrollerInstanceDic );
    }
    #endregion

    #region Debug Show Log
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
    #endregion

}
