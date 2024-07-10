using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingRopeEnabler : MonoBehaviour
{
    private ClimbingRope[] ropes;

    private void Awake()
    {
        ropes = gameObject.GetComponentsInChildren<ClimbingRope>();
    }

    private void Start()
    {
        foreach (var rope in ropes)
        {
            rope.enabled = true;
            rope.PlayerOnThisRope += PlayerOnThisRope;
        }
    }

    private void OnDisable()
    {
        foreach (var rope in ropes)
        {
            rope.PlayerOnThisRope -= PlayerOnThisRope;
        }
    }

    // on == true: on this rope
    // on == false: off this rope
    private void PlayerOnThisRope(int instanceID, bool on)
    {
        foreach (var rope in ropes)
        {
            if (instanceID != rope.GetInstanceID()) rope._playerOnOtherRope = on;
        }
    }
}
