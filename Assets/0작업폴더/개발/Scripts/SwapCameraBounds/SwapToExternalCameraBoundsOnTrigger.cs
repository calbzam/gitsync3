using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapToExternalCameraBoundsOnTrigger : SwapCameraBoundsOnTrigger
{
    public void SetCameraBounds(PolygonCollider2D newCameraBounds) => _cameraBoundsToUse = newCameraBounds;
}
