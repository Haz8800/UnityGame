using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    private EnemyController enemyController;

    void Start()
    {
        currentHealth = maxHealth;
        enemyController = GetComponent<EnemyController>();
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0f) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(0, currentHealth);

        if (currentHealth <= 0f)
        {
            if (enemyController != null)
                enemyController.Die();
        }
        else
        {
            if (enemyController != null && enemyController.currentState == EnemyState.Patrol)
                enemyController.currentState = EnemyState.Chase;
        }
    }
}
