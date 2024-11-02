using UnityEngine;

public class ZipLinePulley : MonoBehaviour
{
    [SerializeField] private ZipLineHandle _zipLineHandle;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Tags.ZipLineStoppingStartBlockTag)
            || collision.gameObject.CompareTag(Tags.ZipLineStoppingEndBlockTag))
        {
            _zipLineHandle.StopMovingPulley = true;
        }
    }
}
