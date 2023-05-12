using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DoorData
{
    [HideInInspector] public bool isOpen, isSelected;
    public bool isShut, autoOpen;
    public float speedMultiplier = 1;
}

[System.Serializable]
public class RotatingDoorData : DoorData
{
    public bool openRight = true;
    public int angle = 90;
}
