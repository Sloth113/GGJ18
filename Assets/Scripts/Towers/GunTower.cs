using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTower : AttackTower {

    // Use this for initialization
    protected override void Start () {
        //HACK this is just for the test, before connections are done
        base.Start();
        m_powerInput = 3;
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}

    protected override void ShootEnemy(RobotNavigation enemy)
    {
        base.ShootEnemy(enemy);
        enemy.OnHit(damage);
    }
}
