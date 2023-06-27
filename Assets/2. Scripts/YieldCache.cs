using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class YieldCache  
{
    class FloatCompare : IEqualityComparer<float>
    {
        bool IEqualityComparer<float>.Equals(float x, float y)
        {
            return x == y;
        }

        int IEqualityComparer<float>.GetHashCode(float obj)
        {
            return obj.GetHashCode();
        }
    }

    static readonly Dictionary<float, WaitForSeconds> timeList = new Dictionary<float, WaitForSeconds>(new FloatCompare());
    static readonly Dictionary<float, WaitForSecondsRealtime> realTimeList = new Dictionary<float, WaitForSecondsRealtime>(new FloatCompare());

    public static readonly WaitForEndOfFrame WaitForEndOffFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();

    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        seconds = (float)System.Math.Round(seconds, 3);
        if (!timeList.TryGetValue(seconds, out WaitForSeconds wfs))
        {
            timeList.Add(seconds, wfs = new WaitForSeconds(seconds));
        }

        return wfs;
    }

    public static WaitForSecondsRealtime WaitForSecondsRealtime(float seconds)
    {
        seconds = (float)System.Math.Round(seconds, 3);
        if (!realTimeList.TryGetValue(seconds, out WaitForSecondsRealtime wfsrt))
        {
            realTimeList.Add(seconds, wfsrt = new WaitForSecondsRealtime(seconds));
        }

        return wfsrt;
    }
}
