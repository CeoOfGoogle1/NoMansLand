using UnityEngine;

public class PostureController : MonoBehaviour
{
    public float crouchSpeed = 2.5f;
    public float proneSpeed = 1.5f;

    public bool isCrouching;
    public bool isProne;

    [Header("Temporary")]
    public CameraController cameraController;
    private float standHeight => cameraController.standHeight;
    private float crouchHeight => cameraController.crouchHeight;
    private float proneHeight => cameraController.proneHeight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandlePostureInput();
    }

    void HandlePostureInput()
    {
        if (!Input.GetKeyDown(KeyCode.C)) return;

        if (!isCrouching && !isProne)
            SetCrouch();
        else if (isCrouching)
            SetProne();
    }

    public void SetStand()
    {
        isCrouching = false;
        isProne = false;
        StartCoroutine(cameraController.SmoothCameraHeight(standHeight));
    }

    public void SetCrouch()
    {
        isCrouching = true;
        isProne = false;
        StartCoroutine(cameraController.SmoothCameraHeight(crouchHeight));
    }

    public void SetProne()
    {
        isCrouching = false;
        isProne = true;
        StartCoroutine(cameraController.SmoothCameraHeight(proneHeight));
    }
}
