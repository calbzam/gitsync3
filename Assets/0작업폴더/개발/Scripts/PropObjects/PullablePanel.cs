using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PullablePanel : MonoBehaviour
{
    [SerializeField] private GameObject[] _gameobjsToActivateChildRbsOnPullStart;

    private HingeJoint2D _joint;

    public event Action<bool> PlayerIsInRange;
    public void InvokePlayerIsInRange(bool playerIsInRange) => PlayerIsInRange?.Invoke(playerIsInRange);

    private bool _playerIsConnected;
    private bool _isPickedUp;
    private bool _firstPickedUp;

    private void Awake()
    {
        _playerIsConnected = false;
        _isPickedUp = false;
        _firstPickedUp = false;
        _joint = gameObject.GetComponent<HingeJoint2D>();
    }

    private void Start()
    {
        _joint.enabled = false;
        initRbsBodytype();
    }

    private void OnEnable()
    {
        PlayerIsInRange += PlayerIsInRangeAction;
    }

    private void OnDisable()
    {
        PlayerIsInRange -= PlayerIsInRangeAction;
    }

    private void initRbsBodytype()
    {
        if (_gameobjsToActivateChildRbsOnPullStart != null)
        {
            foreach (var parentRb in _gameobjsToActivateChildRbsOnPullStart)
            {
                var _childRbArray = parentRb.GetComponentsInChildren<Rigidbody2D>();
                if (_childRbArray != null) foreach (var rb in _childRbArray) rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }
    }

    private void toggleRbsBodytype()
    {
        if (_gameobjsToActivateChildRbsOnPullStart != null)
        {
            foreach (var parentRb in _gameobjsToActivateChildRbsOnPullStart)
            {
                var _childRbArray = parentRb.GetComponentsInChildren<Rigidbody2D>();
                if (_childRbArray != null) foreach (var rb in _childRbArray) rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }

    private void PlayerIsInRangeAction(bool playerIsInRange)
    {
        if (playerIsInRange || _playerIsConnected) CentralInputReader.Input.Player.PickupActivate.started += PickupActivatedAction;
        else CentralInputReader.Input.Player.PickupActivate.started -= PickupActivatedAction;
    }

    private void PickupActivatedAction(InputAction.CallbackContext ctx)
    {
        _isPickedUp = !_isPickedUp;

        if (_isPickedUp)
        {
            _joint.enabled = true;
            _playerIsConnected = true;
            if (!_firstPickedUp) { _firstPickedUp = true; toggleRbsBodytype(); }
        }
        else
        {
            _joint.enabled = false;
            _playerIsConnected = false;
        }
    }
}
