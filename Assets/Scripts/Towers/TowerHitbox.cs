using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHitbox : MonoBehaviour {

    public AttackTower parent;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerStay(Collider other)
    {
        //TODO change this around when enemies tagged/actually made
        RobotNavigation enemy = other.gameObject.GetComponent<RobotNavigation>();
        if (enemy != null)
        {
            if (parent.Powered && parent.ShotReady)
            {
                parent.ShootEnemy(enemy);
            }
        }
    }
}
