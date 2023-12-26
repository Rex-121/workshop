using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RangExtension
{


    public static int RandomInRange(this RangeInt o)
    {
        return Random.Range(o.start, o.end + 1);
    }
    
    
}
