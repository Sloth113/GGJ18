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

    public int damage;

    public SphereCollider EnemyDetector;

    protected bool powered;

    protected bool shotReady;

    public float range =10;


	// Use this for initialization
	protected override void Start () {
        base.Start();
        shotReady = true;
        reloadProgress = 0;
	}
	
	// Update is called once per frame
	protected override void Update () {

        //TODO change based on scale? just make it big and check range elsewhere?
        EnemyDetector.radius = range;

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

    public void OnTriggerStay(Collider other)
    {
        //TODO change this around when enemies tagged/actually made
        RobotNavigation enemy = other.gameObject.GetComponent<RobotNavigation>();
        if (enemy != null)
        {
            if (powered && shotReady)
            {
                ShootEnemy(enemy);
            }
        }
    }

    protected virtual void ShootEnemy(RobotNavigation enemy)
    {
        enemy.OnHit(damage);
    }

}
