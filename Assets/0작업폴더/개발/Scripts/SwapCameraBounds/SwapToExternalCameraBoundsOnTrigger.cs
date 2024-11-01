using UnityEngine;

public class SwapToExternalCameraBoundsOnTrigger : SwapCameraBoundsOnTrigger
{
    public void SetCameraBounds(PolygonCollider2D newCameraBounds) => _cameraBoundsToUse = newCameraBounds;
}
