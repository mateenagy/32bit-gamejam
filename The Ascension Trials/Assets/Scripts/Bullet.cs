using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            other.gameObject.GetComponent<PlayerSM>().TakeDamage(damage);
        }
    }
}
