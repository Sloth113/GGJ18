﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunctionTower : Tower, IConnector {


    public Tower[] connections;
    public int nextConnection;  // Index for next connection to change
    [SerializeField] float connectionRange;
	private Color originalColour;	// Stores the original colour of the tower

    public override List<Tower> GetConnections()
    {
        List<Tower> l_children = new List<Tower>();
        if (connections[0] != null)
        {
            l_children.Add(connections[0]);
        }
        if (connections[1] != null)
        {
            l_children.Add(connections[1]);
        }
        return l_children;
    }

    public bool MakeConnection(Tower target)
    {
        bool success = false;
		//// Check if target is already connected
		//foreach (Tower t in connections) 
		//{
		//	if (t == target)
		//		return success;
		//}

        // Check if target in range
        Vector3 displacement = target.transform.position - transform.position;
        if (displacement.sqrMagnitude <= connectionRange * connectionRange)
        {
            connections[nextConnection] = target;
            nextConnection = 1 - nextConnection;    // Toggle between 0 and 1
            success = true;
        }
        return success;
    }

    // Use this for initialization
    protected override void Start () {
		// This is used to get the original colour of the tower
		originalColour = GetComponent<Renderer>().material.GetColor("_EmissionColor");
        base.Start();
        connections = new Tower[2];
        nextConnection = 0;
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
