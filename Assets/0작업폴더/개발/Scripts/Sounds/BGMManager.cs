using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [SerializeField] private AudioSource _stage1_1Bgm;
    [SerializeField] private StageBGMPlay _stage1_1Play;
    [SerializeField] private StageBGMFade _stage1_1Fade;
    [Header("")]
    [SerializeField] private AudioSource _stage1_2Bgm;
    [SerializeField] private StageBGMPlay _stage1_2Play;

    private void OnEnable()
    {
        PlayerLogic.PlayerRespawned += PlayerRespawnedAction;
        _stage1_1Play.PlayerEntered += Stage1_1EnteredAction;
        _stage1_2Play.PlayerEntered += Stage1_2EnteredAction;
    }

    private void OnDisable()
    {
        PlayerLogic.PlayerRespawned -= PlayerRespawnedAction;
        _stage1_1Play.PlayerEntered -= Stage1_1EnteredAction;
        _stage1_2Play.PlayerEntered -= Stage1_2EnteredAction;
    }

    private void PlayerRespawnedAction()
    {
        _stage1_1Fade.EvalVolumePercentInt();
        _stage1_1Bgm.volume = _stage1_1Fade.VolumePercent;
    }

    private void Stage1_1EnteredAction()
    {
        _stage1_1Bgm.Play();
        _stage1_2Bgm.Stop();
        _stage1_2Play.StageBGMStarted = false;
    }

    private void Stage1_2EnteredAction()
    {
        _stage1_2Bgm.Play();
        _stage1_1Bgm.Stop();
        _stage1_1Play.StageBGMStarted = false;
    }

    private void Update()
    {
        if (_stage1_1Fade.PlayerInZone)
        {
            _stage1_1Bgm.volume = _stage1_1Fade.VolumePercent;
        }
    }
}
