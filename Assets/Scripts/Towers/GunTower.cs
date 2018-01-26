using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTower : AttackTower {

    // Use this for initialization
    protected override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}

    protected override void ShootEnemy(RobotNavigation enemy)
    {
        shotReady = false;
        reloadProgress = 0;
        Debug.Log("Shots fired");
    }
}
