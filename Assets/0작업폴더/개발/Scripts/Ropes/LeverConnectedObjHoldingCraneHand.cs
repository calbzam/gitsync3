using UnityEngine;

public class LeverConnectedObjHoldingCraneHand : LeverConnectedObject
{
    [SerializeField] private Transform _parentToPoint;
    [SerializeField] private Rigidbody2D _connectedObjRb;

    private bool _hasActivatedOnce;

    private void Awake()
    {
        _hasActivatedOnce = false;
    }

    private void Start()
    {
        _connectedObjRb.transform.SetParent(_parentToPoint, true);
        _connectedObjRb.simulated = false;
    }

    public override void ActivatedAction(bool enabledState)
    {
        if (!_hasActivatedOnce)
        {
            _hasActivatedOnce = true;

            _connectedObjRb.transform.SetParent(null, true);
            _connectedObjRb.simulated = true;
        }
    }
}
