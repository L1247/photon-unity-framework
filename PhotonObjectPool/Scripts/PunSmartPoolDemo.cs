using UnityEngine;
using System.Collections;

using ExitGames.Client.Photon;

/// <summary>
/// Pun smart pool demo. Simple script to instantiate the player. Note that this script doesn't change whether you use a pool manager or not.
/// Check out PunSmartPoolBridge.cs to see how pooling is enabled.
/// </summary>
public class PunSmartPoolDemo: Photon.PunBehaviour {
	
	public override void OnJoinedRoom ()
	{
       // PhotonNetwork.Instantiate("cube", new Vector3(0f, 0f, 0f), Quaternion.identity, 0);      
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) {
            PhotonNetwork.Instantiate("Cube", new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
        }

        if ( Input.GetKeyDown( KeyCode.B ) )
        {
            PhotonNetwork.Instantiate( "Cube" , new Vector3( 0f , 0f , 0f ) , Quaternion.identity , 0 , true );
        }
        
        if ( Input.GetKeyDown( KeyCode.C ) )
        {
            PhotonNetwork.Instantiate( "Raptor1" , new Vector3( 0f , 0f , 0f ) , Quaternion.identity , 0 , true );
        }

        if ( Input.GetMouseButtonDown( 0 ) )
        {
            Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            RaycastHit hit;
            Debug.DrawRay( Camera.main.transform.position , Camera.main.transform.forward , Color.red , 10 );
            if ( Physics.Raycast( ray , out hit , 10 ) )
            {
                print( "Hit " + hit.transform.name );
                PhotonNetwork.Destroy( hit.collider.gameObject );
            }
        }
    }

}
