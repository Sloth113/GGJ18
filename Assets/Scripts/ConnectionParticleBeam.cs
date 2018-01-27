using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionParticleBeam : MonoBehaviour {

    public LineRenderer beam;
    public Tower target;
    public Tower parent;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void Orient()
    {
        beam.SetPosition(0, parent.beamEndpoint.transform.position);
        beam.SetPosition(1, target.beamEndpoint.transform.position);
    }
}
