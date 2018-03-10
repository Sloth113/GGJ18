using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(AudioSource))]
public class AttackTower : Tower {

    [SerializeField]
    protected AudioClip m_fireAudioClip;
    protected AudioSource m_audioSource;

    public float minPower;
    public float maxPower;
    public float minROF;    // Min shots per second
    public float maxROF;    // Max shots per second
    public float rof;
    public float fireTime;  // Seconds between shots
    public float reloadProgress;    // How long reload is taking

    public int damage;

    public SphereCollider EnemyDetector;

    protected bool powered;

    protected bool shotReady;

    public bool Powered {get{ return powered; }}
    public bool ShotReady { get { return shotReady; } }

    public float range =10;

	private Color originalColour;	// Stores the original colour of the tower


	// Use this for initialization
	protected override void Start () {
        base.Start();
        shotReady = true;
        reloadProgress = 0;
        m_audioSource = GetComponent<AudioSource>();
		// This is used to get the original colour of the tower
		originalColour = GetComponent<Renderer>().material.GetColor("_EmissionColor");
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();

        //TODO change based on scale? just make it big and check range elsewhere?
        EnemyDetector.radius = range;

        // Calculate current rate of fire
        if(m_powerInput >= minPower)
        {
            //TODO maybe have different interpolation function?
            rof = Mathf.Lerp(minROF, maxROF, (m_powerInput - minPower) / (maxPower - minPower));
            fireTime = 1.0f / rof;
            powered = true;
			// Sets the colour of the tower to its original colour when powered
			GetComponent<Renderer> ().material.SetColor ("_EmissionColor", originalColour);
        } else
        {
            powered = false;
            rof = 0;
			// Sets the colour of the tower to black when not powered
			GetComponent<Renderer> ().material.SetColor ("_EmissionColor", Color.black);
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

   

    public virtual void ShootEnemy(RobotNavigation enemy)
    {
        enemy.OnHit(damage);
    }

    protected override void SetRangeCircle()
    {
        SetRangeCirclePoints(range);
    }


    public override string GetInfoText()
    {
        string text = "Cost: " + cost + " Power: " + m_powerInput + " RoF: " + rof + " Dmg: " + damage;
        return text;
    }
}
