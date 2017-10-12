using UnityEngine;

/// <summary>
/// 負責設定 Photon連線時，非MasterClient是否開啟此GameObject的屬性等等
/// </summary>
public class srPhotonBehaviour : Photon.PunBehaviour
{
    [Header("If Not MasterClient")]
    [SerializeField]
    private bool Enable , Active;
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
        iPhoton[] iPhotons = GetComponents<iPhoton>();
        for ( int i = 0 ; i < iPhotons.Length ; i++ )
        {
            iPhoton _iPhoton = iPhotons[ i ];
            if ( _iPhoton != null )
            {
                if ( PhotonNetwork.isMasterClient == false )
                {
                    //print( "Not MasterClient" );
                    MonoBehaviour mono = _iPhoton as MonoBehaviour;
                    srMonoUtility.SetEnable( mono , Enable );
                    srMonoUtility.SetActive( mono.gameObject , Active );
                }
            }
        }
    }
}