public class PickableObjectWithCollision : PickableObject // WITH collision and mass of object
{
    private float _objOrigDensity;

    private void Start()
    {
        _objOrigDensity = _objCol.density;
    }

    protected override void AttachObjectToPlayer()
    {
        IsHeldByPlayer = true;
        PlayerLogic.PlayerHoldingSomething = true;

        _objRb.simulated = false;
        SetObjectFacingDir();
        _objRb.transform.SetParent(PlayerLogic.Player.transform, true);
        _objCol.transform.SetParent(PlayerLogic.Player.transform, true);
        _objCol.density = 0; // Player Rb uses auto mass
    }

    protected override void DetachObjectFromPlayer()
    {
        IsHeldByPlayer = false;
        PlayerLogic.PlayerHoldingSomething = false;

        _objCol.density = _objOrigDensity;
        _objCol.transform.SetParent(_objRb.transform, true);
        _objRb.transform.SetParent(null, true);
        _objRb.simulated = true;
    }
}
