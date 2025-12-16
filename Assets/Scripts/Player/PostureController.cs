using UnityEngine;

public class PostureController : MonoBehaviour
{
    public bool isCrouching = false;
    public bool isProne = false;
    private CameraController cameraController;

    void Awake()
    {
        cameraController = GetComponent<CameraController>();
    }

    void Update()
    {
        HandlePostureInput();
    }

    void HandlePostureInput()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!isCrouching && !isProne)
                SetCrouch();
            else if (isCrouching)
                SetProne();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isProne)
                SetCrouch();
            else if (isCrouching)
                SetStand();
        }
    }

    public void SetStand()
    {
        isCrouching = false;
        isProne = false;
        StartCoroutine(cameraController.SmoothCameraHeight(cameraController.standHeight));
    }

    public void SetCrouch()
    {
        isCrouching = true;
        isProne = false;
        StartCoroutine(cameraController.SmoothCameraHeight(cameraController.crouchHeight));
    }

    public void SetProne()
    {
        isCrouching = false;
        isProne = true;
        StartCoroutine(cameraController.SmoothCameraHeight(cameraController.proneHeight));
    }
}
