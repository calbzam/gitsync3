using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [SerializeField] private AudioSource _stage1_1Bgm;
    [SerializeField] private StageBGMPlay _stage1_1Play;
    [SerializeField] private StageBGMFade _stage1_1Fade;
    [Header("")]
    [SerializeField] private AudioSource _stage1_2_DrumBgm;
    [SerializeField] private StageBGMFade _stage1_2_DrumFade;
    [Header("")]
    [SerializeField] private AudioSource _stage1_2Bgm;
    [SerializeField] private StageBGMPlay _stage1_2Play;

    private bool _waitingForStage1_2DrumStop;

    private void Start()
    {
        _waitingForStage1_2DrumStop = false;
    }

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

        _stage1_2_DrumBgm.Stop();
        _stage1_2Bgm.Stop();
    }

    private void Stage1_2EnteredAction()
    {
        //_stage1_2Bgm.Play();

        _waitingForStage1_2DrumStop = true;
        _stage1_1Bgm.Stop();
    }

    private bool AudioLoopEnded(AudioSource audioSource)
    {
        return (!audioSource.isPlaying || audioSource.clip.length - audioSource.time < 0.05f);
    }

    private void Update()
    {
        if (_stage1_1Fade.PlayerInZone)
        {
            _stage1_1Bgm.volume = _stage1_1Fade.VolumePercent;
        }

        if (_stage1_2_DrumFade.PlayerInZone)
        {
            if (!_stage1_2_DrumBgm.isPlaying) _stage1_2_DrumBgm.Play();
            _stage1_2_DrumBgm.volume = _stage1_2_DrumFade.VolumePercent;
        }

        if (_waitingForStage1_2DrumStop)
        {
            if (AudioLoopEnded(_stage1_2_DrumBgm))
            {
                _waitingForStage1_2DrumStop = false;
                _stage1_2Bgm.Play();
                _stage1_2_DrumBgm.Stop();
            }
        }
    }
}
