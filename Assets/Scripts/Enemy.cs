using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    [Header("Stats")]
    public float health = 100f;

    private bool isDead;

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    private EnemyWeapon weapon;

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("PlayerObj").transform;

        agent = GetComponent<NavMeshAgent>();
        weapon = GetComponent<EnemyWeapon>();

        if (weapon != null)
        {
            weapon.player = player;
        }
    }

    private void Update()
    {
        if (isDead)
            return;

        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(
            transform.position,
            sightRange,
            whatIsPlayer);

        playerInAttackRange = Physics.CheckSphere(
            transform.position,
            attackRange,
            whatIsPlayer);

        if (!playerInSightRange &&
            !playerInAttackRange)
        {
            Patroling();
        }

        if (playerInSightRange &&
            !playerInAttackRange)
        {
            ChasePlayer();
        }

        if (playerInAttackRange &&
            playerInSightRange)
        {
            AttackPlayer();
        }
    }

    private void Patroling()
    {
        if (!walkPointSet)
            SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(
                walkPoint);

        Vector3 distanceToWalkPoint =
            transform.position -
            walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ =
            Random.Range(
                -walkPointRange,
                walkPointRange);

        float randomX =
            Random.Range(
                -walkPointRange,
                walkPointRange);

        walkPoint = new Vector3(
            transform.position.x + randomX,
            transform.position.y,
            transform.position.z + randomZ);

        if (Physics.Raycast(
            walkPoint,
            -transform.up,
            2f,
            whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(
            player.position);
    }

    private void AttackPlayer()
    {
        // Stop moving
        agent.SetDestination(
            transform.position);

        // Face player
        transform.LookAt(player);

        // Shoot
        if (weapon != null)
        {
            weapon.TryShoot();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        health -= damage;

        Debug.Log(
            gameObject.name +
            " HP: " +
            health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        Debug.Log(
            gameObject.name +
            " died.");

        if (agent != null)
        {
            agent.enabled = false;
        }

        if (weapon != null)
        {
            weapon.enabled = false;
        }

        Destroy(
            gameObject,
            0.5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color =
            Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            attackRange);

        Gizmos.color =
            Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            sightRange);
    }
}