using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOETower : AttackTower {

    // Use this for initialization
    protected override void Start () {
		
	}
	
	// Update is called once per frame
	protected override void Update () {
        // start with shot not ready, if reload finishes
        shotReady = false;
        base.Update();
        // If reload finished this update, do AOE firing effect
        if (shotReady)
        {
            FireAOE();
        }
	}

    void FireAOE()
    {
        Debug.Log("AOE fired");
        //TODO
    }

    protected override void ShootEnemy(RobotNavigation enemy)
    {
        //TODO specific effect for hurting enemy
    }
}
