using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    
    private Transform[] patrolPoints;
    private float patrolSpeed = 2f;
    private int currentPatrolIndex;

    private float chaseSpeed = 4f;
    private float detectionRange = 10f;
    private float stopChaseDistance = 15f;

    private bool isChasing;
    
    public float damageAmount = 25f;

    private void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        if (patrolPoints.Length > 0)
        {
            currentPatrolIndex = 0;
            //GoToNextPatrolPoint(); 
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= detectionRange)
        {
            //StopChasing();
        }
        
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }
}
