using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// [Avatar] ik 人物管理器
/// </summary>
public class PlayerFinder
{
    static List<Transform> playerList = new List<Transform>();
    static List<Transform> playerStandSpaceList = new List<Transform>();
    /// <summary>
    /// id / PlayerStandPlace
    /// </summary>
    static Dictionary<int , Transform> playerDic = new Dictionary<int , Transform>();
    /// <summary>
    /// PlayerStandPlace / id
    /// </summary>
    static Dictionary<Transform , int> playerStandSpaceDic = new Dictionary<Transform , int>();

    public static void RegPlayerStandSpaceList ( List<Transform> playerList )
    {
        playerStandSpaceList = playerList;
        for ( int i = 0 ; i < playerStandSpaceList.Count ; i++ )
        {
            playerStandSpaceDic[ playerStandSpaceList[ i ] ] = 0;
        }
    }

    /// <summary>
    /// Client Do This
    /// </summary>
    /// <param name="PlayerDatas"></param>
    public static void SetPlayerStandSpaceDic ( int[ ] PlayerDatas )
    {
        int i = 0;
        var buffer = new List<Transform>( playerStandSpaceDic.Keys );

        foreach ( var item in buffer )
        {
            //Debug.Log( item );
            if ( i < PlayerDatas.Length )
            {
                //Debug.Log( "key : " + item );
                playerStandSpaceDic[ item ] = PlayerDatas[ i ];
                //Debug.Log( "value : " + playerStandSpaceDic[ item ] );
                i++;
            }
        }
    }

    public static void RegPlayer ( Transform trans , int id )
    {
        if ( playerDic.ContainsKey( id ) )
        {
            playerDic[ id ] = trans;
        }
        else
        {
            playerList.Add( trans );
            playerDic.Add( id , trans );
        }
    }

    public static void UnRegPlayer ( int id )
    {
        if ( playerDic.ContainsKey( id ) )
        {
            playerList.Remove( playerDic[ id ] );
            playerDic.Remove( id );
        }
    }

    public static Transform GetPlayerTrans ( int id )
    {
        return playerDic[ id ];
    }

    public static int GetPlayerID ( Transform standPlace )
    {
        return playerStandSpaceDic[ standPlace ];
    }

    public static List<Transform> GetPlayerTransList ( )
    {
        //return playerDic.Values.ToList();
        return playerList;
    }

    public static List<int> GetPlayerIDList ( )
    {
        return playerDic.Keys.ToList();
    }

    public static void MasterPlayerConnected ( PhotonPlayer masterPlayer )
    {
        PlayerConnected( masterPlayer );
    }

    public static Transform GetPlayerStandPlace ( int id )
    {
        return playerStandSpaceDic.FirstOrDefault( kvp => kvp.Value == id ).Key;
    }

    public static void PlayerConnected ( PhotonPlayer newPlayer )
    {
        //Debug.Log( newPlayer.ID );
        int id = newPlayer.ID;
        var keys = playerStandSpaceDic.FirstOrDefault( kvp => kvp.Value == 0 ).Key;
        //Debug.Log( keys );
        if ( playerStandSpaceDic.ContainsValue( id ) == false && keys != null )
        {
            Transform standPlace = keys as Transform;
            playerStandSpaceDic[ standPlace ] = id;
        }

        //foreach ( var item in playerStandSpaceDic )
        //{
        //    Debug.Log( item.Key + "   :   " + item.Value );
        //}

        PhotonPlayerHandler.instance.SyncPlayerStandSpaceList( playerStandSpaceDic.Values.ToArray() );
    }

    public static void PlayerDisconnected ( PhotonPlayer otherPlayer )
    {
        //Debug.Log( otherPlayer.ID );
        int id = otherPlayer.ID;
        var keys = playerStandSpaceDic.FirstOrDefault( kvp => kvp.Value == id ).Key;
        //Debug.Log( keys );
        if ( playerStandSpaceDic.ContainsValue( id ) && keys != null )
        {
            Transform standPlace = keys as Transform;
            playerStandSpaceDic[ standPlace ] = 0;
        }
        //foreach ( var item in playerStandSpaceDic )
        //{
        //    Debug.Log( item.Key + "   :   " + item.Value );
        //}
    }
}
