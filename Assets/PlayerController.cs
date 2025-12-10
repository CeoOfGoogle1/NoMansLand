using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float sprintSpeed = 8.0f;
    public float crouchSpeed = 2.5f;
    public float proneSpeed = 1.5f;
    public float jumpForce = 7.0f;
    public float sensitivity = 2.0f;

    public float airControl = 0.2f; // <--- NEW: air control multiplier

    private CharacterController characterController;

    public Transform playerCamera;
    public Transform gun;

    public Vector3 gunPosition = new Vector3(0.25f, -0.35f, 0.5f);
    public Vector3 aimedGunPosition = new Vector3(0.0f, -0.15f, 0.5f);
    public float aimSpeed = 5.0f;

    private float xRotation = 0f;

    bool isAiming = false;
    bool isCrouching = false;
    bool isProne = false;

    private Vector3 velocity;       // <--- CHANGED: now stores x,y,z
    private bool isGrounded;

    public float gravity = -9.81f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gun.position = gunPosition;
    }

    void Update()
    {
        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Movement input (NO smoothing)
        float moveHorizontal = Input.GetAxisRaw("Horizontal"); // <--- FIXED
        float moveVertical   = Input.GetAxisRaw("Vertical");   // <--- FIXED

        Vector3 move = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;

        isGrounded = characterController.isGrounded;

        // Reset vertical velocity when grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;

            // <<< NEW: stop horizontal momentum on ground >>>
            velocity.x = 0f;
            velocity.z = 0f;
        }

        // Jumping with momentum
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isProne)
            {
                isProne = false;
                isCrouching = true;
                speed = crouchSpeed;
                playerCamera.localPosition = new Vector3(0f, 0.4f, 0f);
            }
            else if (isCrouching)
            {
                isCrouching = false;
                speed = 5.0f;
                playerCamera.localPosition = new Vector3(0f, 0.7f, 0f);
            }
            else if (isGrounded)
            {
                // <<< NEW: preserve horizontal momentum >>>
                Vector3 horizontalVel = move * speed;

                velocity.x = horizontalVel.x;
                velocity.z = horizontalVel.z;

                velocity.y = jumpForce;
            }
        }

        // Crouch cycling
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!isCrouching && !isProne)
            {
                isCrouching = true;
                speed = crouchSpeed;
                playerCamera.localPosition = new Vector3(0f, 0.4f, 0f);
            }
            else if (isCrouching && !isProne)
            {
                isCrouching = false;
                isProne = true;
                speed = proneSpeed;
                playerCamera.localPosition = new Vector3(0f, 0.2f, 0f);
            }
        }

        // Sprint
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching && !isProne)
        {
            speed = sprintSpeed;
        }
        else if (!isCrouching && !isProne)
        {
            speed = 5.0f;
        }

        // Aiming
        if (Input.GetMouseButtonDown(1))
        {
            isAiming = !isAiming;
        }

        if (isAiming)
        {
            gun.localPosition = Vector3.Lerp(gun.localPosition, aimedGunPosition, Time.deltaTime * aimSpeed);
        }
        else
        {
            gun.localPosition = Vector3.Lerp(gun.localPosition, gunPosition, Time.deltaTime * aimSpeed);
        }

        // --- Movement on ground or air ---

        if (isGrounded)
        {
            // full movement
            characterController.Move(move * speed * Time.deltaTime);
        }
        else
        {
            // limited air control
            characterController.Move(move * speed * airControl * Time.deltaTime);
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;

        // Apply momentum (vertical + horizontal from jump)
        characterController.Move(velocity * Time.deltaTime);
    }
}
