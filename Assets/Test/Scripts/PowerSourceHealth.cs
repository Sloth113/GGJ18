using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSourceHealth : MonoBehaviour {

    [SerializeField] private int m_maxHealth;
    private int m_health;

	// Use this for initialization
	void Start () {
        m_health = m_maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ApplyDamage(int damage)
    {
        m_health -= damage;
    }

    public int ShowCurrentHealth()
    {
        return m_health;
    }
}
