using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RangExtension
{


    /// <summary>
    /// 获取区间内随机值
    /// </summary>
    /// <param name="o">Range</param>
    /// <returns>随机值</returns>
    public static int RandomInRange(this RangeInt o)
    {
        return Random.Range(o.start, o.end + 1);
    }
    
    
}
