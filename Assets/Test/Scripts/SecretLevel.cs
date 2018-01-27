using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretLevel : MonoBehaviour {

    public PowerSource source;
    public JunctionTower splitter;
    public AOETower aoe;
    public GunTower gun;

    bool started = false;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (!started)
        {
            splitter.MakeConnection(aoe);
            splitter.MakeConnection(gun);
            source.CalculateChildren();
            source.AddPower(60);
            started = true;
        }
        if (Input.GetButtonDown("Cancel"))
        {
            GameManager.Instance.unxyxxy();
        }
	}
}
