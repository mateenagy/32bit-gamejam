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
    float xRotation = 0f;
    float yRotation = 0f;
    [Header("Input Actions")]
    public InputActionReference move;
    public InputActionReference cameraAction;
    public InputActionReference jump;
    public InputActionReference dash;
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
    Vector3 velocity;
    [Header("Player Options")]
    public float mouseSensitivity = 2f;
    public float controllerSensitivity = 2f;
    public bool isController = false;
    public int life = 100;

    [Header("Hand Animation")]
    public GameObject hand;
    public float handMovementSpeed = 10;
    public float handMovementElevation = 0.001f;

    [Header("Dash Options")]
    public float dashSpeed = 10f;
    public float dashTime = 0.5f;
    public float dashCooldown = 1f;
    private float nextCoolDownTime = 0f;
    public AudioSource dashSoundSource;
    public AudioClip dashSoundClip;

    VisualElement root;

    #region GETTERS / SETTERS
    public PlayerState CurrentState { get => currentState; set => currentState = value; }
    public Vector3 Velocity { get => velocity; set => velocity = value; }
    public float Gravity { get => gravity; }
    public Camera PlayerCamera { get => playerCamera; }
    public bool IsGrounded { get => isGrounded; }
    public PlayerFactory Factory { get => factory; set => factory = value; }
    #endregion

    void Awake()
    {
        InputSystem.settings.maxEventBytesPerUpdate = 0;
        Factory = new PlayerFactory(this);
        CurrentState = factory.States[PlayerStates.Idle];
        controller = GetComponent<CharacterController>();
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        CurrentState.EnterStates();
        root = skillUI.rootVisualElement;
    }

    void Update()
    {
        if (isController)
        {
            mouseSensitivity = controllerSensitivity;
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


        Vector2 moveDirection = move.action.ReadValue<Vector2>();
        Vector2 cameraDirection = cameraAction.action.ReadValue<Vector2>();

        float mouseX = cameraDirection.x * mouseSensitivity * Time.deltaTime;
        float mouseY = cameraDirection.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        yRotation += mouseX;
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        Vector3 finalMove = transform.right * moveDirection.x + transform.forward * moveDirection.y;
        transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        controller.Move(finalMove * speed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;

        if (jump.action.triggered && coyoteCounter > 0)
        {
            velocity.y = Mathf.Sqrt(3f * -2f * gravity);
            coyoteCounter = 0;
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
        CurrentState.UpdateStates();

        hand.transform.position = handMovementElevation * Mathf.Cos(Time.time * handMovementSpeed) * Vector3.up + hand.transform.position;

        if (life <= 0)
        {
            SkillManager.Instance = null;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
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

    void TakeDamage(int damage)
    {
        life -= damage;
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }
}
