using UnityEngine;
using Obi;

public class LeverConnectedCraneRope : LeverConnectedObject
{
    [Help("Needs ObiRope and ObiRopeCursor on this object")]
    [SerializeField] private float _ropeSpeed = 5;
    [SerializeField] private float _maxRopeLength = 50;

    private bool _craneMoveStart;

    private ObiRope _rope;
    private ObiRopeCursor _ropeCursor;

    private void Awake()
    {
        _craneMoveStart = false;
        _rope = gameObject.GetComponent<ObiRope>();
        _ropeCursor = gameObject.GetComponent<ObiRopeCursor>();
    }

    private void Update()
    {
        if (_craneMoveStart)
        {
            _ropeCursor.ChangeLength(_rope.restLength + _ropeSpeed * Time.deltaTime);
            if (_rope.restLength > _maxRopeLength) _craneMoveStart = false;
        }
    }

    public override void ActivatedAction(bool enabledState)
    {
        _craneMoveStart = true;
    }
}
