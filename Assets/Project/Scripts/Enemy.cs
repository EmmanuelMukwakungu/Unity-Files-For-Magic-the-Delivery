using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask GroundLayer, whatIsPlayer;
    
    [Header("Patrol Settings")]
    public List<Transform> patrolPoints = new List<Transform>();
    private int currentPatrolIndex = 0;
    public float patrolPointThreshold;
    
    [Header("Attack Settings")]
    public float damageAmount = 25f;
    public float damageOnEnemy = 2f;
    public float sightRange, attackRange, timeBetweenAttacks;
    public bool playerInSightRange, playerInAttackRange, alreadyAttacked;
    
    
    public void Awake()
    {
        if (player == null) 
        { 
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform; 
                
            } 
        }
        agent = GetComponent<NavMeshAgent>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }

        if (other.CompareTag("Bullet"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageOnEnemy);
            }
        }

    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patrol();
        }

        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }

        if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
        }
    }
    

    private void Patrol()
    {
        if (patrolPoints.Count == 0) return;

        // Move to the current patrol point
        Transform targetPoint = patrolPoints[currentPatrolIndex];
        agent.SetDestination(targetPoint.position);

        // Check if reached patrol point
        if (Vector3.Distance(transform.position, targetPoint.position) < patrolPointThreshold)
        {
            // Move to next point
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        }
    }
    private void ChasePlayer()
    {
        if (player == null) return;
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);
    }
 

    private void OnDrawGizmosSelected()
    {
        // Draw sight and attack ranges
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        // Draw patrol route lines
        if (patrolPoints == null || patrolPoints.Count == 0) return;

        Gizmos.color = Color.cyan;
        for (int i = 0; i < patrolPoints.Count; i++)
        {
            Transform current = patrolPoints[i];
            Transform next = patrolPoints[(i + 1) % patrolPoints.Count];

            if (current != null && next != null)
                Gizmos.DrawLine(current.position, next.position);

            if (current != null)
                Gizmos.DrawSphere(current.position, 0.3f);
        }
    }
}
