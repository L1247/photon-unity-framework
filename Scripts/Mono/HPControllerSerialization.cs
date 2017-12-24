using UnityEngine;
/*** 需加入aStar Defin Symbol 才可使用 ***/
#if ( aStar )
[RequireComponent( typeof( PhotonView ) )]
[RequireComponent( typeof( HPController ) )]
[DisallowMultipleComponent]
/// <summary>
/// 配合HPController 序列化MasterClient的HP給每個Client
/// </summary>
public class HPControllerSerialization : MonoBehaviour, IPunObservable
{
    HPController hpController;

    void Awake ( )
    {
        DontDestroyOnLoad( gameObject );
        hpController = GetComponent<HPController>();
    }
    public void OnPhotonSerializeView ( PhotonStream stream , PhotonMessageInfo info )
    {
        if ( stream.isWriting )
        {
            //Debug.Log( hpController.HP );
            stream.SendNext( hpController.HP );
        }
        else
        {
            int hp =  (int)stream.ReceiveNext();
            //Debug.Log( hp );
            hpController.SetHp( hp );
        }
    }
}
#endif