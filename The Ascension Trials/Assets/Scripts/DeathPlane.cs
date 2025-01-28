using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPlane : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has fallen off the map");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
