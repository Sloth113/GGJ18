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
        foreach(Tower t in connections)
        {
            if(t != null)
            {
                l_children.Add(t);
            }
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
            nextConnection = (nextConnection + 1) %3;    // Toggle between 0 and 1 and 2
            success = true;
        }
        return success;
    }

    // Use this for initialization
    protected override void Start () {
        base.Start();
        connections = new Tower[3];
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
