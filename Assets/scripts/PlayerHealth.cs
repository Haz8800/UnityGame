using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    private TextMeshProUGUI healthText;
    private TextMeshProUGUI gameOverText;
    private TextMeshProUGUI restartingText;

    private void Start()
    {
        currentHealth = maxHealth;

        // Find the health text object in the correct path
        GameObject healthObject = GameObject.Find("Canvas/HUD/ScreenSpace/MiddleLeft/HUD_HealthBar_Dial_Minimal/Label_Health_Current");
        if (healthObject != null)
        {
            healthText = healthObject.GetComponent<TextMeshProUGUI>();
        }

        // Find the Game Over text object
        GameObject gameOverObject = GameObject.Find("Canvas/Gameover");
        if (gameOverObject != null)
        {
            gameOverText = gameOverObject.GetComponent<TextMeshProUGUI>();
            gameOverText.gameObject.SetActive(false); // Hide it initially
        }

        // Find the Restarting text object
        GameObject restartingObject = GameObject.Find("Canvas/Restarting");
        if (restartingObject != null)
        {
            restartingText = restartingObject.GetComponent<TextMeshProUGUI>();
            restartingText.gameObject.SetActive(false); // Hide it initially
        }

        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = $"{currentHealth}%";
            healthText.color = (currentHealth < 30f) ? Color.red : Color.white;
        }
    }

    private void Die()
    {
        if (healthText != null)
        {
            healthText.text = "0%";
        }

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true); // Show Game Over text
            gameOverText.text = "Game Over!";
        }

        if (restartingText != null)
        {
            restartingText.gameObject.SetActive(true); // Show Restarting text
            restartingText.text = "Restarting...";
        }

        Debug.Log("Player died!");

        // Stop all player movement and interactions
        Time.timeScale = 0f;

        // Restart the game after 3 seconds using a coroutine
        StartCoroutine(RestartGameAfterDelay(3f));
    }

    private IEnumerator RestartGameAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // Wait for the delay in real time
        Time.timeScale = 1f; // Resume the game time before restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }
}