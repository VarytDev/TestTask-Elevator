using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Globally cached waiters to avoid allocations
/// </summary>
public static class Waiters
{
    static Dictionary<float, WaitForSeconds> timeInterval = new Dictionary<float, WaitForSeconds>(100);
    static WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
    static WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate();

    public static WaitForEndOfFrame EndOfFrame
    {
        get
        {
            return endOfFrame;
        }
    }

    public static WaitForFixedUpdate FixedUpdate
    {
        get
        {
            return fixedUpdate;
        }
    }

    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        if (!timeInterval.ContainsKey(seconds))
        {
            timeInterval.Add(seconds, new WaitForSeconds(seconds));
        }
            
        return timeInterval[seconds];
    }
}
