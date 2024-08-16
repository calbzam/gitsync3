using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCameraBoundsOnTrigger : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D _onTriggerEnterBounds;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            CameraBounds.VirtualCamDefaultConfiner.m_BoundingShape2D = _onTriggerEnterBounds;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            CameraBounds.VirtualCamDefaultConfiner.m_BoundingShape2D = _onTriggerEnterBounds;
        }
    }
}
