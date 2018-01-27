using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTower : AttackTower {

    public LineRenderer laser;
    public GameObject laserOrigin;

    [SerializeField] float laserDisplayTime = 0.2f;
    [SerializeField] float laserTimer = 0.0f;
    [SerializeField] bool laserShowing = false;

    // Use this for initialization
    protected override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
        if (laserShowing)
        {
            laserTimer += Time.deltaTime;
            if(laserTimer >= laserDisplayTime)
            {
                laserShowing = false;
                laserTimer = 0;
                laser.gameObject.SetActive(false);
            }
        }
	}

    protected override void ShootEnemy(RobotNavigation enemy)
    {
        shotReady = false;
        reloadProgress = 0;
        Debug.Log("Shots fired");
        ShowLaser(enemy.gameObject.transform.position);
        base.ShootEnemy(enemy);
        //TODO play shooting noise
    }

    private void ShowLaser(Vector3 target)
    {
        laser.SetPosition(0, laserOrigin.transform.position);
        laser.SetPosition(1, target);
        laserTimer = 0;
        laserShowing = true;
        laser.gameObject.SetActive(true);
    }
}
