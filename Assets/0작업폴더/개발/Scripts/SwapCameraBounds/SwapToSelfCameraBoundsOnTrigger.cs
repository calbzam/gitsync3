using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapToSelfCameraBoundsOnTrigger : SwapCameraBoundsOnTrigger
{
    [SerializeField] private PolygonCollider2D _onTriggerEnterBounds;

    private void Start()
    {
        _cameraBoundsToUse = _onTriggerEnterBounds;
        if (_cameraBoundsToUse == null)
            Debug.Log("_onTriggerEnterBounds is null, camerabounds also null:  " + transform.parent.name + " > " + name);
    }
}
