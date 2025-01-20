using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    public int enemiesLeft = 0;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
