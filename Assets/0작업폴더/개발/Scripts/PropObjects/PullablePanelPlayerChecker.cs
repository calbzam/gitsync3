using System;
using UnityEngine;

public class PullablePanelPlayerChecker : MonoBehaviour
{
    [SerializeField] private PullablePanel _pullableBase;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(Tags.PlayerTag))
        {
            _pullableBase.InvokePlayerIsInRange(true);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag(Tags.PlayerTag))
        {
            _pullableBase.InvokePlayerIsInRange(false);
        }
    }
}
