using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElevatorDoorAniationHandler : MonoBehaviour
{
    public delegate void ElevatorDoorAniationHandlerDelegate(EElevatorDoorState _newState);
    public event ElevatorDoorAniationHandlerDelegate OnDoorStateChanged;

    public float NormalizedOpenLevel { get; set; } = 0f;

    [Header("References")]
    [SerializeField] private Animator animator = null;

    [Header("Door Movement Settings")]
    [SerializeField] private float doorMovementDuration = 3f;

    private float currentOpenLevel = 0f;
    private Tween currentDoorTween = null;

    public void OpenDoor()
    {
        moveOpenLevelTowards(1, EElevatorDoorState.Opening, EElevatorDoorState.Open);
    }

    public void CloseDoor()
    {
        moveOpenLevelTowards(0, EElevatorDoorState.Closing, EElevatorDoorState.Closed);
    }

    private void moveOpenLevelTowards(float _endValue, EElevatorDoorState _newState, EElevatorDoorState _onCompleateState) //calculate door time by normalized value distance
    {
        if (isAnyRequiredComponentNull())
        {
            return;
        }

        OnDoorStateChanged?.Invoke(_newState);
        currentDoorTween?.Kill();

        float _movementTime = getDoorMovementTime(_endValue);

        currentDoorTween = DOTween.To(()=> currentOpenLevel, _x => currentOpenLevel = _x, _endValue, _movementTime)
            .OnUpdate(onDoorTweenUpdate)
            .SetEase(Ease.InOutSine)
            .OnComplete(()=> onDoorTweenCompleate(_onCompleateState));
    }

    private void onDoorTweenUpdate()
    {
        animator.SetFloat("OpenLevel", currentOpenLevel); //TODO Cache parameter
    }

    private void onDoorTweenCompleate(EElevatorDoorState _onCompleateState)
    {
        OnDoorStateChanged?.Invoke(_onCompleateState);
    }

    private float getDoorMovementTime(float _endValue)
    {
        return doorMovementDuration * Mathf.Abs(currentOpenLevel - _endValue);
    }

    private bool isAnyRequiredComponentNull()
    {
        if (animator == null && TryGetComponent(out animator) == false)
        {
            Debug.LogError("ElevatorDoorAniationHandler :: One of required components is null!", this);
            return true;
        }

        return false;
    }
}
