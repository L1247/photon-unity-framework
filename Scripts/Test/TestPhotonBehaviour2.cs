using UnityEngine;
class TestPhotonBehaviour2 : PhotonBehaviour
{

    public override void Start ( )
    {
        Debug.Log( "Start2" );
    }


    public override void StartIsMine ( )
    {
        Debug.Log( "StartIsMine2" );
    }

    public override void OnTriggerEnterIsMine ( Collider other )
    {

    }

    public void Hello ( )
    {
        RPC_AllCache( test , 111 );
    }

    void test ( object obj )
    {
        Log( obj );
    }

    

}
