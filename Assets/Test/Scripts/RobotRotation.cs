using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotRotation : MonoBehaviour {

    private Vector3 transformAngle;

	// Use this for initialization
	void Start () {
        transformAngle = transform.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
        transformAngle = transform.eulerAngles;

        transformAngle.y += Mathf.Sin(Time.deltaTime) * Mathf.Rad2Deg * 10;
        transform.eulerAngles = transformAngle;
	}
}
