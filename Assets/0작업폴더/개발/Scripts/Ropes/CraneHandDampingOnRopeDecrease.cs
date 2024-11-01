using UnityEngine;
using RopeLengthStatus = LeverConnectedCraneRope.RopeLengthStatus;

public class CraneHandDampingOnRopeDecrease : MonoBehaviour
{
    [SerializeField] private LeverConnectedCraneRope _craneRope;
    [SerializeField] private float _rbDampingAmount = 3;

    private RopeLengthStatus _ropeLengthStatus;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _ropeLengthStatus = RopeLengthStatus.Unchanging;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        EvalDragAmount();
    }

    private void Update()
    {
        if (_ropeLengthStatus != _craneRope.RopeLengthChangeStatus)
        {
            _ropeLengthStatus = _craneRope.RopeLengthChangeStatus;
            EvalDragAmount();
        }
    }

    private void EvalDragAmount()
    {
        if (_ropeLengthStatus == RopeLengthStatus.Increasing) _rb.drag = 0;
        else _rb.drag = _rbDampingAmount;

        // reset damped velocities
        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0;
    }
}
