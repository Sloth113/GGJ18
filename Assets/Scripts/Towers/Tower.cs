using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour {

    protected float m_powerInput;

    public float powerInput { get { return m_powerInput; } }

    [HideInInspector]
    public bool visited = false;    //Has been visited in power graph search
    [HideInInspector]
    public bool inStack = false;    //Is currently in recursion stack for power graph search

    // Towers connected to which this is sending power to
    public List<Tower> children;

	// Use this for initialization
	protected virtual void Start () {
        m_powerInput = 0;
        children = new List<Tower>();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		
	}

    // Returns all towers this tower is trying to connect to
    public abstract List<Tower> GetConnections();

    // Reset current power to 0
    public void ResetPower()
    {
        m_powerInput = 0;
    }

    // Send power to this and its children
    public void AddPower(float power)
    {
        m_powerInput += power;
        float childPower = power / children.Count;
        foreach(Tower child in children)
        {
            child.AddPower(childPower);
        }
    }

    // Recursively determine which connections are downstream
    protected void SetChildren()
    {
        if (!visited)
        {
            visited = true;
            inStack = true;
            foreach(Tower connectedTower in GetConnections())
            {
                if (!connectedTower.inStack)    // If connected tower wasn't passed through to reach this
                {
                    // Add connection to children and set its children
                    children.Add(connectedTower);
                    connectedTower.SetChildren();
                }
            }
            inStack = false;
        }
    }
}
