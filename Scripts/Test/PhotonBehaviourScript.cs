using UnityEngine;
public enum aaa
{
    a,
    b,
    c,
}
[RequireComponent( typeof( PhotonView ) )]
public class PhotonBehaviourScript : Photon.PunBehaviour, IPunObservable
{
    [ClassSelector (typeof( PhotonBehaviour))]
    [SerializeField]
    private string pbEnum;

    public PhotonBehaviour pb;
    //[EnumFlag]
    //public aaa thisEnum;
    //// This lets you give the field a custom name in the inspector.
    //[EnumFlag("Custom Inspector Name")]
    //public aaa anotherEnum;
    //public aaa otherEnum;

    void Awake ( )
    {
        pb = System.Reflection.Assembly.GetExecutingAssembly().CreateInstance( pbEnum ) as PhotonBehaviour;
        pb.Init();
        pb.Awake();

        if ( PhotonNetwork.isMasterClient )
            pb.AwakeIsMasterClient();
        else
            pb.AwakeIsNotMasterClient();

        if ( photonView.isMine )
            pb.AwakeIsMine();
        else
            pb.AwakeIsNotMine();
    }

    // Use this for initialization
    void Start ( )
    {
        //pb = Activator.CreateInstance( type ) as PhotonBehaviour;
        pb.Start();

        if ( PhotonNetwork.isMasterClient )
            pb.StartIsMasterClient();
        else
            pb.StartIsNotMasterClient();
        if ( photonView.isMine )
            pb.StartIsMine();
        else
            pb.StartIsNotMine();

        //print( thisEnum );
        //print( anotherEnum);
        //print( otherEnum );
    }

    public void Hello ( )
    {
        ( pb as TestPhotonBehaviour ).Hello();
    }
    public void Hello2 ( )
    {
        ( pb as TestPhotonBehaviour ).Hello2();
    }

    public override void OnPhotonPlayerConnected ( PhotonPlayer newPlayer )
    {
        foreach ( var item in PhotonNetwork.playerList )
        {
            print( item );
        }
    }

    // Update is called once per frame
    void Update ( )
    {
        pb.Update();

        if ( PhotonNetwork.isMasterClient )
            pb.UpdateIsMasterClient();
        else
            pb.UpdateIsNoMasterClient();

        if ( photonView.isMine )
            pb.UpdateIsMine();
        else
            pb.UpdateIsNotMine();
    }

    void OnTriggerEnter ( Collider other )
    {
        Debug.Log( "OnTriggerEnter" );
        pb.OnTriggerEnter( other );

        if ( PhotonNetwork.isMasterClient )
            pb.OnTriggerEnterIsMasterClient( other );
        else
            pb.OnTriggerEnterIsNotMasterClient(other);

        if ( photonView.isMine )
            pb.OnTriggerEnterIsMine( other );
        else
            pb.OnTriggerEnterIsNotMine(other);
    }

    void OnTriggerExit ( Collider other )
    {
        pb.OnTriggerExit( other );

        if ( PhotonNetwork.isMasterClient )
            pb.OnTriggerExitIsMasterClient( other );
        else
            pb.OnTriggerExitIsNotMasterClient( other );

        if ( photonView.isMine )
            pb.OnTriggerExitIsMine( other );
        else
            pb.OnTriggerExitIsNotMine( other );
    }

    public void BuildType ( )
    {
    }

    public void OnPhotonSerializeView ( PhotonStream stream , PhotonMessageInfo info )
    {
        if ( stream.isWriting )
        {
            pb.StreamSend( stream , info );
        }
        else
        {
            pb.StreamReceive( stream , info );
        }
    }

    void abc ( )
    {
    }
}
