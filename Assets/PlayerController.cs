//using NUnit.Framework;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float sprintSpeed = 8.0f;
    public float crouchSpeed = 2.5f;
    public float proneSpeed = 1.5f;
    public float jumpForce = 7.0f;
    public float sensitivity = 2.0f;
    private CharacterController characterController;
    public Transform playerCamera; // Assign your camera in the inspector
    public Transform gun;
    public Vector3 gunPosition = new Vector3(0.25f, -0.35f, 0.5f);
    public Vector3 aimedGunPosition = new Vector3(0.0f, -0.15f, 0.5f);
    public float aimSpeed = 5.0f;
    private float xRotation = 0f;
    bool isAiming = false;
    bool isCrouching = false;
    bool isProne = false;

    private Vector3 velocity;
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

        // Posture and jump handling
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isProne)
            {
                // Prone to crouch
                isProne = false;
                isCrouching = true;
                speed = crouchSpeed;
                playerCamera.localPosition = new Vector3(0f, 0.4f, 0f);
            }
            else if (isCrouching)
            {
                // Crouch to standing
                isCrouching = false;
                speed = 5.0f;
                playerCamera.localPosition = new Vector3(0f, 0.7f, 0f);
            }
            else if (isGrounded)
            {
                // Standing to jump
                velocity.y = jumpForce;
            }
        }

        // Posture down cycling with C key
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!isCrouching && !isProne)
            {
                // Standing to crouch
                isCrouching = true;
                speed = crouchSpeed;
                playerCamera.localPosition = new Vector3(0f, 0.4f, 0f);
            }
            else if (isCrouching && !isProne)
            {
                // Crouch to prone
                isCrouching = false;
                isProne = true;
                speed = proneSpeed;
                playerCamera.localPosition = new Vector3(0f, 0.2f, 0f);
            }
        }

        // Sprint logic (only when standing)
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
        
        // Movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 move = Vector3.zero;
        if (Mathf.Abs(moveHorizontal) > 0.01f || Mathf.Abs(moveVertical) > 0.01f)
        {
            move = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;
        }

        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small downward force to keep grounded
        }

        characterController.Move(move * speed * Time.deltaTime);

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(new Vector3(0, velocity.y, 0) * Time.deltaTime);
    }
}
