using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class GfeController : MonoBehaviour
{
    [Header("Patrol")]
    public Transform[] waypoints;
    public float walkSpeed = 2f;

    [Header("Chase")]
    public float chaseSpeed = 4f;
    public float sightDistance = 10f;
    [Tooltip("Layers that block vision (e.g. walls). Leave Player on Default or a separate layer.")]
    public LayerMask obstacleMask;

    [Header("Attack")]
    [Tooltip("How close the enemy must be to trigger its attack")]
    public float attackRange = 3f;
    [Tooltip("Minimum time between attack animations")]
    public float attackCooldown = 1f;
    [Tooltip("Damage dealt per attack")]
    public int attackDamage = 20;

    private NavMeshAgent agent;
    private Animator animator;
    private Transform player;
    private PlayerHealth playerHealth;
    private Collider playerCollider;
    private int wpIndex = 0;
    private float _lastAttackTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerCollider = playerObj.GetComponent<Collider>();
            playerHealth = playerObj.GetComponent<PlayerHealth>();
        }

        agent.speed = walkSpeed;
        GoToNextWaypoint();
    }

    void Update()
    {
        if (player == null) return;

        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (distToPlayer <= sightDistance && CanSeePlayer())
        {
            // === CHASE ===
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
            animator.SetBool("IsChasing", true);
            animator.SetBool("IsWalking", false);

            // Attack if in range and off cooldown
            if (distToPlayer <= attackRange && Time.time - _lastAttackTime >= attackCooldown)
            {
                _lastAttackTime = Time.time;
                animator.SetTrigger("Attack");  // play attack animation
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                }
            }
        }
        else
        {
            // === PATROL ===
            agent.speed = walkSpeed;
            animator.SetBool("IsChasing", false);
            animator.SetBool("IsWalking", true);

            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GoToNextWaypoint();
        }
    }

    private bool CanSeePlayer()
    {
        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Vector3 target = playerCollider.bounds.center;
        Vector3 direction = (target - origin).normalized;
        float distance = Vector3.Distance(origin, target);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance, ~obstacleMask))
            return hit.collider.CompareTag("Player");

        return false;
    }

    private void GoToNextWaypoint()
    {
        if (waypoints.Length == 0) return;
        wpIndex = (wpIndex + 1) % waypoints.Length;
        agent.SetDestination(waypoints[wpIndex].position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightDistance);

        if (player != null && playerCollider != null)
        {
            Gizmos.color = CanSeePlayer() ? Color.red : Color.green;
            Vector3 origin = transform.position + Vector3.up * 1.5f;
            Vector3 target = playerCollider.bounds.center;
            Gizmos.DrawLine(origin, target);

            // draw attack range
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
