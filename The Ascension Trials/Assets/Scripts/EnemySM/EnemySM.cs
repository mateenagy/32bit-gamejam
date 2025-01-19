using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySM : MonoBehaviour
{
    EnemyState currentState;
    EnemyFactory factory;
    NavMeshAgent agent;
    public GameObject player;
    public GameObject bulletPrefab; // Bullet prefab
    public Transform firePoint; // Point from where the bullet will be fired
    public float fireRate = 1f; // Fire rate in seconds
    private float nextFireTime = 0f; // Time when the next bullet can be fired
    public int life = 20;
    public float extraRotationSpeed = 5f;

    #region GETTERS / SETTERS
    public EnemyState CurrentState { get => currentState; set => currentState = value; }
    public EnemyFactory Factory { get => factory; set => factory = value; }
    #endregion

    void Awake()
    {
        Factory = new EnemyFactory(this);
        CurrentState = factory.States[EnemyStates.Basic];
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        CurrentState.EnterStates();
    }

    void Update()
    {
        CurrentState.UpdateStates();
        agent.SetDestination(player.transform.position);
        Vector3 lookrotation = agent.steeringTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), extraRotationSpeed * Time.deltaTime);

        if (Time.time >= nextFireTime)
        {
            StartCoroutine(FireBullet());
            nextFireTime = Time.time + fireRate;
        }
    }

    IEnumerator FireBullet()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        yield return null;
    }

    public void TakeDamage(int damage)
    {
        life -= damage;
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }
}
