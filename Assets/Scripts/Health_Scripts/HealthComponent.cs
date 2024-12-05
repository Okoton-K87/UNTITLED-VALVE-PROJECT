using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Audio Settings")]
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip damageSound; // Audio clip for damage sound
    public AudioClip deathSound; // Audio clip for death sound
    public float pitchVariation = 0.2f; // Amount of pitch variation for damage sound

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Current health: {currentHealth}");

        PlayDamageSound();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void PlayDamageSound()
    {
        if (audioSource != null && damageSound != null)
        {
            audioSource.pitch = 1.0f + Random.Range(-pitchVariation, pitchVariation);
            audioSource.PlayOneShot(damageSound);
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died!");
        PlayDeathSound();
        Destroy(gameObject); // Remove the enemy from the scene
    }

    private void PlayDeathSound()
    {
        if (audioSource != null && deathSound != null)
        {
            audioSource.pitch = 1.0f; // Reset pitch to normal
            audioSource.PlayOneShot(deathSound);
        }
    }
}
