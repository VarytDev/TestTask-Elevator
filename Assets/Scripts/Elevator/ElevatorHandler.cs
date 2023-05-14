using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ElevatorDoorHandler elevatorDoor = null;

    [Header("ElevatorSettings")]
    [SerializeField] private int startingFloor = 0;
    [SerializeField] private float elevatorSpeed = 1f;
    [SerializeField] private Transform[] floorTransforms = new Transform[0];

    private int currentFloor = 0;
    private Tween movementTween = null;
    private Queue<int> movementQueue = new Queue<int>();
    private bool isMoving = false;

    private void Start()
    {
        initializeElevator();
    }

    private void Update()
    {
        if (elevatorDoor.CurrentDoorState != EElevatorDoorState.Closed || isMoving == true || movementQueue.Count <= 0)
        {
            return;
        }

        int _targetFloor = movementQueue.Dequeue();
        isMoving = true;

        movementTween = transform.DOMoveY(getFloorTransformInRange(_targetFloor).position.y, getElevatorMovementTime(_targetFloor))
            .OnUpdate(onElevatorMovementUpdate)
            .SetEase(Ease.InOutSine)
            .OnComplete(()=> onElevatorArrived(_targetFloor));
    }

    public void MoveElevator(int _targetFloor)
    {
        movementQueue.Enqueue(_targetFloor);
    }

    private void onElevatorMovementUpdate()
    {
        
    }

    private void onElevatorArrived(int _targetFloor)
    {
        isMoving = false;
        elevatorDoor.OpenDoor();
        currentFloor = _targetFloor;
    }

    private void initializeElevator()
    {
        transform.position = getFloorTransformInRange(startingFloor).position;
    }

    private int getCurrentFloor(int _movementDirection)
    {
        Transform _floorTransformToCheck = getFloorTransformInRange(currentFloor + _movementDirection);
        //if(transform.y)
        return 1;
    }

    private float getElevatorMovementTime(int _targetFloor)
    {
        if (_targetFloor == currentFloor)
        {
            return 0f;
        }

        Transform _target = getFloorTransformInRange(_targetFloor);
        Transform _current = getFloorTransformInRange(currentFloor);

        if (_target == null || _current == null)
        {
            return 0f;
        }

        return (_target.position - _current.position).magnitude / elevatorSpeed;
    }

    private Transform getFloorTransformInRange(int _floor)
    {
        if (floorTransforms.Length <= 0)
        {
            return null;
        }

        return floorTransforms[Mathf.Clamp(_floor, 0, floorTransforms.Length - 1)];
    }
}
