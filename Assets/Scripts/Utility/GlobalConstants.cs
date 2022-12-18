using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GlobalConstants : ScriptableObject
{

    public float minXHairDifference = 50f;
    public float maxFireInterval = 0.5f;
    public float maxCoverInterval = 0.75f;
    public float firePressDuration = 0.3f;
    public float coverStateChangeSpeed = 2.5f;
    public float endofLevelDelay = 2f;

}
