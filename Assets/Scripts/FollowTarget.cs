using UnityEngine;
using UnityEngine.AI;

public class FollowTarget : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private Animator animator;
    [SerializeField] private bool alwaysFollowPlayer = false;
    [SerializeField] private bool isInvincible = false;

    [Header("Settings")]
    [SerializeField] private float maxChaseDistance = 10f;
    [SerializeField] private float idleDuration = 1.5f;
    [SerializeField] private float destroyDelay = 3.0f;

    private int curWayIndex = 0;
    private const float respawnDistance = 7f;
    private Transform hostileTarget;

    private AIState state = AIState.Wander;
    private float idleTimer;
    private bool isDead = false;


    void Start()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            state = AIState.Idle;
            Debug.LogWarning($"{name}: No waypoints assigned. AI will remain idle.");
            return;
        }

        agent.SetDestination(waypoints[curWayIndex].position);
    }


    void Update()
    {
        if (isDead)
            return;

        switch (state)
        {
            case AIState.Idle:
                HandleIdle();
                break;

            case AIState.Wander:
                HandleWander();
                break;

            case AIState.Chase:
                HandleChase();
                break;
        }

        UpdateAnimator();
    }

    private void HandleIdle()
    {
        idleTimer -= Time.deltaTime;

        if (idleTimer <= 0f)
        {
            if (waypoints != null && waypoints.Length > 0)
            {
                state = AIState.Wander;
                agent.SetDestination(waypoints[curWayIndex].position);
            }
        }
    }


    private void HandleWander()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            state = AIState.Idle;
            return;
        }

        // Single waypoint logic
        if (waypoints.Length == 1)
        {
            if (!agent.pathPending &&
                agent.remainingDistance <= agent.stoppingDistance)
            {
                state = AIState.Idle;
                agent.ResetPath();
            }
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            state = AIState.Idle;
            idleTimer = idleDuration;

            agent.ResetPath();

            curWayIndex = (curWayIndex + 1) % waypoints.Length;
        }

    }



    private void HandleChase()
    {
        if (hostileTarget == null)
        {
            SwitchToIdle();
            return;
        }

        if (agent.destination != hostileTarget.position)
        {
            agent.SetDestination(hostileTarget.position);
        }

        float distance = GetVectorDistanceWithoutY(transform.position, hostileTarget.position);

        if (!alwaysFollowPlayer && distance >= maxChaseDistance)
        {
            SwitchToIdle();
        }
    }


    private void SwitchToIdle()
    {
        hostileTarget = null;

        state = AIState.Idle;
        idleTimer = idleDuration;

        agent.ResetPath();
    }




    public void FoundHostile(Transform hostile)
    {
        hostileTarget = hostile;
        state = AIState.Chase;
    }

    private void UpdateAnimator()
    {
        if (state == AIState.Idle)
        {
            animator.SetInteger("State", 0);
            return;
        }

        float speed = agent.velocity.magnitude;

        if (speed < 0.1f)
        {
            animator.SetInteger("State", 0);
        }
        else if (state == AIState.Chase)
        {
            animator.SetInteger("State", 2);
        }
        else
        {
            animator.SetInteger("State", 1);
        }
    }

    private float GetVectorDistanceWithoutY(Vector3 origin, Vector3 destination)
    {
        destination = new Vector3(destination.x, origin.y, destination.z);
        return Vector3.Distance(origin, destination);
    }

    public void Die()
    {
        if (isInvincible) return;
        if (isDead) return;

        isDead = true;

        if (agent.enabled && agent.isOnNavMesh)
            agent.ResetPath();

        agent.enabled = false;

        animator.SetTrigger("DeathTrigger");

        Destroy(gameObject, destroyDelay);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();

            if (player != null)
            {
                player.Die();
            }
        }
    }

    public void ResetAgent()
    {
        agent.ResetPath();
        agent.Warp(transform.position);
    }

    public void RespawnBehindPlayer(Transform player, float distance = respawnDistance)
    {
        if (!alwaysFollowPlayer) return;
        if (player == null) return;

        Vector3 spawnPos = player.position - player.forward * distance;

        if (agent.enabled && agent.isOnNavMesh)
        {
            agent.Warp(spawnPos); 
        }
        else
        {
            transform.position = spawnPos;
        }

        hostileTarget = player;
        state = AIState.Chase;
    }


}

public enum AIState
{
    Idle = 0,
    Wander = 1,
    Chase = 2
}

