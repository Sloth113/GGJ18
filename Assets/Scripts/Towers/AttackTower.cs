using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTower : Tower {

    public float minPower;
    public float maxPower;
    public float minROF;    // Min shots per second
    public float maxROF;    // Max shots per second
    public float fireTime;  // Seconds between shots
    public float reloadProgress;    // How long reload is taking

    bool powered;

    bool shotReady;

    public float range;


	// Use this for initialization
	void Start () {
        shotReady = true;
        reloadProgress = 0;
	}
	
	// Update is called once per frame
	void Update () {

        // Calculate current rate of fire
        if(m_powerInput >= minPower)
        {
            //TODO maybe have different interpolation function?
            float rof = Mathf.Lerp(minROF, maxROF, (m_powerInput - minPower) / (maxPower - minPower));
            fireTime = 1.0f / rof;
            powered = true;
        } else
        {
            powered = false;
        }

        // Reload attack
        if (powered && !shotReady)
        {
            reloadProgress += Time.deltaTime;
            if(reloadProgress >= fireTime)
            {
                reloadProgress = 0;
                shotReady = true;
            }
        }
	}

    public override List<Tower> GetConnections()
    {
        return new List<Tower>();
    }
}
