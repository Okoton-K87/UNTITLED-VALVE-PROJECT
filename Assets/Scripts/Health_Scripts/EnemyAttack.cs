using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public int damagePerSecond = 10; // Damage to inflict per second
    public float attackInterval = 1f; // Time between attacks in seconds
    public string playerTag = "Player"; // Tag to identify the player

    private float nextAttackTime; // Timer to track when to apply the next attack
    private PlayerHealth playerHealth; // Reference to the player's HealthComponent

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Debug.Log($"Player entered attack zone of {gameObject.name}.");
                nextAttackTime = Time.time; // Reset the attack timer
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(playerTag) && playerHealth != null)
        {
            // Inflict damage at regular intervals
            if (Time.time >= nextAttackTime)
            {
                playerHealth.TakeDamage(damagePerSecond);
                nextAttackTime = Time.time + attackInterval;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag) && playerHealth != null)
        {
            Debug.Log($"Player exited attack zone of {gameObject.name}.");
            playerHealth = null; // Stop attacking when the player leaves the zone
        }
    }
}
