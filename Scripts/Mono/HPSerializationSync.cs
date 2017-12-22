
using UnityEngine;

[RequireComponent( typeof( PhotonView ) )]
[DisallowMultipleComponent]
public class HPSerializationSync : MonoBehaviour,IPunObservable
{
    HPController    hpController;
    void Awake()
    {
        hpController = GetComponent<HPController>();
    }
    public void OnPhotonSerializeView ( PhotonStream stream , PhotonMessageInfo info )
    {
        if(stream.isWriting)
        {
            //Debug.Log( hpController.HP );
            stream.SendNext( hpController.HP );
        }else
        {
            int hp =  (int)stream.ReceiveNext();
            //Debug.Log( hp );
            hpController.SetHp( hp );
        }

        //if ( stream.isReading )
        //{
        //    int hp =  (int)stream.ReceiveNext();
        //    Debug.Log( hp );
        //    hpController.HP = hp;
        //}
    }
}
