using UnityEngine;
using UnityEditor;

public class NetworkSetting : EditorWindow
{
    string IpAddress;
    string MinPlayerPerRoom;
    // The position of the window
    public Rect windowRect = new Rect( 100 , 100 , 200 , 200 );
    string NowIPAddress;

    void OnEnable ( )
    {
        IpAddress = Common.ReadPhotonServerIPAddress();
        //MinPlayerPerRoom = Common.ReadMinPlayerPerRoom();
        NowIPAddress = Network.player.ipAddress;
        
    }

    void OnDisable ( )
    {
        PhotonNetwork.PhotonServerSettings.ServerAddress = IpAddress;
        Common.WritePhotonServerIPAddress( IpAddress );
    }

    void OnGUI ( )
    {
        BeginWindows();

        GUILayout.Label( "Photon Server Address Is : " + PhotonNetwork.PhotonServerSettings.ServerAddress );
        IpAddress = GUILayout.TextField( IpAddress , 17 );
        bool b_CloseWindow = GUILayout.Button( "Use This IPAddress" );
        if ( b_CloseWindow )
            this.Close();
        GUILayout.Space( 20 );

        GUILayout.Label( "Now IPAddress Is : " + NowIPAddress );
        bool b_UseNowIpAddress = GUILayout.Button( "Use Now IPAddress For Photon" );
        if ( b_UseNowIpAddress )
        {
            IpAddress = NowIPAddress;
            this.Close();
        }

        GUILayout.Space( 20 );

        //GUILayout.Label( "Now MinPlayerPerRoom is : " );
        //MinPlayerPerRoom = GUILayout.TextField( MinPlayerPerRoom , 1 );
        EndWindows();
    }

    // Add menu item to show this demo.
    [MenuItem( "ccTools/Network Setting _F12" )]
    static void Init ( )
    {
        EditorWindow.GetWindow( typeof( NetworkSetting ) );
    }

}