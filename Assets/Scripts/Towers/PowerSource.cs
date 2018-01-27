using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSource : Tower, IConnector
{

    public Tower child;

    [SerializeField] float connectionRange;

    public override List<Tower> GetConnections()
    {
        List<Tower> children = new List<Tower>();
        if (child != null)
        {
            children.Add(child);
        }
        return children;
    }

    // Use this for initialization
    protected override void Start () {
		
	}

    // Update is called once per frame
    protected override void Update () {
		
	}

 
    public void CalculateChildren()
    {
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
