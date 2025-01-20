using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public List<GameObject> enemies = new();
}

[System.Serializable]
public class WavePool
{
    public List<Wave> waves = new();
}

public class EnemySpawner : MonoBehaviour
{
    public WavePool wavePool = new();
    public GameObject[] spawnPoints;
    public int currentWave = 0;
    private List<GameObject> enemies = new();

    void Start()
    {
        SpawnEnemy();
    }


    void Update()
    {
        if (currentWave >= wavePool.waves.Count - 1 && WaveManager.Instance.enemiesLeft <= 0)
        {
            Debug.Log("You win!");
        }
        else
        {
            if (WaveManager.Instance.enemiesLeft > 0)
            {
                return;
            }
            else
            {
                if (currentWave < wavePool.waves.Count - 1)
                {
                    currentWave++;
                    SpawnEnemy();
                }
            }
        }
    }

    void SpawnEnemy()
    {
        Wave wave = wavePool.waves[currentWave];
        WaveManager.Instance.enemiesLeft = wave.enemies.Count;

        foreach (GameObject enemy in wave.enemies)
        {
            GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            enemies.Add(Instantiate(enemy, spawnPoint.transform.position, spawnPoint.transform.rotation));
        }
    }
}
