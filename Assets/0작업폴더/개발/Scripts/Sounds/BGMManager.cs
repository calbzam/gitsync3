using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [SerializeField] private AudioSource _stage1_1Bgm;
    [SerializeField] private StageBGMFade _stage1_1Fade;

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
        _stage1_1Fade.EvalVolumePercentInt();
        _stage1_1Bgm.volume = _stage1_1Fade.VolumePercent;
    }

    private void Update()
    {
        if (_stage1_1Fade.PlayerIsInside)
        {
            _stage1_1Bgm.volume = _stage1_1Fade.VolumePercent;
        }
    }
}
