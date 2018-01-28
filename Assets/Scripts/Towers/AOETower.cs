using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOETower : AttackTower {

    public ParticleSystem blast;

    // Use this for initialization
    protected override void Start () {
        base.Start();
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
        // Set blastwave lifetime to fit range
        blast.startLifetime = range / blast.startSpeed;
        blast.Stop();
        blast.Play();
        if (m_fireAudioClip != null)
        {
            SoundManager.instance.PlaySfx(m_fireAudioClip);
        }
        //TODO
    }

    public override void ShootEnemy(RobotNavigation enemy)
    {
        //TODO specific effect for hurting enemy
        base.ShootEnemy(enemy);
    }


}
