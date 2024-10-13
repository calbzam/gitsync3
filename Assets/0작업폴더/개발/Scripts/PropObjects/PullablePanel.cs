using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PullablePanel : MonoBehaviour
{
    [Help("Needs HingeJoint2D on this object")]

    [SerializeField] private GameObject _childRbsToToggleBodytype;

    private HingeJoint2D _joint;
    private Rigidbody2D[] _rbsToToggleBodytype;

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
        if (_childRbsToToggleBodytype != null)
        {
            _rbsToToggleBodytype = _childRbsToToggleBodytype.GetComponentsInChildren<Rigidbody2D>();
            foreach (var rb in _rbsToToggleBodytype) rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void toggleRbsBodytype()
    {
        if (_childRbsToToggleBodytype != null)
        {
            foreach (var rb in _rbsToToggleBodytype) rb.bodyType = RigidbodyType2D.Dynamic;
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
