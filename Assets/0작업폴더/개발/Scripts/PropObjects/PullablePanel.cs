using UnityEngine;
using UnityEngine.InputSystem;

public class PullablePanel : MonoBehaviour
{
    [SerializeField] private GameObject[] _parentObjsToDeactivateChildRbsUntilPullStarft;
    [SerializeField] private HingeJoint2D _playerJoint;
    [SerializeField] private BoxCollider2D _selfCollider;

    public bool PlayerIsInRange;

    private bool _playerIsConnected;
    private bool _isPickedUp;
    private bool _firstPickedUp;

    private static int _heldInstanceID; // instanceID of PullablePanel currently held by Player (== 0: no PullablePanel held by Player)

    private void Awake()
    {
        _playerIsConnected = false;
        _isPickedUp = false;
        _firstPickedUp = false;
        _heldInstanceID = 0;
    }

    private void Start()
    {
        _playerJoint.enabled = false;
        initRbsBodytype();
    }

    private void OnEnable()
    {
        CentralInputReader.Input.Player.PickupActivate.started += PickupActivatedAction;
    }

    private void OnDisable()
    {
        CentralInputReader.Input.Player.PickupActivate.started -= PickupActivatedAction;
    }

    private void initRbsBodytype()
    {
        if (_parentObjsToDeactivateChildRbsUntilPullStarft != null)
        {
            foreach (var parentRb in _parentObjsToDeactivateChildRbsUntilPullStarft)
            {
                var _childRbArray = parentRb.GetComponentsInChildren<Rigidbody2D>();
                if (_childRbArray != null) foreach (var rb in _childRbArray) rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }
    }

    private void toggleRbsBodytype()
    {
        if (_parentObjsToDeactivateChildRbsUntilPullStarft != null)
        {
            foreach (var parentRb in _parentObjsToDeactivateChildRbsUntilPullStarft)
            {
                var _childRbArray = parentRb.GetComponentsInChildren<Rigidbody2D>();
                if (_childRbArray != null) foreach (var rb in _childRbArray) rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }

    private void PickupActivatedAction(InputAction.CallbackContext ctx)
    {
        if (_heldInstanceID == 0 && PlayerIsInRange && !_playerIsConnected)
        {
            PickupPanel();
        }
        else
        {
            LetgoPanel();
        }
    }

    private void PickupPanel()
    {
        _heldInstanceID = GetInstanceID();

        _playerJoint.enabled = true;
        _playerIsConnected = true;
        if (!_firstPickedUp) { _firstPickedUp = true; toggleRbsBodytype(); }
    }

    public void LetgoPanel()
    {
        if (_heldInstanceID == GetInstanceID()) _heldInstanceID = 0;

        _playerJoint.enabled = false;
        _playerIsConnected = false;
    }

    private void OnCollisionEnter2D(Collision2D collision) // ignore collisions between PullablePanels
    {
        if (collision.collider.CompareTag(Tags.PullablePanelTag))
        {
            Physics2D.IgnoreCollision(_selfCollider, collision.collider);
        }
    }
}
