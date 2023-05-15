using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public const string PLAYER_TAG = "Player";
    public const float GRAVITY_ACCELERATION = 9.81f;

    [Header("References")]
    [SerializeField] private CharacterController characterController = null;

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 3;

    private Vector3 gravityAcceleration = Vector3.down * GRAVITY_ACCELERATION;

    void Update()
    {
        if (isAnyRequiredComponentNull() == true)
        {
            return;
        }

        Vector3 _verticalInput = transform.forward * Input.GetAxis("Vertical") * movementSpeed;
        Vector3 _horizontalInput = transform.right * Input.GetAxis("Horizontal") * movementSpeed;

        characterController.Move((_verticalInput + _horizontalInput + gravityAcceleration) * Time.deltaTime);
    }

    private bool isAnyRequiredComponentNull()
    {
        if (characterController == null && TryGetComponent(out characterController) == false)
        {
            Debug.LogError("PlayerMovement :: One of required components is null!", this);
            return true;
        }

        return false;
    }
}
