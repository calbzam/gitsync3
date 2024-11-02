using UnityEngine;
using Obi;
using System;

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
    private float _origRopeLength;
    private float _avgParticleDistanceY;
    private int _evenDistanceStartElemIndex;

    public event Action<Vector3> CraneRopeResetted;

    private void Awake()
    {
        _rope = gameObject.GetComponent<ObiRope>();
        _ropeCursor = gameObject.GetComponent<ObiRopeCursor>();
    }

    private void Start()
    {
        _origRopeLength = _rope.restLength;
        RopeLengthChangeStatus = RopeLengthStatus.Unchanging;
        resetCraneRopeFucked();
        evalInitialAvgYOfParticles();
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

    private void OnEnable()
    {
        PlayerLogic.PlayerRespawned += PlayerRespawnedAction;
    }

    private void OnDisable()
    {
        PlayerLogic.PlayerRespawned -= PlayerRespawnedAction;
    }

    private void PlayerRespawnedAction()
    {
        resetCraneRopeFucked();
    }

    private void ResetCraneRope()
    {
        if (!_firstTimeActivated)
        {
            _firstTimeActivated = true;

            if (RopeLengthChangeStatus == RopeLengthStatus.Increasing) _ropeCursor.ChangeLength(_maxRopeLength);
            else if (RopeLengthChangeStatus == RopeLengthStatus.Decreasing) _ropeCursor.ChangeLength(_minRopeLength);
            RopeLengthChangeStatus = RopeLengthStatus.Unchanging;
        }
        _switchActivated = false;
        _currentRopeSpeed = 0;
        _switchDelayTimer = 0;
    }

    private void resetCraneRopeFucked()
    {
        var prevStatus = RopeLengthChangeStatus;
        if (!_firstTimeActivated)
        {
            _firstTimeActivated = true;

            if (RopeLengthChangeStatus == RopeLengthStatus.Increasing) changeLengthAndRepositionParticles(_maxRopeLength);
            else if (RopeLengthChangeStatus == RopeLengthStatus.Decreasing) changeLengthAndRepositionParticles(_minRopeLength);
            RopeLengthChangeStatus = RopeLengthStatus.Unchanging;
        }
        
        _switchActivated = false;
        _currentRopeSpeed = 0;
        _switchDelayTimer = 0;
    }

    private void evalInitialAvgYOfParticles()
    {
        /* Solution Process: */

        //Debug.Log("global diff:");
        //Vector2 prevPos = RopeCalcs.GetGlobalParticlePos(transform, _rope.solver.positions[_rope.elements[0].particle1]);
        //string printstr = "";
        //int elemIndex = 0;
        //foreach (var elem in _rope.elements)
        //{
        //    Vector2 pos = RopeCalcs.GetGlobalParticlePos(transform, _rope.solver.positions[elem.particle2]);
        //    printstr += elemIndex + ": " + (pos - prevPos) + ", ";
        //    prevPos = pos;
        //    ++elemIndex;
        //}
        //Debug.Log(printstr);

        /* Solution Process print result: */

        //0: (-0.01, 0.47), 1: (-0.01, 0.47), 2: (-0.03, 0.47), 3: (-0.05, 0.47), 4: (-0.16, 0.43),
        //5: (-0.46, 0.11), 6: (-0.48, 0.10), 7: (-0.48, 0.10), 8: (-0.49, 0.10), 9: (-0.49, 0.10),
        //10: (-0.48, 0.09), 11: (-0.48, 0.09), 12: (-0.49, 0.09), 13: (-0.48, 0.09), 14: (-0.48, 0.09),
        //15: (-0.48, 0.09), 16: (-0.48, 0.09), 17: (-0.48, 0.09), 18: (-0.48, 0.09), 19: (-0.48, 0.09),
        //20: (-0.48, 0.09), 21: (-0.49, 0.09), 22: (-0.48, 0.09), 23: (-0.48, 0.09), 24: (-0.49, 0.09),
        //25: (-0.49, 0.08), 26: (-0.49, 0.08), 27: (-0.49, 0.08), 28: (-0.47, 0.05), 29: (-0.23, -0.23),
        //30: (-0.08, -0.33), 31: (-0.01, -0.47), 32: (0.00, -0.47), 33: (0.00, -0.48), 34: (0.00, -0.47),
        //35: (0.00, -0.48), 36: (0.00, -0.48), 37: (0.00, -0.48), 38: (0.00, -0.48), 39: (0.00, -0.48),
        //40: (0.00, -0.48), 41: (0.00, -0.48), 42: (0.00, -0.47), 43: (0.00, -0.48), 44: (0.00, -0.47), 

        Vector2 prevPos = _rope.solver.positions[_rope.elements[0].particle1];
        int diffXZeroParticlesCnt = 0;
        _avgParticleDistanceY = 0;
        _evenDistanceStartElemIndex = -1;
        for (int i = 0; i < _rope.elements.Count; ++i)
        {
            Vector2 pos = _rope.solver.positions[_rope.elements[i].particle2];
            Vector2 diff = new Vector2(Mathf.Abs(pos.x - prevPos.x), Mathf.Abs(pos.y - prevPos.y));
            if (diff.x < 0.001f)
            {
                if (_evenDistanceStartElemIndex < 0) _evenDistanceStartElemIndex = i;
                _avgParticleDistanceY += diff.y; ++diffXZeroParticlesCnt;
            }
            prevPos = pos;
        }
        _avgParticleDistanceY /= diffXZeroParticlesCnt;
    }

    private void changeLengthAndRepositionParticles(float newLength)
    {
        _ropeCursor.ChangeLength(newLength);

        for (int i = _evenDistanceStartElemIndex + 1; i < _rope.elements.Count; ++i)
        {
            Vector3 newParticlePos = _rope.solver.positions[_rope.elements[i - 1].particle2];
            newParticlePos.y -= _avgParticleDistanceY;
            _rope.solver.positions[_rope.elements[i].particle2] = newParticlePos;
        }

        int lastParticle = _rope.elements[_rope.elements.Count - 1].particle2;
        Vector3 lastParticleGlobalPos = RopeCalcs.GetGlobalParticlePos(transform, _rope.solver.positions[lastParticle]);
        CraneRopeResetted?.Invoke(lastParticleGlobalPos);
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
