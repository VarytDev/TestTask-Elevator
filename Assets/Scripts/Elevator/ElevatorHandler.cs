using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ElevatorDoorHandler elevatorDoor = null;
    [SerializeField] private ElevatorAudioHandler audioHandler = null;

    [Header("ElevatorSettings")]
    [SerializeField] private int startingFloor = 0;
    [SerializeField] private float elevatorSpeed = 1f;
    [SerializeField] private Transform[] floorTransforms = new Transform[0];

    private int currentFloor = 0;
    private Tween movementTween = null;
    private Queue<int> movementQueue = new Queue<int>();

    private Transform playerTransformToUpdate = null;
    private float playerHeightOffset = 0f;

    private void Start()
    {
        initializeElevator();
    }

    private void Update()
    {
        if (isAnyRequiredComponentNull() == true)
        {
            return;
        }

        if (elevatorDoor.CurrentDoorState != EElevatorDoorState.Closed || (movementTween != null && movementTween.active == true) || movementQueue.Count <= 0)
        {
            return;
        }

        startMovingElevator();
    }

    //HACK to stop player from shaking in elevator
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag(PlayerMovement.PLAYER_TAG) == true)
        {
            playerTransformToUpdate = _other.transform;
            playerHeightOffset = playerTransformToUpdate.position.y - transform.position.y;
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        if (_other.CompareTag(PlayerMovement.PLAYER_TAG) == true)
        {
            playerTransformToUpdate = null;
        }
    }

    public void MoveElevator(int _targetFloor)
    {
        if (isAnyRequiredComponentNull() == true)
        {
            return;
        }

        if (movementQueue.Contains(_targetFloor))
        {
            return;
        }

        if (currentFloor == _targetFloor)
        {
            elevatorDoor.OpenDoor();
            return;
        }

        movementQueue.Enqueue(_targetFloor);
    }

    private void startMovingElevator()
    {
        elevatorDoor.CanOpenDoors = false;

        int _targetFloor = movementQueue.Dequeue();

        audioHandler.PlayElevatorMoveSFX();
        audioHandler.PlayMusic();

        movementTween = transform.DOMoveY(getFloorTransformInRange(_targetFloor).position.y, getElevatorMovementTime(_targetFloor))
            .OnUpdate(onElevatorMoveUpdate)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => onElevatorArrived(_targetFloor));
    }

    private void onElevatorMoveUpdate()
    {
        if (playerTransformToUpdate == null)
        {
            return;
        }

        //HACK to stop player from shaking in elevator
        Vector3 _positionToSet = playerTransformToUpdate.position;
        _positionToSet.y = transform.position.y + playerHeightOffset;
        playerTransformToUpdate.position = _positionToSet;
    }

    private void onElevatorArrived(int _targetFloor)
    {
        if (isAnyRequiredComponentNull() == true)
        {
            return;
        }

        elevatorDoor.CanOpenDoors = true;

        audioHandler.StopElevatorMoveSFX();
        audioHandler.StopMusic();
        audioHandler.PlayElevatorDingSFX();

        elevatorDoor.OpenDoor();
        currentFloor = _targetFloor;
    }

    private void initializeElevator()
    {
        if (isAnyRequiredComponentNull() == true)
        {
            return;
        }

        transform.position = getFloorTransformInRange(startingFloor).position;
    }

    private float getElevatorMovementTime(int _targetFloor)
    {
        if (isAnyRequiredComponentNull() == true)
        {
            return 0f;
        }

        if (_targetFloor == currentFloor)
        {
            return 0f;
        }

        Transform _target = getFloorTransformInRange(_targetFloor);
        Transform _current = getFloorTransformInRange(currentFloor);

        return (_target.position - _current.position).magnitude / elevatorSpeed;
    }

    private Transform getFloorTransformInRange(int _floor)
    {
        if (floorTransforms.IsArrayValid() == false)
        {
            return null;
        }

        return floorTransforms[Mathf.Clamp(_floor, 0, floorTransforms.Length - 1)];
    }

    private bool isAnyRequiredComponentNull()
    {
        if (elevatorDoor == null || audioHandler == null || floorTransforms.IsArrayValid() == false)
        {
            Debug.LogError("ElevatorHandler :: One of required components is null!", this);
            return true;
        }

        return false;
    }
}
