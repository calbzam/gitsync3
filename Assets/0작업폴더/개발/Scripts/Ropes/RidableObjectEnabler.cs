using System;
using UnityEngine;

public class RidableObjectEnabler : MonoBehaviour
{
    private RidableObject[] ropes;

    private void Awake()
    {
        ropes = gameObject.GetComponentsInChildren<RidableObject>();
    }

    private void Start()
    {
        foreach (var rope in ropes)
        {
            rope.enabled = true;
            rope.PlayerOnThisObject += PlayerOnThisObject;
        }
    }

    private void OnDisable()
    {
        foreach (var rope in ropes)
        {
            rope.PlayerOnThisObject -= PlayerOnThisObject;
        }
    }

    // on == true: on this rope
    // on == false: off this rope
    private void PlayerOnThisObject(int instanceID, bool on)
    {
        foreach (var rope in ropes)
        {
            if (instanceID != rope.GetInstanceID()) rope._playerOnOtherObject = on;
        }
    }
}
