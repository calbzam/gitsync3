using UnityEngine;

public class RespawnCraneHandOnPlayerRespawn : MonoBehaviour
{
    [SerializeField] private LeverConnectedCraneRope _craneRope;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _craneRope.CraneRopeResetted += ResetCraneHandPositon;
    }

    private void OnDisable()
    {
        _craneRope.CraneRopeResetted -= ResetCraneHandPositon;
    }

    private void ResetCraneHandPositon(Vector3 lastParticleGlobalPos)
    {
        transform.position = lastParticleGlobalPos;
    }
}
