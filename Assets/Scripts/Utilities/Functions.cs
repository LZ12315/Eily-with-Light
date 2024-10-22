using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorApproximate
{
    public static bool Approximate(Vector3 vector1, Vector3 vector2)
    {
        Vector3 distanceVector = vector1 - vector2;

        if(distanceVector.magnitude < 0.1f)
            return true;
        else 
            return false;
    }
}
