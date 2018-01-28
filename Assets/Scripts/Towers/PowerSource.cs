using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSource : Tower, IConnector
{

    public Tower child;

    [SerializeField] float connectionRange;

    public float powerUsed;

    public GameObject destroyEffect;
    public float explosionDelay = 1.0f;

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

        if(destroyEffect == null && explosionDelay <= 0)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - Time.deltaTime * 10, this.transform.position.z);
        }
        else if (destroyEffect == null)
        {
            explosionDelay -= Time.deltaTime;
        }
        if(this.transform.position.y < 10)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
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

    protected override void SetRangeCircle()
    {
        SetRangeCirclePoints(connectionRange);
    }

    public float GetPowerReq()
    {
        float power = 0;
        List<Tower> fullList = GetAllChildren();
        Debug.Log(fullList.Count);
        foreach (Tower t in fullList)
        { 
            if(t is AttackTower)
            {
                power += (t as AttackTower).minPower;
            }
        }
        return power;
    }
    public List<Tower> GetAllChildren()
    {
        List<Tower> list = new List<Tower>();
        List<Tower> nodesToVisit = new List<Tower>();
        Tower currentChild = null;
        nodesToVisit.Add(this);
        
        if (GetConnections().Count > 0)
        {
            currentChild = child;

            //Get children

            while (nodesToVisit.Count > 0 )
            {
                currentChild = nodesToVisit[0];
                nodesToVisit.Remove(currentChild);
                foreach (Tower t in currentChild.GetConnections())
                    if((!list.Contains(t)))
                        nodesToVisit.Add(t);
                list.Add(currentChild);
                
            }
        }
        
        return list;
    }

    public void DestroyTower()
    {
        Instantiate<GameObject>(destroyEffect, new Vector3(this.transform.position.x, this.transform.position.y - 5, this.transform.position.z), this.transform.rotation);
        destroyEffect = null;
    }
}
