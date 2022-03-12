using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static float Map(float value, float from1, float to1, float from2, float to2)
    {
        float scale = (to2 - to1) / (from2 - from1);
        return (to1 + ((value - from1) * scale));
    }
}
