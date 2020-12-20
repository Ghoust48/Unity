using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Records
{
    private static float _recordDistance;
    
    public static float RecordDistance
    {
        get
        {
            return _recordDistance;
        }
        set
        {
            if (_recordDistance < value)
            {
                _recordDistance = value;
            }
        }
    }

}
