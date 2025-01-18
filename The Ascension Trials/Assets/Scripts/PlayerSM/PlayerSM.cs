using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public Camera playerCamera;
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

    #region GETTERS / SETTERS
    public PlayerState CurrentState { get => currentState; set => currentState = value; }
    public PlayerFactory Factory { get => factory; set => factory = value; }
    #endregion

    void Awake()
    {
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
        CurrentState.UpdateStates();

    }

    void FixedUpdate()
    {
        CurrentState.FixedUpdateStates();
    }
}
