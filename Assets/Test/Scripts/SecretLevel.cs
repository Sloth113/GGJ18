using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretLevel : MonoBehaviour {

    public PowerSource source;
    public JunctionTower splitter;
    public AOETower aoe;
    public GunTower gun;

    public Animator chickAnimator;

    float sceneTime;

    bool started = false;

	// Use this for initialization
	void Start () {
        sceneTime = 0;
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

        sceneTime += Time.deltaTime;

        if(sceneTime >= 3.0f)
        {
            chickAnimator.SetTrigger("face_camera");
        }
        if (sceneTime >= 5.0f)
        {
            GameManager.Instance.unxyxxy();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            GameManager.Instance.unxyxxy();
        }
	}
}
