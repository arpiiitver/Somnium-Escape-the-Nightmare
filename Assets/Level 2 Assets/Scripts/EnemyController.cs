using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class EnemyController : MonoBehaviour
{
    [Header("Patrol")]
    public Transform[] waypoints;
    public float walkSpeed = 2f;

    [Header("Chase")]
    public float chaseSpeed = 4f;
    public float sightDistance = 10f;
    [Tooltip("Layers that block vision (e.g. walls). Leave Player on Default or a separate layer.")]
    public LayerMask obstacleMask;

    [Header("Audio")]
    public AudioClip spotSound;           // Sound to play when spotting the player

    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audioSource;
    private Transform player;
    private Collider playerCollider;
    private int wpIndex = 0;
    private bool hasSpotted = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerCollider = playerObj.GetComponent<Collider>();
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
            if (!hasSpotted)
            {
                audioSource.PlayOneShot(spotSound);
                hasSpotted = true;
            }

            // === CHASE ===
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
            animator.SetBool("IsChasing", true);
            animator.SetBool("IsWalking", false);
        }
        else
        {
            // reset spotting flag so sound can play again on next detection
            hasSpotted = false;

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
        {
            return hit.collider.CompareTag("Player");
        }
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
        }
    }
}
