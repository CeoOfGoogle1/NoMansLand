using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Speeds")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;

    public float jumpForce = 7f;

    [Header("Controls")]
    public float airControl = 0.2f;

    [Header("Jump Slowdown")]
    public float slowdownAmount = 0.9f;
    public float slowdownDuration = 0.9f;
    public float airSpeedReduction = 0.6f;

    private CharacterController controller;
    private float currentSpeed;
    private float gravity = -9.81f;
    private bool isGrounded;
    private bool isPreJumping;
    private bool slowdownActive;
    private Vector3 velocity;

    [Header("Temporary")]
    public PostureController postureController;
    private bool isCrouching => postureController.isCrouching;
    private bool isProne => postureController.isProne;
    private float crouchSpeed => postureController.crouchSpeed;
    private float proneSpeed => postureController.proneSpeed;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        
        HandleMovementInput();
        ApplyGravity();
    }

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

    void HandleJump(Vector3 input)
    {
        if (isProne)
        {
            postureController.SetCrouch();
            return;
        }

        if (isCrouching)
        {
            postureController.SetStand();
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

        Vector3 horizontal = move * currentSpeed * airSpeedReduction;
        velocity.x = horizontal.x;
        velocity.z = horizontal.z;
        velocity.y = jumpForce;

        slowdownActive = false;
        isPreJumping = false;
    }

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

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
