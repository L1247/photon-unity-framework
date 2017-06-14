
using UnityEngine;

interface IPhotonEvent
{
    //Mono
    void Awake ( );
    void AwakeIsMine ( );
    void AwakeIsNotMine ( );
    void AwakeIsMasterClient ( );
    void AwakeIsNotMasterClient ( );

    void Start ( );
    void StartIsMine ( );
    void StartIsNotMine ( );
    void StartIsMasterClient ( );
    void StartIsNotMasterClient ( );

    void Update ( );
    void UpdateIsMine ( );
    void UpdateIsNotMine ( );
    void UpdateIsMasterClient ( );
    void UpdateIsNoMasterClient ( );

    void OnTriggerEnter ( Collider other );
    void OnTriggerEnterIsMine ( Collider other );
    void OnTriggerEnterIsNotMine ( Collider other );
    void OnTriggerEnterIsMasterClient ( Collider other );
    void OnTriggerEnterIsNotMasterClient ( Collider other );

    void OnTriggerExit ( Collider other );
    void OnTriggerExitIsMine ( Collider other );
    void OnTriggerExitIsNotMine ( Collider other );
    void OnTriggerExitIsMasterClient ( Collider other );
    void OnTriggerExitIsNotMasterClient ( Collider other );

    //IPunObservable
    void StreamSend ( PhotonStream stream , PhotonMessageInfo info );
    void StreamReceive ( PhotonStream stream , PhotonMessageInfo info );
    

}

