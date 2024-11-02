using System.Collections;
using UnityEngine;

public class StageBGMFade : MonoBehaviour
{
    [SerializeField] private AudioSource _stageBgm;
    [SerializeField] private BoxCollider2D _selfCol;
    private float _colTopY, _colBottomY;
    private ColBounds2D _bgmBounds;

    [SerializeField] VolumeDir _volumeDir = VolumeDir.TopIsMaxVolume;
    [SerializeField] private float _volumeMin = 0.1f;
    private float _volumeMax;

    [Header("Fade in BGM on scene load")]
    [SerializeField] private bool _fadeInBGMOnSceneLoad = false;
    [SerializeField] private BoxCollider2D _bgmLoadFadeInArea;
    [SerializeField] private float _volumeFadeInSpeed = 1;

    public float VolumePercent { get; private set; }

    public bool PlayerInZone { get; private set; }

    private enum VolumeDir { TopIsMaxVolume, BottomIsMaxVolume, }

    private void Start()
    {
        _colTopY = _selfCol.bounds.max.y;
        _colBottomY = _selfCol.bounds.min.y;

        _volumeMax = _stageBgm.volume;
        EvalVolumePercentInt();
        EvalFadeInBGM();

        PlayerInZone = false;
    }

    public void EvalVolumePercentInt()
    {
        if (PlayerLogic.Player.transform.position.y > _colTopY) VolumePercent = _volumeMax;
        else if (PlayerLogic.Player.transform.position.y < _colBottomY) VolumePercent = _volumeMin;
    }

    private void EvalFadeInBGM()
    {
        if (_fadeInBGMOnSceneLoad)
        {
            _bgmBounds = new ColBounds2D(_bgmLoadFadeInArea);
            if (_bgmBounds.OtherIsInSelf(PlayerLogic.Player.transform)) StartCoroutine(IncreaseBgmVolume());
        }
    }

    private IEnumerator IncreaseBgmVolume()
    {
        _stageBgm.volume = 0;

        yield return new WaitUntil(() =>
        {
            _stageBgm.volume = Mathf.MoveTowards(_stageBgm.volume, _volumeMax, _volumeFadeInSpeed * Time.deltaTime);
            return _stageBgm.volume == _volumeMax;
        });
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(Tags.PlayerTag))
        {
            PlayerInZone = true;
            EvalVolumePercentInt();
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag(Tags.PlayerTag))
        {
            if (_volumeDir == VolumeDir.TopIsMaxVolume)
                VolumePercent = Mathf.Max(PlayerLogic.Player.transform.position.y - _colBottomY, 0) / (_colTopY - _colBottomY) * (_volumeMax - _volumeMin) + _volumeMin;
            else // BottomIsMaxVolume
                VolumePercent = Mathf.Max(_colTopY - PlayerLogic.Player.transform.position.y, 0) / (_colTopY - _colBottomY) * (_volumeMax - _volumeMin) + _volumeMin;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag(Tags.PlayerTag))
        {
            PlayerInZone = false;
            EvalVolumePercentInt();
        }
    }
}
