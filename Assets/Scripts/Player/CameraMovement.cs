using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera = null;

    [Header("Camera Settings")]
    [SerializeField] private float minVerticalAngle = -70;
    [SerializeField] private float maxVerticalAngle = 70;

    [SerializeField] private float mouseSensitivityX = 5;
    [SerializeField] private float mouseSensitivityY = 5;

    private float currentVecticalAngle;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    void Update()
    {
        if (isAnyRequiredComponentNull() == true)
        {
            return;
        }

        transform.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse X") * mouseSensitivityX, Vector3.up); //TODO Convert to new input system

        currentVecticalAngle -= Input.GetAxis("Mouse Y") * mouseSensitivityY;
        currentVecticalAngle = Mathf.Clamp(currentVecticalAngle, minVerticalAngle, maxVerticalAngle);
        playerCamera.transform.localEulerAngles = new Vector3(currentVecticalAngle, playerCamera.transform.localEulerAngles.y, 0);
    }

    private bool isAnyRequiredComponentNull()
    {
        if (playerCamera == null)
        {
            Debug.LogError("CameraMovement :: One of required components is null!", this);
            return true;
        }

        return false;
    }
}
