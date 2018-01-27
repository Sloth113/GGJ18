using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSource : Tower, IConnector
{

    public Tower child;

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

    // Use this for initialization
    protected override void Start () {
        base.Start();
	}

    // Update is called once per frame
    protected override void Update () {
        base.Update();
	}

 
    public void CalculateChildren()
    {
        distanceFromSource = 0;
        SetChildren();
    }

    public bool MakeConnection(Tower target)
    {
        bool success = false;
        // Check if target in range
        Vector3 displacement = target.transform.position - transform.position;
        if (displacement.sqrMagnitude <= connectionRange * connectionRange)
        {
            child = target;
            success = true;
        }
        return success;
    }
}
