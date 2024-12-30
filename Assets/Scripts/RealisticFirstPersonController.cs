using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RealisticFirstPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float sprintSpeed = 6f;
    public float crouchSpeed = 1.5f;
    public float acceleration = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public bool canJump = true;

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float mouseSensitivity = 100f;
    public float crouchHeight = 0.5f;

    [Header("Head Bob Settings")]
    public float headBobFrequency = 1.5f;
    public float headBobAmplitude = 0.1f;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;
    private bool isCrouching = false;
    private float originalHeight;
    private float targetSpeed;
    private float currentSpeed;

    private float headBobTimer = 0f;
    private Vector3 cameraStartPosition;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        originalHeight = controller.height;
        targetSpeed = walkSpeed;
        cameraStartPosition = cameraTransform.localPosition;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
        HandleHeadBob();
    }

    void HandleMovement()
    {
        // Sprinting and Crouching
        if (Input.GetKey(KeyCode.LeftShift))
        {
            targetSpeed = sprintSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            targetSpeed = crouchSpeed;
            if (!isCrouching)
            {
                controller.height = crouchHeight;
                isCrouching = true;
            }
        }
        else
        {
            targetSpeed = walkSpeed;
            if (isCrouching)
            {
                controller.height = originalHeight;
                isCrouching = false;
            }
        }

        // Smooth speed transition
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.deltaTime);

        // Movement input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Jumping and Gravity
        if (controller.isGrounded)
        {
            velocity.y = -2f; // Reset velocity
            if (canJump && Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void HandleHeadBob()
    {
        if (controller.isGrounded && controller.velocity.magnitude > 0.1f)
        {
            headBobTimer += Time.deltaTime * headBobFrequency;
            cameraTransform.localPosition = cameraStartPosition +
                new Vector3(0, Mathf.Sin(headBobTimer) * headBobAmplitude, 0);
        }
        else
        {
            headBobTimer = 0;
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, cameraStartPosition, Time.deltaTime * 10f);
        }
    }
}