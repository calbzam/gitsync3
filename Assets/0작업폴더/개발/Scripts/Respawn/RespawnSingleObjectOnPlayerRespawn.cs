using UnityEngine;

public class RespawnSingleObjectOnPlayerRespawn : MonoBehaviour
{
    protected Transform _initialParent;
    protected Vector3 _initialPos;
    protected Quaternion _initialRot;

    protected Rigidbody2D _rb2D;
    protected Rigidbody _rb3D;

    protected Vector3 _initialVelocity;

    protected float _rb2DInitialAngularVelocity;
    protected Vector3 _rb3DInitialAngularVelocity;

    private void Start()
    {
        _initialParent = transform.parent;
        _initialPos = transform.position;
        _initialRot = transform.rotation;

        _rb2D = GetComponent<Rigidbody2D>();
        _rb3D = GetComponent<Rigidbody>();

        if (_rb2D != null) { _initialVelocity = _rb2D.velocity; _rb2DInitialAngularVelocity = _rb2D.angularVelocity; }
        else if (_rb3D != null) { _initialVelocity = _rb3D.velocity; _rb3DInitialAngularVelocity = _rb3D.angularVelocity; }
        else _initialVelocity = Vector3.zero;
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
        transform.SetParent(_initialParent);
        ResetTransform();
        ResetRb();
    }

    protected virtual void ResetTransform()
    {
        transform.position = _initialPos;
        transform.rotation = _initialRot;
    }

    private void ResetRb()
    {
        if (_rb2D != null)
        {
            _rb2D.velocity = _initialVelocity;
            _rb2D.angularVelocity = _rb2DInitialAngularVelocity;
        }
        else if (_rb3D != null)
        {
            _rb3D.velocity = _initialVelocity;
            _rb3D.angularVelocity = _rb3DInitialAngularVelocity;
        }
    }
}
