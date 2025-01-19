using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    PlayerSM player;
    Vector3 direction;
    public float bulletSpeed = 10f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = FindFirstObjectByType<PlayerSM>();
        rb.isKinematic = true;
        direction = (player.transform.position - transform.position).normalized;
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += bulletSpeed * Time.deltaTime * direction;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            Debug.Log("Player Hit");
            other.gameObject.GetComponent<PlayerSM>().life -= 10;
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
