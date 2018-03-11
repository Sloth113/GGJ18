using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelayTower : Tower, IConnector
{

    public Tower child;
	private Color originalColour;	// Stores the original colour of the tower


    [SerializeField] float connectionRange;

    public override List<Tower> GetConnections()
    {
        List<Tower> l_children = new List<Tower>();
        if (child != null)
        {
            l_children.Add(child);
        }
        return l_children;
    }

    public bool MakeConnection(Tower target)
    {
        bool success = false;
        // Check if target in range
        Vector3 displacement = target.transform.position - transform.position;
        if(displacement.sqrMagnitude <= connectionRange * connectionRange)
        {
            child = target;
            success = true;
        }
        return success;
    }

    // Use this for initialization
    protected override void Start () {
		// This is used to get the original colour of the tower
		originalColour = GetComponent<Renderer>().material.GetColor("_EmissionColor");
        base.Start();
	}

    // Update is called once per frame
    protected override void Update () {
        base.Update();
		if(m_powerInput > 0)
		{
			// Sets the colour of the tower to its original colour when powered
			GetComponent<Renderer> ().material.SetColor ("_EmissionColor", originalColour);
		} else
		{
			// Sets the colour of the tower to black when not powered
			GetComponent<Renderer> ().material.SetColor ("_EmissionColor", Color.black);
		}
	}

    protected override void SetRangeCircle()
    {
        SetRangeCirclePoints(connectionRange);
    }

}
