using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBehavior : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float patrolRadius = 20f; // Radius for random patrol points
    public float waitTimeAtPoint = 2f; // Time to wait at each patrol point
    public bool randomPatrol = true; // Enable random patrol by default

    [Header("Auto Generate Settings")]
    public Transform patrolAreaCenter; // Center of the patrol area

    // Private variables
    private EnemyController enemyController;
    private NavMeshAgent navMeshAgent;
    private bool isWaiting = false;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Set patrol area center to the enemy's position if not assigned
        if (patrolAreaCenter == null)
        {
            patrolAreaCenter = transform;
        }
    }

    private void Start()
    {
        if (enemyController != null)
        {
            StartCoroutine(PatrolCoroutine());
        }
    }

    private IEnumerator PatrolCoroutine()
    {
        while (true)
        {
            // Only patrol when in Patrol state
            if (enemyController.currentState == EnemyState.Patrol)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                {
                    if (!isWaiting)
                    {
                        isWaiting = true;
                        yield return new WaitForSeconds(waitTimeAtPoint);
                        isWaiting = false;

                        SetRandomPatrolPoint();
                    }
                }
            }

            yield return null;
        }
    }

    private void SetRandomPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += patrolAreaCenter.position;

        // Ensure the random position is on the NavMesh
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(hit.position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw patrol area
        Gizmos.color = new Color(0, 1, 0, 0.2f);
        if (patrolAreaCenter != null)
        {
            Gizmos.DrawSphere(patrolAreaCenter.position, patrolRadius);
        }
    }
}