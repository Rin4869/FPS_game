using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;

    [Header("Detection")]
    public float detectionRange = 20f;
    public float attackRange = 2f;

    [Header("Combat")]
    public float attackDamage = 10f;
    public float attackCooldown = 1f;

    private NavMeshAgent agent;

    private float attackTimer;

    private enum State
    {
        Idle,
        Chase,
        Attack
    }

    private State currentState = State.Idle;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    void Update()
    {
        if (player == null)
            return;

        attackTimer -= Time.deltaTime;

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        if (distance > detectionRange)
        {
            currentState = State.Idle;
        }
        else if (distance > attackRange)
        {
            currentState = State.Chase;
        }
        else
        {
            currentState = State.Attack;
        }

        HandleState();
    }

    void HandleState()
    {
        switch (currentState)
        {
            case State.Idle:
                Idle();
                break;

            case State.Chase:
                Chase();
                break;

            case State.Attack:
                Attack();
                break;
        }
    }

    void Idle()
    {
        agent.isStopped = true;
    }

    void Chase()
    {
        agent.isStopped = false;

        agent.SetDestination(player.position);
    }

    void Attack()
    {
        agent.isStopped = true;

        LookAtPlayer();

        if (attackTimer <= 0f)
        {
            DealDamage();

            attackTimer = attackCooldown;
        }
    }

    void LookAtPlayer()
    {
        Vector3 direction =
            player.position -
            transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion rotation =
                Quaternion.LookRotation(direction);

            transform.rotation =
                Quaternion.Slerp(
                    transform.rotation,
                    rotation,
                    10f * Time.deltaTime
                );
        }
    }

    void DealDamage()
    {
        PlayerHealth playerHealth =
            player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(
                attackDamage
            );
        }

        Debug.Log(
            "Enemy attacked Player for " +
            attackDamage +
            " damage"
        );
    }
}