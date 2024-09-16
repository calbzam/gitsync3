using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZipLinePulley : MonoBehaviour
{
    [SerializeField] private ZipLineHandle _zipLineHandle;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ZipLineStoppingBlock"))
        {
            _zipLineHandle.StopMovingPulley = true;
        }
    }
}
