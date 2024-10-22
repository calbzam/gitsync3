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

    // playerOnOther == true: on other rope
    // playerOnOther == false: off other rope
    private void PlayerOnThisObject(int instanceID, bool playerOnOther)
    {
        foreach (var rope in ropes)
        {
            if (instanceID != rope.gameObject.GetInstanceID()) rope.PlayerOnOtherObject = playerOnOther;
        }
    }
}
