using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerSM : MonoBehaviour
{
    PlayerState currentState;
    PlayerFactory factory;
    CharacterController controller;
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
    public float dashSpeed = 10f;
    public float dashTime = 0.5f;
    Vector3 velocity;
    [Header("Player Options")]
    public float mouseSensitivity = 2f;
    public float controllerSensitivity = 2f;
    public bool isController = false;
    public GameObject hand;
    public int life = 100;

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
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        CurrentState.EnterStates();
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
        if (dash.action.triggered)
        {
            StartCoroutine(Dash());
        }
        CurrentState.UpdateStates();

        if (life <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    IEnumerator Dash()
    {
        Vector2 dashInput = move.action.ReadValue<Vector2>();
        Vector3 dashDirection = new Vector3(dashInput.x, 0, dashInput.y);
        dashDirection = transform.TransformDirection(dashDirection).normalized; // Transform to local space
        float startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            if (dashDirection.magnitude > 0f)
            {
                playerCamera.DOFieldOfView(160, 0.3f).SetEase(Ease.InOutSine);
                handCamera.DOFieldOfView(148, 0.3f).SetEase(Ease.InOutSine);
            }
            controller.Move(dashSpeed * Time.deltaTime * dashDirection);
            yield return null;
        }
        playerCamera.DOFieldOfView(82, 0.3f).SetEase(Ease.InOutSine);
        handCamera.DOFieldOfView(70, 0.3f).SetEase(Ease.InOutSine);
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
