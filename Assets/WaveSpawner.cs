using UnityEngine;
using System.Collections;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public int zombieCount;
        public float spawnRate; // zombies per second
    }

    public Wave[] waves;
    public Transform[] spawnPoints;
    public GameObject zombiePrefab;
    public Transform zombieContainer;

    private int currentWave = 0;
    private bool isSpawning = false;

    private TextMeshProUGUI waveText;
    private TextMeshProUGUI enemyCountText;

    void Start()
    {
        Debug.Log("[WaveSpawner] Initialized. Waves: " + waves.Length);

        // Find the wave and enemy count text objects
        GameObject waveObject = GameObject.Find("Canvas/HUD/ScreenSpace/TopLeft/HUD_EventLog/HUD_EventLog_Item/Label_Event");
        if (waveObject != null)
        {
            waveText = waveObject.GetComponent<TextMeshProUGUI>();
        }

        GameObject enemyCountObject = GameObject.Find("Canvas/HUD/ScreenSpace/TopLeft/HUD_EventLog/HUD_EventLog_Item2/Label_Event");
        if (enemyCountObject != null)
        {
            enemyCountText = enemyCountObject.GetComponent<TextMeshProUGUI>();
        }

        UpdateWaveText();
        UpdateEnemyCountText();
    }

    void Update()
    {
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log("[WaveSpawner] Enemy count: " + enemyCount + " | Current Wave: " + (currentWave + 1) + " | IsSpawning: " + isSpawning);

        UpdateEnemyCountText();

        if (!isSpawning && enemyCount == 0)
        {
            if (currentWave < waves.Length)
            {
                Debug.Log("[WaveSpawner] Starting Wave " + (currentWave + 1));
                StartCoroutine(SpawnWave(waves[currentWave]));
                UpdateWaveText(); // Update wave text before incrementing
                currentWave++;
            }
            else
            {
                Debug.Log("[WaveSpawner] All waves completed!");
            }
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        isSpawning = true;
        Debug.Log("[WaveSpawner] Spawning " + wave.zombieCount + " zombies at " + wave.spawnRate + " per second");

        for (int i = 0; i < wave.zombieCount; i++)
        {
            SpawnZombie(i + 1);
            yield return new WaitForSeconds(1f / wave.spawnRate);
        }

        isSpawning = false;
        Debug.Log("[WaveSpawner] Finished spawning wave " + currentWave);
    }

    void SpawnZombie(int index)
    {
        if (zombiePrefab == null)
        {
            Debug.LogError("[WaveSpawner] Missing zombie prefab!");
            return;
        }

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("[WaveSpawner] No spawn points assigned!");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject zombie = Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("[WaveSpawner] Spawned Zombie " + index + " at " + spawnPoint.name);

        if (zombieContainer != null)
        {
            zombie.transform.parent = zombieContainer;
        }
    }

    void UpdateWaveText()
    {
        if (waveText != null)
        {
            waveText.text = $"Wave: {currentWave + 1}";
        }
    }

    void UpdateEnemyCountText()
    {
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (enemyCountText != null)
        {
            enemyCountText.text = $"Enemies Remaining: {enemyCount}";
        }
    }
}
