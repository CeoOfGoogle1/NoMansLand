using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [Header("Camera")]
    public Transform playerCamera;
    public float standHeight = 0.7f;
    public float crouchHeight = 0.4f;
    public float proneHeight = 0.2f;
    public float cameraTransitionSpeed = 6f;
    public float sensitivity = 2f;

    private float xRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerCamera.localPosition = new Vector3(0f, standHeight, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseLook();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    public IEnumerator SmoothCameraHeight(float targetY)
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
}
