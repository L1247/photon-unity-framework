using ExitGames.Client.Photon;
using UnityEngine;
using System.Reflection;
using System;

class TestPhotonBehaviour : PhotonBehaviour
{
    int i = 1;
    public override void Awake ( )
    {
        //base.Awake();
        Debug.Log( "Awake" );
    }
    public override void AwakeIsMine ( )
    {
        Debug.Log( "AwakeIsMine" );
    }
    public override void AwakeIsMasterClient ( )
    {
        Debug.Log( "AwakeIsMasterClient" );
    }

    public override void Start ( )
    {
        Debug.Log( "Start" );
    }

    public override void StartIsMasterClient ( )
    {
        Debug.Log( "StartIsMasterClient" );
        i++;
    }

    public override void StartIsMine ( )
    {
        Debug.Log( "StartIsMine" );
    }

    public override void Update ( )
    {
        //Debug.Log( "Update" );
    }

    public override void UpdateIsMasterClient ( )
    {
        //Debug.Log( "UpdateIsMasterClient" );
    }

    public override void UpdateIsMine ( )
    {
        //Debug.Log( "UpdateIsMine" );
    }

    public override void StreamReceive ( PhotonStream stream , PhotonMessageInfo info )
    {
        int i = ( int ) stream.ReceiveNext();
        Debug.Log( "StreamReceive : " + i );
    }

    public override void StreamSend ( PhotonStream stream , PhotonMessageInfo info )
    {
        stream.SendNext( i );
        //Debug.Log( "StreamSend : " + i );
    }

    public override void OnTriggerEnter ( Collider other )
    {
        //Debug.Log( "OnTriggerEnter" );
    }

    public override void OnTriggerEnterIsMasterClient ( Collider other )
    {
        //Debug.Log( "OnTriggerEnterIsMasterClient" );
    }

    public override void OnTriggerEnterIsMine ( Collider other )
    {
        //Debug.Log( "OnTriggerEnterIsMine" );
    }

    public override void OnTriggerExit ( Collider other )
    {
        //Debug.Log( "OnTriggerExit" );
    }

    public override void OnTriggerExitIsMasterClient ( Collider other )
    {
        //Debug.Log( "OnTriggerExitIsMasterClient" );
    }

    public override void OnTriggerExitIsMine ( Collider other )
    {
        //Debug.Log( "OnTriggerExitIsMine" );
    }

    public void Hello ( )
    {
        RPC_AllCache( test , 111 );
    }

    public void Hello2 ( )
    {
        PhotonNetwork.DestroyAll();
    }

    private void test ( object data )
    {
        Debug.Log( ( int ) data );
    }

    
}
