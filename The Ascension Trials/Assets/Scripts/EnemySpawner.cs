using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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
    public DialogSystem dialogSystem;
    private List<GameObject> enemies = new();
    private bool started = false;

    void Start()
    {
        StartCoroutine(EnemySpawnTimeout(2f));
    }


    void Update()
    {
        if (!started) return;

        if (currentWave >= wavePool.waves.Count - 1 && WaveManager.Instance.enemiesLeft <= 0)
        {
            if (dialogSystem)
            {
                StartCoroutine(ShowEndUI());
                // var root = dialogSystem.ui.rootVisualElement;
                // VisualElement endUI = root.Q<VisualElement>("end-ui-container");
            }
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
                    StartCoroutine(EnemySpawnTimeout(2f));
                }
            }
        }
    }

    IEnumerator ShowEndUI()
    {
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(2f * 0.2f);
        dialogSystem.dialog.SetActive(true);
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

    IEnumerator EnemySpawnTimeout(float time)
    {
        started = false;
        yield return new WaitForSeconds(time);
        started = true;
        SpawnEnemy();
    }
}
