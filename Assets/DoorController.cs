using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public RotatingDoorData data = new RotatingDoorData();
    private bool oppening, queuedAction;

    Quaternion closedRot, openRot;
    public Transform door;
    public AudioSource source;

    private bool canOpen => !oppening && !data.autoOpen && data.isSelected;

    void Start()
    {
        source = GetComponent<AudioSource>();
        door = transform.GetChild(0);
        UpdateAngle();
    }

    void Update()
    {
        if(Input.GetButtonDown("Open Door") && canOpen)
        {
            if(data.isOpen) CloseDoor();
            else OpenDoor();
        }
    }

    public void UpdateAngle()
    {
        int angle = (data.openRight) ? data.angle : -data.angle;

        closedRot = door.rotation;
        openRot = door.rotation * Quaternion.AngleAxis(angle, Vector3.up);
    }

    void OpenDoor()
    {
        if(data.isShut) return;

        source.PlayOneShot(Resources.Load<AudioClip>("Sounds/MC_DoorOpen"));
        data.isOpen = true;

        StartCoroutine(MoveDoor(openRot));
    }

    void CloseDoor()
    {
        if(data.isShut) return;

        source.PlayOneShot(Resources.Load<AudioClip>("Sounds/MC_DoorClose"));
        data.isOpen = false;

        StartCoroutine(MoveDoor(closedRot));
    }

    IEnumerator MoveDoor(Quaternion point)
    {
        oppening = true;

        float t = 0;

        while(true)
        {
            door.rotation = Quaternion.Lerp(door.rotation, point, t);
            t += Time.deltaTime * data.speedMultiplier;
            if(t >= 1) 
            {
                oppening = false;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && data.autoOpen && !queuedAction) StartCoroutine(WaitForAction("Open"));
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player" && data.autoOpen && !queuedAction) StartCoroutine(WaitForAction("Close"));
    }

    IEnumerator WaitForAction(string action)
    {
        queuedAction = true;

        yield return new WaitUntil(() => !oppening);

        if(action == "Open") OpenDoor();
        else CloseDoor();

        queuedAction = false;
    }
}
