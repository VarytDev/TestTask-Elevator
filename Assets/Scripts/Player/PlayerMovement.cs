using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public const string PLAYER_TAG = "Player";
    public const float GRAVITY_ACCELERATION = 9.81f;

    [Header("References")]
    [SerializeField] private CharacterController characterController = null;

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 3;

    void Update()
    {
        if (isAnyRequiredComponentNull() == true)
        {
            return;
        }

        Vector3 _verticalInput = transform.forward * Input.GetAxis("Vertical") * movementSpeed; //TODO Convert to new input system
        Vector3 _horizontalInput = transform.right * Input.GetAxis("Horizontal") * movementSpeed;
        Vector3 _gravity = characterController.isGrounded == true ? Vector3.zero : Vector3.down * GRAVITY_ACCELERATION;

        characterController.Move((_verticalInput + _horizontalInput + _gravity) * Time.deltaTime);
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
