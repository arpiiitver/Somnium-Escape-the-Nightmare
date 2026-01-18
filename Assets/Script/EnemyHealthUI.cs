using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public Slider healthSlider;
    public GfeHealth enemyHealth; // Reference to your EnemyHealth script

    void Update()
    {
        if (enemyHealth != null)
        {
            healthSlider.value = (float)enemyHealth.GetCurrentHealth() / enemyHealth.maxHealth;
        }
    }
}
