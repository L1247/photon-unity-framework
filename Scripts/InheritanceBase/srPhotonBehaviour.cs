using UnityEngine;

public class srPhotonBehaviour : Photon.PunBehaviour
{
    [Header("If Not MasterClient")]
    [SerializeField]
    private bool bEnable , bActive;
    // Use this for initialization
    protected virtual void Awake ( )
    {
        iPhoton _iPhoton = GetComponent<iPhoton>();
        if ( _iPhoton != null )
        {
            if ( PhotonNetwork.isMasterClient == false )
            {
                print( "Not MasterClient" );
                MonoBehaviour mono = _iPhoton as MonoBehaviour;
                srMonoUtility.SetEnable( mono , bEnable );
                srMonoUtility.SetActive( mono.gameObject , bActive );
            }
        }
    }
}
