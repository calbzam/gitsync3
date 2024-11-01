using UnityEngine;
using Obi;

public class LeverConnectedCraneRope : LeverConnectedObject
{
    [Help("Needs ObiRope and ObiRopeCursor on this object")]
    [SerializeField] private float _ropeSpeed = 5;
    private float _currentRopeSpeed;
    private static float _lerpSpeed = 5; // speed for (_currentRopeSpeed MoveTowards _ropeSpeed)
    [SerializeField] private float _maxRopeLength = 50;
    [SerializeField] private float _minRopeLength = 20;

    public RopeLengthStatus RopeLengthChangeStatus { get; private set; }

    public enum RopeLengthStatus
    {
        Unchanging,
        Increasing,
        Decreasing,
    }

    private ObiRope _rope;
    private ObiRopeCursor _ropeCursor;

    private void Awake()
    {
        RopeLengthChangeStatus = RopeLengthStatus.Unchanging;
        _rope = gameObject.GetComponent<ObiRope>();
        _ropeCursor = gameObject.GetComponent<ObiRopeCursor>();
    }

    private void Update()
    {
        switch (RopeLengthChangeStatus)
        {
            case RopeLengthStatus.Increasing:
                _currentRopeSpeed = Mathf.MoveTowards(_currentRopeSpeed, _ropeSpeed, _lerpSpeed * Time.deltaTime);
                if (_rope.restLength > _maxRopeLength) RopeLengthChangeStatus = RopeLengthStatus.Unchanging;
                break;

            case RopeLengthStatus.Decreasing:
                _currentRopeSpeed = Mathf.MoveTowards(_currentRopeSpeed, -_ropeSpeed, _lerpSpeed * Time.deltaTime);
                if (_rope.restLength < _minRopeLength) RopeLengthChangeStatus = RopeLengthStatus.Unchanging;
                break;

            case RopeLengthStatus.Unchanging:
            default:
                _currentRopeSpeed = Mathf.MoveTowards(_currentRopeSpeed, 0, _lerpSpeed * Time.deltaTime);
                break;
        }

        if (_currentRopeSpeed != 0)
        {
            _ropeCursor.ChangeLength(_rope.restLength + _currentRopeSpeed * Time.deltaTime);
        }
    }

    public override void ActivatedAction(bool enabledState)
    {
        RopeLengthChangeStatus = enabledState ? RopeLengthStatus.Increasing : RopeLengthStatus.Decreasing;
    }
}
