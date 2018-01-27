using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotNavigation : MonoBehaviour {

    //[SerializeField] private Vector3 destination;
    [SerializeField] private int m_speed = 1;
    [SerializeField] private int m_maxHealth = 1;
    [SerializeField] private int m_power = 1;
    [SerializeField] private int m_damageOutput = 1;

    private NavMeshAgent m_agent;
    [SerializeField]
    private int m_health;

    public GameObject endDestination;


    // Use this for initialization
    void Start () {
        m_agent = GetComponent<NavMeshAgent>();
        m_health = m_maxHealth;
        m_agent.speed = m_speed;
        if (endDestination != null)
            m_agent.destination = endDestination.transform.position;

    }

    // Update is called once per frame
    void Update () {
        if (endDestination != null)
            m_agent.destination = endDestination.transform.position;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == endDestination)
        {
            endDestination.GetComponent<PowerSourceHealth>().ApplyDamage(m_damageOutput);
            OnDeath();
            // Destroy itself
            // Game manager call or tower call
        }
    }

    public void OnHit(int damage)
    {
        m_health -= damage;
        if (m_health <= 0)
            OnDeath();
    }

    void OnDeath ()
    {
        // gameManager.addPower(power);
        Destroy(gameObject);
        // Destroy gameObject
        // Explosion particle system
        // Power gain particle system
    }
}
