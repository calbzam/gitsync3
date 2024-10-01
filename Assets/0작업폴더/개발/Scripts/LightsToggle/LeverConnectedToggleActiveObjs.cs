using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverConnectedToggleActiveObjs : LeverConnectedObject
{
    [SerializeField] private GameObject[] _objsToToggleSetActive;

    private void Start()
    {
        foreach (var obj in _objsToToggleSetActive)
            obj.SetActive(false);
    }

    public override void ActivatedAction(bool enabledState)
    {
        foreach (var obj in _objsToToggleSetActive)
            obj.SetActive(enabledState);
    }
}
