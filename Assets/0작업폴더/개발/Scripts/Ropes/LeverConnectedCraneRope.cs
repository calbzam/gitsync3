using UnityEngine;
using Obi;

public class LeverConnectedCraneRope : LeverConnectedObject
{
    [Help("Needs ObiRope and ObiRopeCursor on this object")]
    [SerializeField] private float _ropeSpeed = 5;
    private static float _lerpSpeed = 5; // speed for (_currentRopeSpeed MoveTowards _ropeSpeed)
    private float _currentRopeSpeed;
    [SerializeField] private float _switchDelay = 3;
    private float _switchDelayTimer;
    private bool _switchActivated;
    private bool _firstTimeActivated;
    [SerializeField] private float _maxRopeLength = 55;
    [SerializeField] private float _minRopeLength = 18;

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
        _switchActivated = false;
        _firstTimeActivated = true;
        _switchDelayTimer = 0f;
        RopeLengthChangeStatus = RopeLengthStatus.Unchanging;
        _rope = gameObject.GetComponent<ObiRope>();
        _ropeCursor = gameObject.GetComponent<ObiRopeCursor>();
    }

    public override void ActivatedAction(bool enabledState)
    {
        RopeLengthChangeStatus = enabledState ? RopeLengthStatus.Increasing : RopeLengthStatus.Decreasing;

        if (_firstTimeActivated)
        {
            _firstTimeActivated = false;
            _switchDelayTimer = Time.time - (_switchDelay + 0.1f);
        }
        else
        {
            _switchDelayTimer = Time.time;
        }

        _switchActivated = true;
    }

    private void Update()
    {
        if (_switchActivated)
        {
            if (Time.time - _switchDelayTimer > _switchDelay)
            {
                ChangeRopeLength();
            }
        }
    }

    private void ChangeRopeLength()
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

        if (_currentRopeSpeed == 0)
        {
            _switchActivated = false;
        }

        else
        {
            _ropeCursor.ChangeLength(_rope.restLength + _currentRopeSpeed * Time.deltaTime);
        }
    }
}
