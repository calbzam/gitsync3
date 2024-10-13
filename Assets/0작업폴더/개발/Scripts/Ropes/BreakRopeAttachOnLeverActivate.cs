using Obi;
using UnityEngine;

public class BreakRopeAttachsOnLeverActivate : LeverConnectedObject
{
    private ObiParticleAttachment[] _ropeAttachments;
    [SerializeField] private MakeRopeFloatOnWater _makeRopeFloat;

    public bool IsBroken { get; private set; }

    private void Awake()
    {
        IsBroken = false;
        _ropeAttachments = gameObject.GetComponents<ObiParticleAttachment>();
    }

    public override void ActivatedAction(bool enabledState)
    {
        foreach (var attachment in _ropeAttachments) attachment.enabled = false;
        IsBroken = true;
        _makeRopeFloat.AttachFloatingColliders();
    }
}
