using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameIndependentLerp : MonoBehaviour
{
    public static Vector3 V3Damp(Vector3 a, Vector3 b, float lambda, float dt)
    {
        return Vector3.Lerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }
}
