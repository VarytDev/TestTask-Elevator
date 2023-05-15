using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ElevatorDoorHandler : MonoBehaviour
{
    public EElevatorDoorState CurrentDoorState { get; private set; } = EElevatorDoorState.Closed;
    public bool CanOpenDoors { get; set; } = true;

    [Header("References")]
    [SerializeField] private ElevatorDoorAniationHandler animationHandler = null;

    [Header("DoorSettings")]
    [SerializeField] private float timeToAutoClose = 4f;

    private Tween autoCloseTween = null;
    private bool allowAutoClose = true;

    #region Unity Callbacks

    private void OnEnable()
    {
        animationHandler.OnDoorStateChanged += onElevatorStateChanged;
    }

    private void OnDisable()
    {
        animationHandler.OnDoorStateChanged -= onElevatorStateChanged;
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag(PlayerMovement.PLAYER_TAG) == false)
        {
            return;
        }

        stopAutoClose();
        allowAutoClose = false;

        if (CurrentDoorState == EElevatorDoorState.Closing)
        {
            OpenDoor();
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        if (_other.CompareTag(PlayerMovement.PLAYER_TAG) == false)
        {
            return;
        }

        allowAutoClose = true;
        closeDoorAfterTime();
    }
    #endregion

    public void OpenDoor()
    {
        if (isAnyRequiredComponentNull() == true || CanOpenDoors == false)
        {
            return;
        }

        if (CurrentDoorState == EElevatorDoorState.Open || CurrentDoorState == EElevatorDoorState.Opening)
        {
            return;
        }

        animationHandler.OpenDoor();
    }

    public void CloseDoor()
    {
        if (isAnyRequiredComponentNull() == true)
        {
            return;
        }

        if (CurrentDoorState == EElevatorDoorState.Closed || CurrentDoorState == EElevatorDoorState.Closing)
        {
            return;
        }

        animationHandler.CloseDoor();
    }

    private void closeDoorAfterTime()
    {
        stopAutoClose();

        Debug.Log(allowAutoClose);

        if (allowAutoClose == false)
        {
            return;
        }

        autoCloseTween = DOVirtual.DelayedCall(timeToAutoClose, () => CloseDoor());
    }

    private void stopAutoClose()
    {
        autoCloseTween?.Kill();
    }

    private bool isAnyRequiredComponentNull()
    {
        if (animationHandler == null && TryGetComponent(out animationHandler) == false)
        {
            Debug.LogError("ElevatorDoorHandler :: One of required components is null!", this);
            return true;
        }

        return false;
    }

    private void onElevatorStateChanged(EElevatorDoorState _stateToSet)
    {
        CurrentDoorState = _stateToSet;

        if (CurrentDoorState == EElevatorDoorState.Open)
        {
            closeDoorAfterTime();
        }
    }
}
