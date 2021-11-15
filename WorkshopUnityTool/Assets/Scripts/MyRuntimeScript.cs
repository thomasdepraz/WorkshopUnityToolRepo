using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRuntimeScript : MonoBehaviour
{
    public Color someColor;
    public Vector3 vector;
    public AnimationCurve someCurve;
    public Transform _transform;
    public float someFloat;
    public Color[] colorArray;
    public myStruct someStruct;

}

[System.Serializable]
public struct myStruct
{
    public float structFloat;
    public Color structColor;
}