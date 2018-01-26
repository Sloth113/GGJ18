using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSource : Tower {

    public Tower child;

    public override List<Tower> GetConnections()
    {
        List<Tower> children = new List<Tower>();
        children.Add(child);
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
}
