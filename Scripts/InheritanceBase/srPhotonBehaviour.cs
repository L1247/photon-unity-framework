using UnityEngine;


/// <summary>
/// 負責設定 Photon連線時，非MasterClient是否開啟此GameObject的屬性等等
/// </summary>
public class srPhotonBehaviour : Photon.PunBehaviour
{
    public enum MonoType
    {
        Mono,
        IPhoton,
    }

    [Header("Other Client   ")]
    [SerializeField]
    private bool ScriptEnable , GameObjectActive;

    [SerializeField]
    private MonoType m_MonoType ;
    // Use this for initialization
    protected virtual void Awake ( )
    {
        if ( PhotonNetwork.inRoom == false )
            return;
        SetMonoProperty();
    }


    public override void OnJoinedRoom ( )
    {
        SetMonoProperty();
    }

    void SetMonoProperty ( )
    {
        if ( PhotonNetwork.isMasterClient == true )
            return;
        switch ( m_MonoType )
        {
            case MonoType.Mono:
                MonoBehaviour[] monos = GetComponents<MonoBehaviour>();
                srMonoUtility.SetActive( gameObject , GameObjectActive );
                for ( int i = 0 ; i < monos.Length ; i++ )
                {
                    MonoBehaviour mono = monos[ i ];
                    //避免Photon Observe的物件被關閉
                    if ( mono is IPunObservable == false )
                        srMonoUtility.SetEnable( mono , ScriptEnable );
                }
                break;
            case MonoType.IPhoton:
                iPhoton[] iPhotons = GetComponents<iPhoton>();
                for ( int i = 0 ; i < iPhotons.Length ; i++ )
                {
                    iPhoton _iPhoton = iPhotons[ i ];
                    if ( _iPhoton != null )
                    {
                        //print( "Not MasterClient" );
                        MonoBehaviour mono = _iPhoton as MonoBehaviour;
                        srMonoUtility.SetEnable( mono , ScriptEnable );
                        srMonoUtility.SetActive( mono.gameObject , GameObjectActive );
                    }
                }
                break;
            default:
                break;
        }
    }
}