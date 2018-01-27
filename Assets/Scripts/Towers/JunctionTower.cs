using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunctionTower : Tower {

    public Tower connection1;
    public Tower connection2;

    public override List<Tower> GetConnections()
    {
        List<Tower> children = new List<Tower>();
        if (connection1 != null)
        {
            children.Add(connection1);
        }
        if (connection2 != null)
        {
            children.Add(connection2);
        }
        return children;
    }

    // Use this for initialization
    protected override void Start () {
		
	}

    // Update is called once per frame
    protected override void Update () {
		
	}
}
