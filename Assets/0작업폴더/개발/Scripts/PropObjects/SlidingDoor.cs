using UnityEngine;

public class SlidingDoor : LeverConnectedObject
{
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private float _moveAmountY = 8;
    [SerializeField] private float _checkMargin = 0.02f;

    private enum DoorStatus
    {
        Static,
        IsOpening,
        IsClosing,
    }

    private DoorStatus _doorStatus;
    private Vector3 _destPos, _sourcePos;

    private void Start()
    {
        _doorStatus = DoorStatus.Static;
        _destPos = new Vector3(transform.position.x, transform.position.y + _moveAmountY, transform.position.z);
        _sourcePos = transform.position;
    }

    public override void ActivatedAction(bool enabledState)
    {
        _doorStatus = enabledState ? DoorStatus.IsOpening : DoorStatus.IsClosing;
    }

    private void Update()
    {
        if (_doorStatus == DoorStatus.IsOpening)
        {
            transform.position = Vector3.MoveTowards(transform.position, _destPos, _moveSpeed * Time.deltaTime);
            if (MyMath.Vector2DiffLessThan(transform.position, _destPos, _checkMargin)) _doorStatus = DoorStatus.Static;
        }
        else if (_doorStatus == DoorStatus.IsClosing)
        {
            transform.position = Vector3.MoveTowards(transform.position, _destPos, _moveSpeed * Time.deltaTime);
            if (MyMath.Vector2DiffLessThan(transform.position, _sourcePos, _checkMargin)) _doorStatus = DoorStatus.Static;
        }
    }
}
