using UnityEngine;

public partial class BatteryPickup : MonoBehaviour
{
    private Transform _initialParent;
    private Vector3 _initialPos;
    private Quaternion _initialRot;

    private void PlayerRespawnedAction()
    {
        if (!BatteryInserted)
        {
            ResetBools();
            DetachFromPlayer();
            SetBatteryParent(_initialParent);
            ResetTransform();
        }
    }

    private void ResetTransform()
    {
        transform.position = _initialPos;
        transform.rotation = _initialRot;
    }
}