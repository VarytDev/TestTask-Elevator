using UnityEngine;

public class ElevatorDoorAnimationState : StateMachineBehaviour
{
    [SerializeField] private bool isOpening = false;

    private ElevatorDoor masterScript = null;

    public override void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        if (masterScript == null && _animator.TryGetComponent(out masterScript) == false)
        {
            return;
        }

        masterScript.SetIsTransitioningState(true);
    }

    public override void OnStateExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        if (masterScript == null && _animator.TryGetComponent(out masterScript) == false)
        {
            return;
        }

        masterScript.SetIsTransitioningState(false);
        masterScript.SetIsOpenState(isOpening);

    }
}
