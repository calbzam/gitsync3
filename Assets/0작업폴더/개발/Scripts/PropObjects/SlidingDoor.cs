using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : LeverConnectedObject
{
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private float _moveAmountY = 8;
    [SerializeField] private float _checkMargin = 0.02f;

    private bool _isOpening;
    private Vector3 _destPos;

    private void Start()
    {
        _isOpening = false;
        _destPos = new Vector3(transform.position.x, transform.position.y + _moveAmountY, transform.position.z);
    }

    public override void ActivatedAction(bool enabledState)
    {
        _isOpening = true;
    }

    private void Update()
    {
        if (_isOpening)
        {
            transform.position = Vector3.MoveTowards(transform.position, _destPos, _moveSpeed * Time.deltaTime);
            if (MyMath.Vector2DiffLessThan(transform.position, _destPos, _checkMargin)) _isOpening = false;
        }
    }
}
