using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ElevatorDoor elevatorDoor = null;

    [Header("ElevatorSettings")]
    [SerializeField] private int startingFloor = 0;
    [SerializeField] private float elevatorSpeed = 1f;
    [SerializeField] private Transform[] floorTransforms = new Transform[0];

    private int currentFloor = 0;

    private void Start()
    {
        initializeElevator();
    }

    public void MoveElevator(int _targetFloor)
    {
        Sequence _tweenSequence = DOTween.Sequence();

        //make sure that elevator is not moving when starting another dotween, or kill previous? or queue?
        currentFloor = _targetFloor;
    }

    private IEnumerator moveElevatorHandler(int _targetFloor)
    {
        yield return new WaitForSeconds(3f);

        yield return transform.DOMoveY(getFloorTransformInRange(_targetFloor).position.y, getElevatorMovementTime(_targetFloor));
    }

    private void initializeElevator()
    {
        transform.position = getFloorTransformInRange(startingFloor).position;
    }

    private float getElevatorMovementTime(int _targetFloor)
    {
        if (_targetFloor == currentFloor)
        {
            return 0f;
        }

        float _distance = (getFloorTransformInRange(_targetFloor).position - getFloorTransformInRange(currentFloor).position).magnitude;
        return _distance / elevatorSpeed;
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
