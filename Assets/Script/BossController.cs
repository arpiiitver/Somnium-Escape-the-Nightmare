using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class BossController : MonoBehaviour
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
    [Tooltip("How close the boss must be to trigger its attack")]
    public float attackRange = 3f;
    [Tooltip("Minimum time between attack animations")]
    public float attackCooldown = 1f;
    [Tooltip("Damage dealt per attack")]
    public int attackDamage = 20;

    [Header("Spawn Minions")]
    [Tooltip("Prefab of the minion enemy to spawn")]
    public GameObject minionPrefab;
    [Tooltip("Spawn points for minions")]
    public Transform[] spawnPoints;
    [Tooltip("Number of minions per wave")]
    public int spawnCount = 5;            // now 5 per wave
    [Tooltip("Seconds between spawn waves")]
    public float spawnInterval = 40f;     // 40-second cooldown

    private NavMeshAgent agent;
    private Animator animator;
    private Transform player;
    private PlayerHealth playerHealth;
    private Collider playerCollider;
    private int wpIndex = 0;
    private float _lastAttackTime = 0f;
    private bool _isSpawning = false;

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

            // Attack logic
            if (distToPlayer <= attackRange && Time.time - _lastAttackTime >= attackCooldown)
            {
                _lastAttackTime = Time.time;
                animator.SetTrigger("Attack");
                playerHealth?.TakeDamage(attackDamage);
            }

            // Spawn a single wave if not on cooldown
            if (!_isSpawning)
                StartCoroutine(SpawnMinionWaves());
        }
        else
        {
            // === PATROL ===
            agent.speed = walkSpeed;
            animator.SetBool("IsChasing", false);
            animator.SetBool("IsWalking", true);

            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GoToNextWaypoint();

            // reset spawn lock so next detection after cooldown will trigger again
            // (no need to reset here; coroutine handles it)
        }
    }

    private IEnumerator SpawnMinionWaves()
    {
        _isSpawning = true;

        // Spawn one wave immediately
        for (int i = 0; i < spawnCount; i++)
        {
            var spawnPoint = spawnPoints[i % spawnPoints.Length];
            Instantiate(minionPrefab, spawnPoint.position, spawnPoint.rotation);
        }

        // Wait for 40 seconds before allowing next wave
        yield return new WaitForSeconds(spawnInterval);

        // After cooldown, allow spawning again on next spot
        _isSpawning = false;
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
        // sight radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightDistance);

        // attack range
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // spawn points
        if (spawnPoints != null)
        {
            Gizmos.color = Color.cyan;
            foreach (var sp in spawnPoints)
                Gizmos.DrawSphere(sp.position, 0.3f);
        }
    }
}
