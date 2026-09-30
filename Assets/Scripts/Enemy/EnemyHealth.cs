using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;

    [Header("Hit Reaction")]
    public float knockbackForce = 3f;

    private float currentHealth;

    private NavMeshAgent agent;

    private EnemySpawner spawner;

    void Start()
    {
        currentHealth = maxHealth;

        agent = GetComponent<NavMeshAgent>();

        spawner =
            FindFirstObjectByType<EnemySpawner>();
    }

    public void TakeDamage(
        float damage,
        Vector3 hitDirection)
    {
        currentHealth -= damage;

        Debug.Log(
            "Enemy HP: " +
            currentHealth
        );

        HitReaction(hitDirection);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void HitReaction(Vector3 direction)
    {
        if (agent == null)
            return;

        agent.velocity = direction * knockbackForce;
    }

    void Die()
    {
        Debug.Log("Enemy defeated!");

        if (spawner != null)
        {
            spawner.EnemyDefeated();
        }

        Destroy(gameObject);
    }
}