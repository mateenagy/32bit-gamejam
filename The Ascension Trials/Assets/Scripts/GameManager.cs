using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float mouseSensitivity = 100.0f;
    public bool isDialogue = false;

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
