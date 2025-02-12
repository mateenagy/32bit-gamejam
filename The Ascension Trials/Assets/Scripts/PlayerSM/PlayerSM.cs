using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(CharacterController))]
public class PlayerSM : MonoBehaviour
{
    PlayerState currentState;
    PlayerFactory factory;
    CharacterController controller;
    public UIDocument skillUI;
    public UIDocument healthUI;
    float xRotation = 0f;
    float yRotation = 0f;
    [Header("Input Actions")]
    public InputActionReference move;
    public InputActionReference cameraAction;
    public InputActionReference jump;
    public InputActionReference dash;
    public InputActionReference heal;
    public Camera playerCamera;
    public Camera handCamera;
    [Header("Player Movement")]
    public float speed = 5f;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded = false;
    public float coyoteTime = 0.2f;
    private float coyoteCounter = 0f;
    public AudioSource runSoundSource;
    public AudioClip runSoundClip;
    Vector3 velocity;
    [Header("Player Options")]
    public int life = 100;

    [Header("Hand Animation")]
    public GameObject hand;
    public float handMovementSpeed = 10;
    public float handMovementElevation = 0.001f;

    [Header("Skills Options")]
    [Header("Dash")]
    public float dashSpeed = 10f;
    public float dashTime = 0.5f;
    public float dashCooldown = 1f;
    private float nextCoolDownTime = 0f;
    public AudioSource dashSoundSource;
    public AudioClip dashSoundClip;
    [Header("Heal")]
    public int healAmount = 40;
    public float healCooldown = 1f;
    private float nextHealCoolDownTime = 0f;

    VisualElement root;
    VisualElement healthUIRoot;

    bool isMoving = false;
    Vector2 moveDirection;

    #region GETTERS / SETTERS
    public PlayerState CurrentState { get => currentState; set => currentState = value; }
    public Vector3 Velocity { get => velocity; set => velocity = value; }
    public float Gravity { get => gravity; }
    public float YRotation { get => yRotation; }
    public Vector2 MoveDirection { get => moveDirection; }
    public float Speed { get => speed; }
    public Transform PlayerTransform { get => transform; }
    public CharacterController Controller { get => controller; }
    public Camera PlayerCamera { get => playerCamera; }
    public bool IsGrounded { get => isGrounded; }
    public bool IsMoving { get => isMoving; }
    public PlayerFactory Factory { get => factory; set => factory = value; }
    #endregion

    void Awake()
    {
        InputSystem.settings.maxEventBytesPerUpdate = 0;
        Factory = new PlayerFactory(this);
        CurrentState = factory.States[PlayerStates.Ground];
        controller = GetComponent<CharacterController>();
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        CurrentState.EnterStates();
        root = skillUI.rootVisualElement;
        healthUIRoot = healthUI.rootVisualElement;
        yRotation = transform.eulerAngles.y;
    }

    void Update()
    {
        if (GameManager.Instance.isDialogue)
        {
            return;
        }
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded)
        {
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }


        moveDirection = move.action.ReadValue<Vector2>();
        Vector2 cameraDirection = cameraAction.action.ReadValue<Vector2>();

        float mouseX = cameraDirection.x * GameManager.Instance.mouseSensitivity * Time.deltaTime;
        float mouseY = cameraDirection.y * GameManager.Instance.mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        yRotation += mouseX;
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);

        /* CHECK IS MOVING */
        if (moveDirection.x != 0 || moveDirection.y != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        velocity.y += gravity * Time.deltaTime;

        if (jump.action.triggered && coyoteCounter > 0)
        {
            velocity.y = Mathf.Sqrt(3f * -2f * gravity);
            coyoteCounter = 0;
        }

        if (isGrounded && (controller.velocity.x != 0 || controller.velocity.z != 0))
        {
            if (!runSoundSource.isPlaying)
            {
                dashSoundSource.pitch = Random.Range(0.8f, 1.2f);
                dashSoundSource.clip = runSoundClip;
                dashSoundSource.Play();
            }
        }

        controller.Move(velocity * Time.deltaTime);

        /* DASH */
        if (SkillManager.Instance && SkillManager.Instance.skills.BinarySearch(Skill.Dash) >= 0)
        {
            if (dash.action.triggered && Time.time >= nextCoolDownTime)
            {
                var dashSkillOverlayUI = root.Q<VisualElement>("dash").Q<VisualElement>("overlay");
                dashSkillOverlayUI.style.scale = new StyleScale(new Vector2(1, 1));
                StartCoroutine(Dash());
                nextCoolDownTime = Time.time + dashCooldown;
            }
        }

        /* HEAL */
        if (SkillManager.Instance && SkillManager.Instance.skills.BinarySearch(Skill.Heal) >= 0)
        {
            if (heal.action.triggered && Time.time >= nextHealCoolDownTime)
            {
                var healSkillOverlayUI = root.Q<VisualElement>("heal").Q<VisualElement>("overlay");
                healSkillOverlayUI.style.scale = new StyleScale(new Vector2(1, 1));
                life += healAmount;
                life = Mathf.Clamp(life, 0, 100);
                UpdateHelathBar();
                float value = 1f;
                nextHealCoolDownTime = Time.time + healCooldown;
                DOTween.To(() => value, x => value = x, 0f, healCooldown).OnUpdate(() =>
                {
                    healSkillOverlayUI.style.scale = new StyleScale(new Vector2(1, value));
                });
            }
        }
        CurrentState.UpdateStates();
    }

    IEnumerator Dash()
    {
        dashSoundSource.clip = dashSoundClip;
        dashSoundSource.Play();
        Vector2 dashInput = move.action.ReadValue<Vector2>();
        Vector3 dashDirection = new Vector3(dashInput.x, 0, dashInput.y);
        dashDirection = transform.TransformDirection(dashDirection).normalized; // Transform to local space
        float startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            if (dashDirection.magnitude > 0f)
            {
                playerCamera.DOFieldOfView(100, 0.3f).SetEase(Ease.InOutSine);
                handCamera.DOFieldOfView(88, 0.3f).SetEase(Ease.InOutSine);
            }
            controller.Move(dashSpeed * Time.deltaTime * dashDirection);
            yield return null;
        }
        playerCamera.DOFieldOfView(82, 0.3f).SetEase(Ease.InOutSine);
        handCamera.DOFieldOfView(70, 0.3f).SetEase(Ease.InOutSine);
        float value = 1f;
        var dashSkillOverlayUI = root.Q<VisualElement>("dash").Q<VisualElement>("overlay");
        DOTween.To(() => value, x => value = x, 0f, dashCooldown).OnUpdate(() =>
        {
            dashSkillOverlayUI.style.scale = new StyleScale(new Vector2(1, value));
        });
    }

    void FixedUpdate()
    {
        CurrentState.FixedUpdateStates();
    }

    void UpdateHelathBar()
    {
        VisualElement healthbar = healthUIRoot.Q<VisualElement>("healthbar_container").Q<VisualElement>("healthbar");
        Length width = new(life, LengthUnit.Percent);
        healthbar.style.width = new StyleLength(width);
    }

    public void TakeDamage(int damage)
    {
        life -= damage;
        UpdateHelathBar();
        if (life <= 0)
        {
            SkillManager.Instance = null;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
