using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Speeds")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float crouchSpeed = 2.5f;
    public float proneSpeed = 1.5f;
    public float jumpForce = 7f;

    [Header("Controls")]
    public float sensitivity = 2f;
    public float airControl = 0.2f;

    [Header("Camera")]
    public Transform playerCamera;
    public float standHeight = 0.7f;
    public float crouchHeight = 0.4f;
    public float proneHeight = 0.2f;
    public float cameraTransitionSpeed = 6f;

    [Header("Gun")]
    public Transform gun;
    public Vector3 gunPosition = new Vector3(0.25f, -0.35f, 0.5f);
    public Vector3 aimedGunPosition = new Vector3(0f, -0.15f, 0.5f);
    public float aimSpeed = 5f;

    [Header("Physics")]
    public float gravity = -9.81f;

    [Header("Jump Slowdown")]
    public float slowdownAmount = 0.5f;
    public float slowdownDuration = 0.2f;

    private CharacterController controller;

    private float xRotation;
    private float currentSpeed;

    private bool isCrouching;
    private bool isProne;
    private bool isAiming;
    private bool isGrounded;
    private bool isPreJumping;
    private bool slowdownActive;

    private Vector3 velocity;

    // ------------------------------------------------------------

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentSpeed = walkSpeed;
        gun.localPosition = gunPosition;

        playerCamera.localPosition = new Vector3(0f, standHeight, 0f);
    }

    // ------------------------------------------------------------
    void Update()
    {
        HandleMouseLook();
        HandlePostureInput();
        HandleAimInput();
        HandleMovementInput();
        ApplyGravity();
    }

    // ------------------------------------------------------------
    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    // ------------------------------------------------------------
    void HandleMovementInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 input = (transform.right * h + transform.forward * v).normalized;

        UpdateCurrentSpeed();

        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
            velocity.x = velocity.z = 0f;

            TriggerLandingSlowdown();
        }

        if (Input.GetKeyDown(KeyCode.Space))
            HandleJump(input);

        float appliedSpeed = isGrounded ? currentSpeed : currentSpeed * airControl;

        controller.Move(input * appliedSpeed * Time.deltaTime);
    }

    // ------------------------------------------------------------
    void UpdateCurrentSpeed()
    {
        if (slowdownActive) return;

        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching && !isProne)
            currentSpeed = sprintSpeed;
        else if (isCrouching)
            currentSpeed = crouchSpeed;
        else if (isProne)
            currentSpeed = proneSpeed;
        else
            currentSpeed = walkSpeed;
    }

    // ------------------------------------------------------------
    void HandleJump(Vector3 input)
    {
        if (isProne)
        {
            SetCrouch();
            return;
        }

        if (isCrouching)
        {
            SetStand();
            return;
        }

        if (isGrounded && !isPreJumping)
            StartCoroutine(PreJumpRoutine(input));
    }

    IEnumerator PreJumpRoutine(Vector3 move)
    {
        isPreJumping = true;

        slowdownActive = true;
        currentSpeed = Mathf.Max(0f, currentSpeed - slowdownAmount);

        yield return new WaitForSeconds(slowdownDuration);

        Vector3 horizontal = move * currentSpeed * 0.5f;
        velocity.x = horizontal.x;
        velocity.z = horizontal.z;
        velocity.y = jumpForce;

        slowdownActive = false;
        isPreJumping = false;
    }

    // ------------------------------------------------------------
    void TriggerLandingSlowdown()
    {
        if (!slowdownActive)
        {
            float reduced = Mathf.Max(0f, currentSpeed - slowdownAmount);
            StartCoroutine(LandingSlowdownRoutine(reduced));
        }
    }

    IEnumerator LandingSlowdownRoutine(float reducedSpeed)
    {
        slowdownActive = true;
        currentSpeed = reducedSpeed;

        yield return new WaitForSeconds(slowdownDuration);

        slowdownActive = false;
    }

    // ------------------------------------------------------------
    void HandlePostureInput()
    {
        if (!Input.GetKeyDown(KeyCode.C)) return;

        if (!isCrouching && !isProne)
            SetCrouch();
        else if (isCrouching)
            SetProne();
    }

    void SetStand()
    {
        isCrouching = false;
        isProne = false;
        StartCoroutine(SmoothCameraHeight(standHeight));
    }

    void SetCrouch()
    {
        isCrouching = true;
        isProne = false;
        StartCoroutine(SmoothCameraHeight(crouchHeight));
    }

    void SetProne()
    {
        isCrouching = false;
        isProne = true;
        StartCoroutine(SmoothCameraHeight(proneHeight));
    }

    // ------------------------------------------------------------
    void HandleAimInput()
    {
        if (Input.GetMouseButtonDown(1))
            isAiming = !isAiming;

        gun.localPosition = Vector3.Lerp(
            gun.localPosition,
            isAiming ? aimedGunPosition : gunPosition,
            Time.deltaTime * aimSpeed
        );
    }

    // ------------------------------------------------------------
    IEnumerator SmoothCameraHeight(float targetY)
    {
        Vector3 start = playerCamera.localPosition;
        Vector3 target = new Vector3(start.x, targetY, start.z);

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * cameraTransitionSpeed;
            playerCamera.localPosition = Vector3.Lerp(start, target, t);
            yield return null;
        }

        playerCamera.localPosition = target;
    }

    // ------------------------------------------------------------
    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
