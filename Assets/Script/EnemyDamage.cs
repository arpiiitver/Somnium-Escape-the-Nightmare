using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRange = 1.5f;      // Distance within which enemy can hit
    public float attackInterval = 1f;     // Seconds between hits
    public int damage = 20;               // Damage per hit

    private Transform player;
    private PlayerHealth playerHealth;
    private float lastAttackTime;
    private Animator animator;

    void Start()
    {
        // Find player by tag and get its health component
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<PlayerHealth>();
        }
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null || playerHealth == null) return;

        // Check distance and attack timing
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange && Time.time - lastAttackTime >= attackInterval)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    void Attack()
    {
        // Trigger attack animation
        if (animator != null)
            animator.SetTrigger("AttackTrigger");

        // Deal damage to player
        playerHealth.TakeDamage(damage);
    }
}
