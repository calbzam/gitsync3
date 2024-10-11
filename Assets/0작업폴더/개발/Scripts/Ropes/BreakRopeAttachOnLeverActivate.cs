using Obi;

public class BreakRopeAttachsOnLeverActivate : LeverConnectedObject
{
    private ObiParticleAttachment[] _ropeAttachments;

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
    }
}
