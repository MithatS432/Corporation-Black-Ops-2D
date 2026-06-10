using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Reference")]
    private Rigidbody2D rb;
    private Transform player;

    [Header("Enemy Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float detectionRange = 5f;
    private float searchCooldown = 0.5f;
    private float lastSearchTime;

    [Header("Obstacle Avoidance (Raycast)")]
    [SerializeField] private float obstacleCheckDistance = 1.2f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float avoidanceForce = 1.5f;

    [SerializeField] private LayerMask wallLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandlePlayerSearching();

        if (player == null)
            return;

        InvisibilityAbility invisibility =
            player.GetComponent<InvisibilityAbility>();

        if (invisibility != null && invisibility.isInvisible)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float distanceToPlayer =
            Vector2.Distance(transform.position, player.position);

        if (
            distanceToPlayer <= detectionRange
            &&
            CanSeePlayer()
        )
        {
            MoveTowardsPlayer();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void HandlePlayerSearching()
    {
        if (player == null && Time.time > lastSearchTime + searchCooldown)
        {
            lastSearchTime = Time.time;
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                Debug.Log($"{gameObject.name} Player'ı buldu!");
            }
        }
    }

    #region  Obstacle Avoidance (Raycast)
    bool CanSeePlayer()
    {
        if (player == null)
            return false;

        Vector2 direction =
            player.position - transform.position;

        RaycastHit2D hit =
            Physics2D.Raycast(
                transform.position,
                direction.normalized,
                direction.magnitude,
                wallLayer
            );

        Debug.DrawRay(
            transform.position,
            direction,
            hit.collider == null ? Color.green : Color.red
        );

        return hit.collider == null;
    }
    void MoveTowardsPlayer()
    {
        Vector2 targetDirection = (player.position - transform.position).normalized;
        Vector2 finalDirection = targetDirection;

        Vector2 leftRayDir = Quaternion.Euler(0, 0, 35) * targetDirection;
        Vector2 rightRayDir = Quaternion.Euler(0, 0, -35) * targetDirection;

        RaycastHit2D hitCenter = Physics2D.Raycast(transform.position, targetDirection, obstacleCheckDistance, obstacleLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, leftRayDir, obstacleCheckDistance * 0.8f, obstacleLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, rightRayDir, obstacleCheckDistance * 0.8f, obstacleLayer);

        Debug.DrawRay(transform.position, targetDirection * obstacleCheckDistance, hitCenter.collider ? Color.red : Color.green);
        Debug.DrawRay(transform.position, leftRayDir * (obstacleCheckDistance * 0.8f), hitLeft.collider ? Color.red : Color.green);
        Debug.DrawRay(transform.position, rightRayDir * (obstacleCheckDistance * 0.8f), hitRight.collider ? Color.red : Color.green);

        if (hitCenter.collider != null)
        {
            if (hitLeft.collider == null)
            {
                finalDirection += leftRayDir * avoidanceForce;
            }
            else if (hitRight.collider == null)
            {
                finalDirection += rightRayDir * avoidanceForce;
            }
            else
            {
                finalDirection += hitCenter.normal * avoidanceForce;
            }
        }
        else if (hitLeft.collider != null && hitLeft.collider.CompareTag("Object"))
        {
            finalDirection += rightRayDir * avoidanceForce;
        }
        else if (hitRight.collider != null && hitRight.collider.CompareTag("Object"))
        {
            finalDirection += leftRayDir * avoidanceForce;
        }

        finalDirection = finalDirection.normalized;
        rb.linearVelocity = finalDirection * moveSpeed;
    }
    #endregion




    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Health playerHealth = other.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.GetDamage();
            }
        }
    }
}