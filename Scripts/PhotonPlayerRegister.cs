using UnityEngine;
using System.Collections;

/// <summary>
/// 幫助Avatar註冊，方便管理所有需要控制位置的角色初始化.
/// </summary>
public class PhotonPlayerRegister : Photon.PunBehaviour
{
    private int ownerID;

    void Awake ( )
    {
        //print( photonView.owner );
        ownerID = photonView.owner.ID;
        gameObject.name = gameObject.name + " (" + ownerID + ")";
        PhotonPlayerFinder.RegPlayer( ownerID , transform );
    }
    void OnDestroy ( )
    {
        PhotonPlayerFinder.UnRegPlayer( ownerID );
    }
}
