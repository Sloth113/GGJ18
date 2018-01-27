using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributionTower : Tower {

    public List<Tower> connections;

    public float range;

    public override List<Tower> GetConnections()
    {
        return connections;
    }

    // Use this for initialization
    protected override void Start () {
        base.Start();
	}

    // Update is called once per frame
    protected override void Update () {
        base.Update();
	}

    public void CalculateConnections()
    {
        connections = new List<Tower>();
        //TODO change how this works when we know more about towers
        Collider[] neighbours = Physics.OverlapSphere(transform.position, range);
        foreach(Collider c in neighbours)
        {
            Tower tower = c.gameObject.GetComponent<Tower>();
            if (tower != null && tower != this)
            {
                connections.Add(tower);
            }
        }
    }

    protected override void SetRangeCircle()
    {
        SetRangeCirclePoints(range);
    }

}
