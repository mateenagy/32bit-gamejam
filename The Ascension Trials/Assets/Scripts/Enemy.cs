using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private int health = 100;
    [SerializeField] private GameObject bloodParticle;
    [SerializeField] private GameObject dismemberedSwarmer;
    [SerializeField] private AudioClip hitSoundClip;
    private AudioSource enemyAudioSource;
    void Start()
    {
        enemyAudioSource = GetComponent<AudioSource>();
        enemyAudioSource.clip = hitSoundClip;
    }

    public void TakeDamage(int damage, Vector3 hitPoint)
    {
        StartCoroutine(DamageCoroutine(damage, hitPoint));
    }

    IEnumerator DamageCoroutine(int damage, Vector3 hitPoint)
    {
        enemyAudioSource.pitch = Random.Range(0.8f, 1.2f);
        enemyAudioSource.Play();
        yield return new WaitForSeconds(0.1f);
        health -= damage;
        GameObject blood = Instantiate(bloodParticle, hitPoint, bloodParticle.transform.rotation);
        Destroy(blood, .5f);
        if (health <= 0)
        {
            WaveManager.Instance.enemiesLeft--;
            GameObject dismembered = Instantiate(dismemberedSwarmer, transform.position, transform.rotation);
            Rigidbody[] rbs = dismembered.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in rbs)
            {
                if (rb)
                {
                    rb.AddExplosionForce(3000, hitPoint, 1);
                }
            }
            Destroy(dismembered, 2f);
            Destroy(gameObject);
        }

    }
}
