using UnityEngine;
using Obi;

public class RespawnRopeOnPlayerRespawnUntilRopeDetached : MonoBehaviour
{
    private ObiRope _rope;
    private ObiParticleAttachment[] _attachments;

    public bool RopeDetached { get; private set; }

    private void Awake()
    {
        _rope = gameObject.GetComponent<ObiRope>();
        _attachments = gameObject.GetComponents<ObiParticleAttachment>();
        RopeDetached = false;
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
        if (RopeDetached) return;
        evalRopeDetached();
        resetRope();
    }

    private void evalRopeDetached()
    {
        foreach (var attachment in _attachments) if (attachment.enabled) return;
        RopeDetached = true;
    }

    private void resetRope()
    {
        if (_rope != null && !RopeDetached)
        {
            _rope.ResetParticles();
        }
    }
}
