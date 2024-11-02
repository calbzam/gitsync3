using UnityEngine;

public class SwapCameraBoundsOnTrigger : MonoBehaviour
{
    protected PolygonCollider2D _cameraBoundsToUse;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(Tags.PlayerTag))
        {
            CameraBounds.VirtualCamDefaultConfiner.m_BoundingShape2D = _cameraBoundsToUse;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(Tags.PlayerTag))
        {
            CameraBounds.VirtualCamDefaultConfiner.m_BoundingShape2D = _cameraBoundsToUse;
        }
    }
}
