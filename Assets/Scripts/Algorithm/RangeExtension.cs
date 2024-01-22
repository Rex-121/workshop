using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RangExtension
{


    /// <summary>
    /// 获取区间内随机值
    /// </summary>
    /// <param name="o">RangeInt-`start`:MIN, `length`:MAX</param>
    /// <returns>随机值(`start`-`length`之间的随机值，包括`start``length`)</returns>
    public static int RandomInRange(this RangeInt o)
    {
        return Random.Range(o.start, o.end);
    }
    
    
}
