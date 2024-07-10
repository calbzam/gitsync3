using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingRopeEnabler : MonoBehaviour
{
    private void Start()
    {
        foreach (var rope in gameObject.GetComponentsInChildren<ClimbingRope>())
        {
            rope.enabled = true;
        }
    }
}
