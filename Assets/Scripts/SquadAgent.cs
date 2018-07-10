using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SquadAgent : MonoBehaviour
{
    [SerializeField]
    private Transform targetOffset;

    private NavMeshAgent agent;

    [SerializeField]
    private float maxHealth = 100.0f;
    [SerializeField]
    private float health;

    public Vector3 Target
    {
        set { agent.SetDestination(value + targetOffset.position - targetOffset.parent.position); }
    }

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        health = maxHealth;
    }

    void Update()
    {
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0.0f)
        {
            health = 0.0f;
            Debug.Log(gameObject.name + " died");
            Destroy(gameObject);
        }
    }
}
