using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Movement Speeds")]
    public float walkSpeed = 5f;
    public float runSpeedMultiplier = 2f;
    public float crouchSpeedMultiplier = 0.5f;
    public float proneSpeedMultiplier = 0.2f;
    public bool isRunning = false;

    [Header("Jump")]
    public float jumpForce = 5f;

    private float currentSpeed;
    private float initialWalkSpeed;
    private float gravity = -9.81f;
    private bool isGrounded = true;
    private Vector3 velocity;
    private PostureController postureController;
    private CharacterController controller;
    private PlayerSpeed playerSpeed;

    void Awake()
    {
        postureController = GetComponent<PostureController>();
        controller = GetComponent<CharacterController>();
        playerSpeed = GetComponent<PlayerSpeed>();
        
        initialWalkSpeed = walkSpeed; 
    }

    void Update()
    {
        walkSpeed = initialWalkSpeed * playerSpeed.GetSpeedFactor();
        Debug.Log("Current Walk Speed: " + walkSpeed);

        isGrounded = controller.isGrounded;

        HandleMovementAndGravity();
        UpdateCurrentSpeed();
        HandleJump();
    }

    void HandleMovementAndGravity()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 input = (transform.right * h + transform.forward * v).normalized;

        float control = isGrounded ? 1f : 0.1f;

        Vector3 move = input * currentSpeed * control;

        // Ground stick
        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f; // small downward force is REQUIRED
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;

        // Combine horizontal + vertical
        Vector3 finalMove = (move + Vector3.up * velocity.y) * Time.deltaTime;

        controller.Move(finalMove);
    }

    void UpdateCurrentSpeed()
    {
        currentSpeed = walkSpeed;

        if (postureController.isProne)
        {
            currentSpeed *= proneSpeedMultiplier;
        }
        else if (postureController.isCrouching)
        {
            currentSpeed *= crouchSpeedMultiplier;
        }

        if (Input.GetKey(KeyCode.LeftShift) && playerSpeed.canRun)
        {
            currentSpeed *= runSpeedMultiplier;
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) &&
            isGrounded &&
            !postureController.isCrouching &&
            !postureController.isProne &&
            playerSpeed.canRun)
        {
            velocity.x *= 0.5f;
            velocity.z *= 0.5f;
            velocity.y = jumpForce;
            isGrounded = false;
        }
    }
}
