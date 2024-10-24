using System.Collections;
using UnityEngine;

public class DeadZone : ScreenFadeRespawn
{
    [SerializeField] private int _playerMaxDistanceFromCenterToStop = 1;

    protected override void BeforeScreenFadeOut()
    {
        StartCoroutine(LockPlayerAfterSmallMove());
    }

    protected override void AfterRespawned()
    {

    }

    private IEnumerator LockPlayerAfterSmallMove()
    {
        yield return new WaitUntil(() => MyMath.Vector2DiffOrLessThan(PlayerLogic.Player.transform.position, transform.position, _playerMaxDistanceFromCenterToStop));
        PlayerLogic.LockPlayer();
    }
}
