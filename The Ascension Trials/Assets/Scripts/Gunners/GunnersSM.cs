using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Enemy))]
public class GunnersSM : MonoBehaviour
{
    GunnersState currentState;
    GunnersFactory factory;
    NavMeshAgent agent;
    PlayerSM player;
    [SerializeField] float radius = 10f;
    [Header("Shooting Settings")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float shootForce = 10f;
    [SerializeField] float shootingTime = 5f;
    [SerializeField] float spread = 1f;
    [SerializeField] float timeToNextShoot = 0.5f;
    [SerializeField] float extraRotationSpeed = 10f;

    Vector3 patrolPoint;

    #region GETTERS / SETTERS
    public GunnersState CurrentState { get => currentState; set => currentState = value; }
    public GunnersFactory Factory { get => factory; set => factory = value; }
    public Vector3 PatrolPoint { get => patrolPoint; set => patrolPoint = value; }
    public PlayerSM Player { get => player; }
    public GameObject Bullet { get => bulletPrefab; }
    public float ShootForce { get => shootForce; }
    public float Spread { get => spread; }
    public NavMeshAgent Agent { get => agent; }
    public float Radius { get => radius; }
    public float ShootingTime { get => shootingTime; }
    public float TimeToNextShoot { get => timeToNextShoot; }
    #endregion

    void Awake()
    {
        Factory = new GunnersFactory(this);
        CurrentState = factory.States[GunnersStates.Ground];
        agent = GetComponent<NavMeshAgent>();
        player = FindFirstObjectByType<PlayerSM>();
    }

    void Start()
    {
        CurrentState.EnterStates();
    }

    void Update()
    {
        agent.updateRotation = false;
        Vector3 lookrotation = agent.steeringTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), extraRotationSpeed * Time.deltaTime);
        CurrentState.UpdateStates();
    }

    public bool CheckPlayerInSight()
    {
        Vector3 direction = player.transform.position - transform.position;
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, radius * 2, layerMask: LayerMask.GetMask("Player", "Ground")))
        {
            if (hit.collider.gameObject.GetComponent<PlayerSM>())
            {
                return true;
            }
        }

        return false;
    }

    void OnDrawGizmos()
    {
        if (player != null)
        {
            if (CheckPlayerInSight())
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Vector3 direction = player.transform.position - transform.position;
            Gizmos.DrawRay(transform.position, 2 * radius * direction.normalized);
        }
    }
}
