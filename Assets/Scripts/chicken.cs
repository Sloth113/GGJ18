using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chicken : MonoBehaviour {

    public PlaySoundV2 audio;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Caw()
    {
        audio.PlayV2(0);
    }
}
