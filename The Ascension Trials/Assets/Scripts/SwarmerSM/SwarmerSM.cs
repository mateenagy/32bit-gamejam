using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SwarmerSM : MonoBehaviour
{
    SwarmerState currentState;
    SwarmerFactory factory;
    private NavMeshAgent agent;
    private GameObject player;
    [Header("Swarmer Stats")]
    public int damage = 5;
    public float jumpSpeed = .2f;
    public float extraRotationSpeed = 10f;
    [SerializeField] private Animator animator;


    #region GETTERS / SETTERS
    public SwarmerState CurrentState { get => currentState; set => currentState = value; }
    public SwarmerFactory Factory { get => factory; set => factory = value; }
    public NavMeshAgent Agent { get => agent; set => agent = value; }
    public GameObject Player { get => player; set => player = value; }
    public int Damage { get => damage; }
    public float JumpSpeed { get => jumpSpeed; }
    public Animator SwarmerAnimator { get => animator; }
    public Transform SwarmerTransform { get => transform; }
    #endregion

    void Awake()
    {
        Factory = new SwarmerFactory(this);
        CurrentState = factory.States[SwarmerStates.Idle];
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        Agent.avoidancePriority = Random.Range(10, 65);
        float RadiusValue = Random.Range(.5f, .8f);
        Agent.radius = Mathf.Round(RadiusValue * 10) * .1f;
        CurrentState.EnterStates();
    }

    void Update()
    {
        agent.updateRotation = false;
        Vector3 lookrotation = agent.steeringTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), extraRotationSpeed * Time.deltaTime);
        CurrentState.UpdateStates();
    }

    void OnTriggerEnter(Collider other)
    {
        CurrentState.OnTriggerEnter(other);
    }
}
