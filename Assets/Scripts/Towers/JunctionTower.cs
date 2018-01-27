using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunctionTower : Tower, IConnector {


    public Tower[] connections;
    public int nextConnection;  // Index for next connection to change
    [SerializeField] float connectionRange;

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
        base.Start();
        connections = new Tower[2];
        nextConnection = 0;
	}

    // Update is called once per frame
    protected override void Update () {
        base.Update();
	}

    protected override void SetRangeCircle()
    {
        SetRangeCirclePoints(connectionRange);
    }
}
