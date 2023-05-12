using System.Collections;
using UnityEngine;

public class ElevatorDoor : MonoBehaviour
{
    public bool IsOpen { get; private set; } = false;
    public bool IsTransitioning { get; private set; } = false;

    [Header("References")]
    [SerializeField] private Animator animator;

    [Header("DoorSettings")]
    [SerializeField] private float timeToAutoClose = 4f;

    private Coroutine autoCloseCoroutine = null;
    private bool closingLocked = false;

    public void SetIsOpenState(bool _newState)
    {
        IsOpen = _newState;
    }

    public void SetIsTransitioningState(bool _newState)
    {
        IsTransitioning = _newState;
    }

    public void OpenDoor()
    {
        stopAutoCloseCoroutine();

        autoCloseCoroutine = StartCoroutine(closeDoorAfterTime());

        if (IsOpen == true || IsTransitioning == true)
        {
            return;
        }

        animator.SetTrigger("OpenDoor"); //TODO Think of a better way to do this (enum instaed of strings)
    }

    public void CloseDoor()
    {
        stopAutoCloseCoroutine();

        if (IsOpen == false || IsTransitioning == true)
        {
            return;
        }

        animator.SetTrigger("CloseDoor");
    }

    public void InterruptClose() //TODO Make this better
    {
        animator.Play("AN_OpenElevatorDoor", 0, 1 - animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

        stopAutoCloseCoroutine();
        autoCloseCoroutine = StartCoroutine(closeDoorAfterTime());

    }

    private void stopAutoCloseCoroutine()
    {
        if (autoCloseCoroutine != null)
        {
            StopCoroutine(autoCloseCoroutine);
            autoCloseCoroutine = null;
        }
    }

    private IEnumerator closeDoorAfterTime()
    {
        yield return Waiters.WaitForSeconds(timeToAutoClose);
        yield return new WaitUntil(() => closingLocked == false);

        CloseDoor();

        autoCloseCoroutine = null;
    }

    private void OnTriggerEnter(Collider _other)
    {
        if(IsOpen == true && IsTransitioning == true)
        {
            InterruptClose();
        }

        closingLocked = true;
    }

    private void OnTriggerExit(Collider _other)
    {
        closingLocked = false;
    }
}
