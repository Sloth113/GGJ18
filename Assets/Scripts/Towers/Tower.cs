using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour {

    public float cost; // Power cost to build
    [SerializeField]protected float m_powerInput;

    public float powerInput { get { return m_powerInput; } }

    [HideInInspector]
    public bool visited = false;    //Has been visited in power graph search
    [HideInInspector]
    public bool inStack = false;    //Is currently in recursion stack for power graph search

    // Towers connected to which this is sending power to
    public List<Tower> children;

    public GameObject particleBeam;

    public LineRenderer rangeCircle;
    [SerializeField] int circleSegments = 50;

    public GameObject beamEndpoint;

    public GameObject selectionPlane;
    public float rotationRate;

    public List<ConnectionParticleBeam> beams;

    public virtual void Awake()
    {
        SetRangeCircle();
    }

	// Use this for initialization
	protected virtual void Start () {
        m_powerInput = 0;
        children = new List<Tower>();
	}
	
	// Update is called once per frame
	protected virtual void Update () {

        if(GameManager.Instance.selectedTower == gameObject)
        {
            selectionPlane.SetActive(true);
            selectionPlane.transform.Rotate(new Vector3(0, Time.deltaTime * rotationRate, 0));
            rangeCircle.gameObject.SetActive(true);
        } else
        {
            selectionPlane.SetActive(false);
            rangeCircle.gameObject.SetActive(false);
        }
        // Manage connection beams
        HashSet<Tower> beamTargets = new HashSet<Tower>(children);
        List<ConnectionParticleBeam> currentBeams = new List<ConnectionParticleBeam>(beams);
        foreach(ConnectionParticleBeam beam in currentBeams)
        {
            if (beamTargets.Contains(beam.target))
            {
                beamTargets.Remove(beam.target);
            } else
            {
                beams.Remove(beam);
                Object.Destroy(beam.gameObject);

            }
        }
        foreach(Tower target in beamTargets)
        {
            GameObject beam = GameObject.Instantiate(particleBeam);
            ConnectionParticleBeam beamComponent = beam.GetComponent<ConnectionParticleBeam>();
            if(beamComponent != null)
            {
                beams.Add(beamComponent);
                beamComponent.parent = this;
                beamComponent.target = target;
                beamComponent.Orient();
            }
        }
	}

    // Returns all towers this tower is trying to connect to
    public abstract List<Tower> GetConnections();

    // Reset current power to 0
    public void ResetPower()
    {
        m_powerInput = 0;
    }

    // Send power to this and its children
    public void AddPower(float power)
    {
        m_powerInput += power;
        if (children.Count > 0)
        {
            float childPower = power / children.Count;
            foreach (Tower child in children)
            {
                child.AddPower(childPower);
            }
        }
    }

    // Recursively determine which connections are downstream
    protected void SetChildren()
    {
        if (!visited)
        {
            visited = true;
            inStack = true;
            foreach(Tower connectedTower in GetConnections())
            {
                if (!connectedTower.inStack)    // If connected tower wasn't passed through to reach this
                {
                    // Add connection to children and set its children
                    children.Add(connectedTower);
                    connectedTower.SetChildren();
                }
            }
            inStack = false;
        }
    }

    protected abstract void SetRangeCircle();

    protected void SetRangeCirclePoints(float radius)
    {
        float x, z;
        float angle = 0;
        float increment = 360f / circleSegments;
        rangeCircle.positionCount = circleSegments + 1;
        rangeCircle.useWorldSpace = false;
        for(int i = 0; i < (circleSegments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            rangeCircle.SetPosition(i, new Vector3(x, 0, z));
            angle += increment;
        }
    }

}
