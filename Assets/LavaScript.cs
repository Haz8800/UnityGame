using UnityEngine;

public class LavaScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(playerHealth.maxHealth); // Instantly kill the player
                Debug.Log("Player touched lava and died!");
            }
        }

        // Check if the object is tagged as "Enemy"
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(enemyHealth.maxHealth); // Instantly kill the enemy
                Debug.Log("Enemy touched lava and died!");
            }
        }
    }
}
