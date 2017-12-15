using UnityEngine;
using System.Collections;
using System;
using UniRx;
using UnityEngine.UI;

/// <summary>
/// Display Photon Connect Ping
/// </summary>
public class DisplayPing : MonoBehaviour
{
    private Text displayText;
    // Use this for initialization
    void Start ( )
    {
        displayText = GetComponent<Text>();
        if ( displayText )
        {
            var intervalStream = Observable.Interval(TimeSpan.FromSeconds(1)).Publish().RefCount();
            intervalStream
                .Subscribe( x => displayText.text =  string.Format( "Ping : {0} ms" , PhotonNetwork.GetPing().ToString() ) );
        }
    }
}
