using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;          // Maximum health value
    private int currentHealth;

    [Header("UI Reference")]
    public Slider healthSlider;          // Drag your UI Slider here

    void Start()
    {
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;  // Set slider range
            healthSlider.value = currentHealth; // Initialize slider
        }
    }

    // Call this method to reduce player's health
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (healthSlider != null)
            healthSlider.value = currentHealth;  // Update slider

        if (currentHealth <= 0)
        {
            Die();                               // Restart level on death
        }
    }

    void Die()
    {
        // Reload the current level when the player dies
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
