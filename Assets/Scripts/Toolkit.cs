using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A static class that contains several values, functions and whatnot that we might use in more than one part of the game.
/// Makes access to things quicker and easier :) 
/// Feel free to add more stuff and use this class as you wish. 
/// </summary>
public static class Toolkit
{
    /// <summary>
    /// Compare two vectors. 
    /// Returns true if vector one is closer to vector two than the allowed difference.
    /// </summary>
    /// <param name="vectorOne"></param>
    /// <param name="vectorTwo"></param>
    /// <param name="allowedDifference"></param>
    /// <returns></returns>
    public static bool CompareVectors(Vector3 vectorOne, Vector3 vectorTwo, float allowedDifference)
    {
        var dx = vectorOne.x - vectorTwo.x;
        if (Mathf.Abs(dx) > allowedDifference)
            return false;

        var dy = vectorOne.y - vectorTwo.y;
        if (Mathf.Abs(dy) > allowedDifference)
            return false;

        var dz = vectorOne.z - vectorTwo.z;
        if (Mathf.Abs(dz) > allowedDifference)
            return false;

        return true;
    }
}
