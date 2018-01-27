using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotNavigation : MonoBehaviour {

    //[SerializeField] private Vector3 destination;
    [SerializeField] private int m_speed = 1;
    [SerializeField] private int m_maxHealth = 1;
    [SerializeField] private int m_power = 1;
    [SerializeField] private int m_damageOutput = 2;
    [SerializeField] private int m_health;

    private NavMeshAgent m_agent;
    private Tower m_newTower;

    private bool m_alive;

    public GameObject endDestination;

    [SerializeField]ParticleSystem damageEffect;
    [SerializeField]ParticleSystem deathEffect;

    // Use this for initialization
    void Start () {
        m_agent = GetComponent<NavMeshAgent>();
        m_health = m_maxHealth;
        m_alive = true;
        m_agent.speed = m_speed;
        if (endDestination != null)
            m_agent.destination = endDestination.transform.position;
        foreach(AttackTower newTower in GameManager.Instance.attackTowers)
        {
            if (newTower.transform.position.x >= -100 && newTower.Powered)
                m_agent.SetAreaCost(4, m_agent.GetAreaCost(4) + 1);
            else if (newTower.transform.position.y < -100 && newTower.Powered)
                m_agent.SetAreaCost(3, m_agent.GetAreaCost(3) + 1);
        }

    }

    // Update is called once per frame

    void Update()
    {
        if (m_alive)
        {
            if (endDestination != null)
            {
                if ((transform.position - endDestination.transform.position).sqrMagnitude <= 400)
                {
                    Debug.Log("Destroy should be called");
                    endDestination.GetComponent<PowerSourceHealth>().ApplyDamage(m_damageOutput);
                    OnDeath();
                }
            }
        }
        else
        {
            if (!deathEffect.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }

    public void OnHit(int damage)
    {
        m_health -= damage;
        if (m_health <= 0)
        {
            if (m_alive)
            {
                GameManager.Instance.power += m_power;
                OnDeath();
            }
        } else
        {
            damageEffect.Play();
            //GameManager.Instance.power += m_power;
        }
    }

    void OnDeath ()
    {
        // gameManager.addPower(power);
        m_alive = false;
        // Disable components
        this.GetComponent<Collider>().enabled = false;
        Destroy(this.GetComponent<Rigidbody>());
        this.GetComponent<NavMeshAgent>().enabled = false;
        this.GetComponentInChildren<MeshRenderer>().enabled = false;
        // Explosion particle system
        deathEffect.Play();
        // Power gain particle system
    }
}


