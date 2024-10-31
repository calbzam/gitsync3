using UnityEngine;
using Obi;

public class LeverConnectedCraneRope : LeverConnectedObject
{
    [Help("Needs ObiRope and ObiRopeCursor on this object")]
    [SerializeField] private float _ropeSpeed = 5;
    [SerializeField] private float _maxRopeLength = 50;
    [SerializeField] private float _minRopeLength = 20;

    private RopeLengthStatus _ropeLengthStatus;

    private enum RopeLengthStatus
    {
        Static,
        Increasing,
        Decreasing,
    }

    private ObiRope _rope;
    private ObiRopeCursor _ropeCursor;

    private void Awake()
    {
        _ropeLengthStatus = RopeLengthStatus.Static;
        _rope = gameObject.GetComponent<ObiRope>();
        _ropeCursor = gameObject.GetComponent<ObiRopeCursor>();
    }

    private void Update()
    {
        if (_ropeLengthStatus == RopeLengthStatus.Increasing)
        {
            _ropeCursor.ChangeLength(_rope.restLength + _ropeSpeed * Time.deltaTime);
            if (_rope.restLength > _maxRopeLength) _ropeLengthStatus = RopeLengthStatus.Static;
        }
        else if (_ropeLengthStatus == RopeLengthStatus.Decreasing)
        {
            _ropeCursor.ChangeLength(_rope.restLength - _ropeSpeed * Time.deltaTime);
            if (_rope.restLength < _minRopeLength) _ropeLengthStatus = RopeLengthStatus.Static;
        }
    }

    public override void ActivatedAction(bool enabledState)
    {
        _ropeLengthStatus = enabledState ? RopeLengthStatus.Increasing : RopeLengthStatus.Decreasing;
    }
}
