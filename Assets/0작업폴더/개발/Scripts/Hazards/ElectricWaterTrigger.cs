public class ElectricWaterTrigger : ScreenFadeRespawn
{
    protected override void BeforeScreenFadeOut()
    {
        PlayerLogic.AnimTools.PlayAnimation(PlayerAnimTools.AnimState.Electrocuted);
        PlayerLogic.PlayerElectrocutedText.gameObject.SetActive(true);

        PlayerLogic.LockPlayer();
        PlayerLogic.AnimController.AnimLocked = true;
    }

    protected override void AfterRespawned()
    {
        PlayerLogic.PlayerElectrocutedText.gameObject.SetActive(false);

        PlayerLogic.AnimController.AnimLocked = false;
    }
}
