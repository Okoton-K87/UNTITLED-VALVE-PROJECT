using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth = 100;
    public Slider slider;

    private void Start()
    {
        currentHealth = maxHealth;
        
    }
    private void Update()
    {
        slider.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        SceneManager.LoadScene("EndGame"); // Load the EndGame scene
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log($"Player healed for {amount}. Current health: {currentHealth}");
    }

   

  
}
