
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoConnect : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PhotonNetwork.ConnectUsingSettings("DinoGen2.0");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
